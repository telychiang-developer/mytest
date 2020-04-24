using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class InfoCheckRep : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "verifyResult")]
      public string VerifyResult { get; set; }

      public InfoCheckRep()
      {
         VerifyResult = "";
      }
   }
}
