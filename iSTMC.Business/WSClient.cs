using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NSKiosk.WebSocket
{
   public class WSClient : IDisposable
   {
      #region private members
      private WSConnection _wsConnection;
      private WebSocketWatchDog<string> _wsWatchDog;
      private KeepAliveWatchDog<long> _kaWatchDog;
      private IObservable<string> _obsConnection;
      private IObservable<long> _obsKeepAlive;
      private IDisposable hConnectionRef;
      private IDisposable hKeepAliveRef;

      private string _authToken = "";
      private readonly string _websocket_Url;
      private readonly string _webapi_Baseurl;
      private readonly double _keepaliveRatio;
      private readonly TimeSpan _retryInterval;

      private bool bTerminate = false;
      #endregion

      #region constructor
      public WSClient(string websocket_url, string webapi_baseurl, int retryInterval = 5000, double keepaliveRatio = 0.7, bool bSocketStatus = false, bool bKeepaliveStatus = false)
      {
         _websocket_Url = websocket_url;
         _webapi_Baseurl = webapi_baseurl;
         _keepaliveRatio = keepaliveRatio;
         _retryInterval = TimeSpan.FromMilliseconds(retryInterval);
         IsShowSocketStatus = bSocketStatus;
         IsShowKeepAliveStatus = bKeepaliveStatus;

         _wsWatchDog = new WebSocketWatchDog<string>(this);
         _wsConnection = new WSConnection();

         _wsConnection.OnConnected += _wsConnection_OnConnected;
         _wsConnection.OnDisconnected += _wsConnection_OnDisconnected;
         _wsConnection.OnMessage += _wsConnection_OnMessage;
         _wsConnection.OnNotification += _wsConnection_OnNotification;

         _obsConnection = ConnectToServer();
         _kaWatchDog = new KeepAliveWatchDog<long>(this);
      }
      #endregion

      #region public properties
      public WSConnection Connection { get { return _wsConnection; } }
      public string WebSocketUrl { get { return _websocket_Url; } }
      public string WebApiBaseUrl { get { return _webapi_Baseurl; } }
      public TimeSpan RetryInterval { get { return _retryInterval; } }
      public double KeepAliveRatio { get { return _keepaliveRatio; } }
      public string AuthToken { get { return _authToken; } }
      public bool IsShowSocketStatus
      {
         get { return _wsWatchDog.IsShowDebugTrace; }
         set { _wsWatchDog.IsShowDebugTrace = value; }
      }
      public bool IsShowKeepAliveStatus
      {
         get { return _kaWatchDog.IsShowDebugTrace; }
         set { _kaWatchDog.IsShowDebugTrace = value; }
      }
      #endregion

      #region public events
      public event EventHandler OnConnecting;
      public event EventHandler OnConnectFail;
      public event EventHandler<string> OnConnected;
      public event EventHandler<bool> OnDisconnected;
      public event EventHandler<WSStatus> OnStatusOK;
      public event EventHandler<WSStatus> OnStatusError;
      public event EventHandler<WSMessage> OnMessage;
      public event EventHandler<NotificationDescriptor> OnNotification;
      #endregion

      #region public methods
      public void Dispose()
      {
         if (hKeepAliveRef != null)
         {
            hKeepAliveRef.Dispose();
            hKeepAliveRef = null;
         }

         if (hConnectionRef != null)
         {
            hConnectionRef.Dispose();
            hConnectionRef = null;
         }
      }

      public void RaiseSocketStatusEvent(bool bError, string msg)
      {
         if (!bError)
            OnStatusOK?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SocketStatus, Status = msg });
         else
            OnStatusError?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SocketStatus, Status = msg });
      }

      public void RaiseKeepAliveEvent(bool bError, string msg)
      {
         if (!bError)
            OnStatusOK?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SocketStatus, Status = msg });
         else
            OnStatusError?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SocketStatus, Status = msg });
      }

      public void Connect()
      {
         bTerminate = false;

         hConnectionRef = _obsConnection.Subscribe(_wsWatchDog);
      }

      public void Disconnect(bool bReconnect)
      {
         bTerminate = !bReconnect;

         if (hKeepAliveRef != null)
         {
            hKeepAliveRef.Dispose();
            hKeepAliveRef = null;
         }

         _wsConnection.DisconnectAsync().ConfigureAwait(false);
      }

      public bool Login(string username, string password)
      {
         using (HttpClient httpClient = new HttpClient())
         {
            httpClient.BaseAddress = new Uri(WebApiBaseUrl);
            StringContent content = null;
            Dictionary<string, string> loginResult = null;

            var loginData = new
            {
               username = username,
               password = password
            };

            string dataJson = JsonConvert.SerializeObject(loginData);
            content = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync("Auth/login", content).Result;
            if (response.IsSuccessStatusCode)
            {
               loginResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);
               if (loginResult != null && loginResult.ContainsKey("token"))
               {
                  _authToken = loginResult["token"];
                  OnStatusOK?.Invoke(this, new WSStatus() { StatusType = WSStatusType.LoginStatus, Status = AuthToken });

                  return true;
               }
               else
               {
                  OnStatusError?.Invoke(this, new WSStatus() { StatusType = WSStatusType.LoginStatus, Status = "No authorized token was found in response." });
                  return false;
               }
            }
            else
            {
               string error = response.Content != null ? response.Content.ReadAsStringAsync().Result : "";
               OnStatusError?.Invoke(this, new WSStatus() { StatusType = WSStatusType.LoginStatus, Status = $"StatusCode:{response.StatusCode.ToString()}, Error:{error}" });
               return false;
            }
         }
      }

      public void SignIn()
      {
         using (HttpClient httpClient = new HttpClient())
         {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
            httpClient.BaseAddress = new Uri(WebApiBaseUrl);
            Dictionary<string, string> signInResult = null;

            string url = $"Notification/signin?id={_wsConnection.ConnectionId}";

            var response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
               signInResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content.ReadAsStringAsync().Result);

               int keepaliveInterval;

               if (signInResult != null && signInResult.ContainsKey("keepalive"))
               {
                  if (int.TryParse(signInResult["keepalive"], out keepaliveInterval))
                     OnStatusOK?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SignInStatus, Status = $"KeepAlive interval is {keepaliveInterval * 0.7} seconds." });
                  else
                  {
                     keepaliveInterval = 5;
                     OnStatusError?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SignInStatus, Status = "The format of KeepAlive interval is invalid. Using defaut value instead." });
                  }
               }
               else
               {
                  keepaliveInterval = 5;
                  OnStatusError?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SignInStatus, Status = "No keepalive interval was found in response. Using defaut value instead." });
               }

               OnStatusOK?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SignInStatus, Status = "Signed in succeed." });

               double client_keepaliveInterval = keepaliveInterval * KeepAliveRatio;
               _obsKeepAlive = Observable.Interval(TimeSpan.FromSeconds(client_keepaliveInterval));
               hKeepAliveRef = _obsKeepAlive.Subscribe(_kaWatchDog);
            }
            else
            {
               string error = response.Content != null ? response.Content.ReadAsStringAsync().Result : "";
               OnStatusError?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SignInStatus, Status = $"StatusCode:{response.StatusCode.ToString()}, Error:{error}" });
            }
         }
      }

      public void SendMessage(string message, string[] receiptants, bool bBroadcast = false)
      {
         using (HttpClient httpClient = new HttpClient())
         {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
            httpClient.BaseAddress = new Uri(WebApiBaseUrl);

            string url = bBroadcast ? "Notification/sendall" : "Notification/send";

            StringContent content = null;

            var model = new
            {
               Message = message,
               Receiptants = bBroadcast ? new string[] { } : receiptants
            };

            string dataJson = JsonConvert.SerializeObject(model);
            content = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(url, content).Result;
            if (!response.IsSuccessStatusCode)
            {
               string error = response.Content != null ? response.Content.ReadAsStringAsync().Result : "";
               OnStatusError?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SignInStatus, Status = $"StatusCode:{response.StatusCode.ToString()}, Error:{error}" });
            }
         }
      }

      public void SendNotification(string notifyName, string[] arguments, string[] receiptants, bool bBroadcast = false)
      {
         using (HttpClient httpClient = new HttpClient())
         {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
            httpClient.BaseAddress = new Uri(WebApiBaseUrl);

            string url = bBroadcast ? "Notification/invokeall" : "Notification/invoke";

            StringContent content = null;

            var model = new
            {
               NotifyName = notifyName,
               Arguments = arguments,
               Receiptants = bBroadcast ? new string[] { } : receiptants
            };

            string dataJson = JsonConvert.SerializeObject(model);
            content = new StringContent(dataJson, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync(url, content).Result;
            if (!response.IsSuccessStatusCode)
            {
               string error = response.Content != null ? response.Content.ReadAsStringAsync().Result : "";
               OnStatusError?.Invoke(this, new WSStatus() { StatusType = WSStatusType.SignInStatus, Status = $"StatusCode:{response.StatusCode.ToString()}, Error:{error}" });
            }
         }
      }
      #endregion

      #region private methods
      private WebSocketException FetchSocketException(Exception ex)
      {
         if (ex.InnerException != null)
         {
            if (ex.InnerException is WebSocketException)
               return ex.InnerException as WebSocketException;
            else
               return FetchSocketException(ex.InnerException);
         }
         else
            return null;
      }

      private IObservable<string> ConnectToServer()
      {
         return Observable.Create<string>(async o =>
         {
            try
            {
               OnConnecting?.Invoke(this, new EventArgs());
               await _wsConnection.ConnectAsync(WebSocketUrl, CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
               string error = ex.Message;

               WebSocketException wex = FetchSocketException(ex);
               if (wex == null && ex.InnerException != null)
                  error = $"{ex.InnerException.Message}";
               else if (wex != null)
                  error = $"{wex.Message}";

               OnConnectFail?.Invoke(this, new EventArgs());

               Thread.Sleep(RetryInterval);

               o.OnError(ex);
            }

            return Disposable.Empty;
         });
      }

      private void BeginKeepAlive()
      {
         _obsKeepAlive.Subscribe(
            x => { },
            err => { },
            () => { }
         );
      }

      private void _wsConnection_OnConnected(object sender, string connId)
      {
         _wsWatchDog.OnNext("Connected.");
         _wsWatchDog.OnCompleted();

         OnConnected?.Invoke(sender, connId);
      }

      private void _wsConnection_OnDisconnected(object sender, string connId)
      {
         OnDisconnected?.Invoke(sender, !bTerminate);

         if (!bTerminate)
         {
            Task task = Task.Delay(RetryInterval).ContinueWith(t => _obsConnection.Subscribe(_wsWatchDog));

            task.Wait();
         }
         else
         {
            Dispose();
         }
      }

      private void _wsConnection_OnMessage(object sender, WSMessage message)
      {
         OnMessage?.Invoke(sender, message);
      }

      private void _wsConnection_OnNotification(object sender, NotificationDescriptor descriptor)
      {
         OnNotification?.Invoke(sender, descriptor);
      }
      #endregion
   }

   public class WebSocketWatchDog<T> : IObserver<T>
   {
      private readonly WSClient _client;

      public bool IsConnected { get; private set; }

      public bool IsShowDebugTrace { get; set; }

      public void OnCompleted()
      {
         IsConnected = true;
         if (IsShowDebugTrace)
            _client.RaiseSocketStatusEvent(false, "Obsevable of WebSocketWatchDog completed.");
      }

      public void OnError(Exception error)
      {
         if (IsShowDebugTrace)
            _client.RaiseSocketStatusEvent(true, $"Obsevable of WebSocketWatchDog error:{error.Message}");
      }

      public void OnNext(T value)
      {
         if (IsShowDebugTrace)
            _client.RaiseSocketStatusEvent(false, $"Obsevable of WebSocketWatchDog next:{value}");
      }

      public WebSocketWatchDog(WSClient client)
      {
         IsConnected = false;
         _client = client;

         IsShowDebugTrace = client.IsShowSocketStatus;
      }
   }

   public class KeepAliveWatchDog<T> : IObserver<T>
   {
      private readonly WSClient _client;

      public bool IsShowDebugTrace { get; set; }

      public KeepAliveWatchDog(WSClient client)
      {
         _client = client;

         IsShowDebugTrace = client.IsShowSocketStatus;
      }

      public void OnCompleted()
      {
         if (IsShowDebugTrace)
            _client.RaiseKeepAliveEvent(false, "Keep-alive process end.");
      }

      public void OnError(Exception error)
      {
         if (IsShowDebugTrace)
            _client.RaiseKeepAliveEvent(true, $"Keep-alive error: {error.Message}");
      }

      public void OnNext(T value)
      {
         _client.Connection.Keepalive().ConfigureAwait(false);

         if (IsShowDebugTrace)
            _client.RaiseKeepAliveEvent(false, "Keepalive packet has been sent.");
      }
   }
}