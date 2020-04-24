using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class ConnectionRep : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "connectionStatus")]
      public string ConnectionStatus { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "newToken")]
      public string NewToken { get; set; }

      public ConnectionRep()
      {
         ConnectionStatus = "";
         NewToken = "";
      }
   }
}
