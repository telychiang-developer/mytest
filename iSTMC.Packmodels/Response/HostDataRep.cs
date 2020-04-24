using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class HostDataRep : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "verifyResult")]
      public string VerifyResult { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "data")]
      public string Data { get; set; }

      public HostDataRep()
      {
         VerifyResult = "";
         Data = "";
      }
   }
}
