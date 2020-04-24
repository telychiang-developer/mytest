using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NSKiosk.WebSocket
{
   public class WSConnection
   {
      #region private members
      private ClientWebSocket _clientWebSocket = null;
      private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings() 
      { 
         ContractResolver = new CamelCasePropertyNamesContractResolver() 
      };
      #endregion

      #region constructor
      public WSConnection()
      {

      }
      #endregion

      #region public properties
      public string ConnectionId { get; set; }
      #endregion

      #region public events
      public event EventHandler<string> OnConnected;
      public event EventHandler<string> OnDisconnected;
      public event EventHandler<WSMessage> OnMessage;
      public event EventHandler<NotificationDescriptor> OnNotification;
      #endregion

      #region public methods
      public async Task ConnectAsync(string uri, CancellationToken cancellationToken)
      {
         if (_clientWebSocket != null)
            _clientWebSocket.Dispose();

         _clientWebSocket = new ClientWebSocket();
         _clientWebSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(10);

         await _clientWebSocket.ConnectAsync(new Uri(uri), cancellationToken).ConfigureAwait(false);

         await Receive(_clientWebSocket, (message) =>
         {
            if (message.MessageType == WSMessageType.ConnectionEvent)
            {
               if (message.Sender == "" && message.Data == "disconnected")
               {
                  string id = this.ConnectionId;
                  this.ConnectionId = "";
                  if(OnDisconnected != null)
                     Task.Factory.StartNew(() => OnDisconnected(this, id));
               }
               else
               {
                  this.ConnectionId = message.Data;
                  if(OnConnected != null)
                     Task.Factory.StartNew(() => OnConnected(this, this.ConnectionId));
               }
            }
            else if (message.MessageType == WSMessageType.Text)
            {
               if (OnMessage != null)
                  Task.Factory.StartNew(() => OnMessage(this, message));
            }
            else if (message.MessageType == WSMessageType.NotificationEvent)
            {
               if (OnNotification != null)
               {
                  Task.Factory.StartNew(() =>
                  {
                     var notifyDescriptor = JsonConvert.DeserializeObject<NotificationDescriptor>(message.Data, _jsonSerializerSettings);
                     OnNotification(this, notifyDescriptor);
                  });
               }
            }
         });
      }

      public async Task DisconnectAsync()
      {
         if (_clientWebSocket == null)
            return;

         string id = this.ConnectionId;
         this.ConnectionId = "";
         if (OnDisconnected != null)
         {
            await Task.Factory.StartNew(() =>
            {
               _clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

               OnDisconnected(this, id);
            }).ConfigureAwait(false);
         }
      }

      public async Task SendAsync(NotificationDescriptor notifyDescriptor)
      {
         if (_clientWebSocket == null || _clientWebSocket.State != WebSocketState.Open)
            return;

         var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notifyDescriptor));
         await _clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
      }

      public async Task Keepalive()
      {
         if (_clientWebSocket == null || _clientWebSocket.State != WebSocketState.Open)
            return;

         NotificationDescriptor invocationDescriptor = new NotificationDescriptor
         {
            NotifyName = "KeepAlive",
            Arguments = new object[] { ConnectionId },
            Sender = ""
         };

         var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(invocationDescriptor));
         await _clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
      }
      #endregion

      #region private methods
      private async Task Receive(ClientWebSocket clientWebSocket, Action<WSMessage> handleMessage)
      {
         while (_clientWebSocket != null && _clientWebSocket.State == WebSocketState.Open)
         {
            ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[1024 * 4]);
            string serializedMessage = null;
            WebSocketReceiveResult result = null;
            using (var ms = new MemoryStream())
            {
               do
               {
                  result = await clientWebSocket.ReceiveAsync(buffer, CancellationToken.None).ConfigureAwait(false);
                  ms.Write(buffer.Array, buffer.Offset, result.Count);
               }
               while (!result.EndOfMessage);

               ms.Seek(0, SeekOrigin.Begin);

               using (var reader = new StreamReader(ms, Encoding.UTF8))
               {
                  serializedMessage = await reader.ReadToEndAsync().ConfigureAwait(false);
               }
            }

            if (result.MessageType == WebSocketMessageType.Text)
            {
               var message = JsonConvert.DeserializeObject<WSMessage>(serializedMessage);
               handleMessage(message);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
               await _clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None).ConfigureAwait(false);

               handleMessage(new WSMessage { MessageType = WSMessageType.ConnectionEvent, Data = "disconnected", Sender = "" });

               break;
            }
         }
      }
      #endregion
   }
}
