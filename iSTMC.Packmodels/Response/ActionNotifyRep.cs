using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class ActionNotifyRep : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "sendCmdType")]
      public string SendCmdType { get; set; }

      public ActionNotifyRep()
      {
         SendCmdType = "";
      }
   }
}
