using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.Packmodels
{
   public class Purpose
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }
   }

   public class PurposeData
   {
      [JsonProperty("purposeOfacc")]
      public List<Purpose> Purposes { get; set; }

      public PurposeData()
      {
         Purposes = new List<Purpose>();
      }
   }

   public class PurposeDropdown
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("data")]
      public PurposeData Data { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
   }
}
