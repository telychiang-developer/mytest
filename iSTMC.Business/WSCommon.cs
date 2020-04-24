using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSKiosk.WebSocket
{
   public enum WSStatusType
   {
      LoginStatus,
      SignInStatus,
      SocketStatus,
      KeepAliveStatus
   }

   public enum WSMessageType
   {
      Text,
      NotificationEvent,
      ConnectionEvent
   }

   public class WSMessage
   {
      public WSMessageType MessageType { get; set; }
      public string Data { get; set; }
      public string Sender { get; set; }
   }

   public class WSStatus
   {
      public WSStatusType StatusType { get; set; }
      public string Status { get; set; }
   }

   public class NotificationDescriptor
   {
      [JsonProperty("notifyName")]
      public string NotifyName { get; set; }

      [JsonProperty("arguments")]
      public object[] Arguments { get; set; }

      [JsonProperty("sender")]
      public string Sender { get; set; }
   }
}
