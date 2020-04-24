using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.Packmodels
{
   public class FundSource
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("extraInput")]
      public string ExtraInput { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }
   }

   public class FundSourceData
   {
      [JsonProperty("sourceOffunds")]
      public List<FundSource> FundSources { get; set; }

      public FundSourceData()
      {
         FundSources = new List<FundSource>();
      }
   }

   public class FundSourceDropdown
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("data")]
      public FundSourceData Data { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
   }
}
