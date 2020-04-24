using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.Packmodels
{
   public class WealthySource
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("extraInput")]
      public string ExtraInput { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }
   }

   public class MoneyFrom
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("extraInput")]
      public string ExtraInput { get; set; }

      [JsonProperty("moneyFromS")]
      public List<WealthySource> WealthySources { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      public MoneyFrom()
      {
         WealthySources = new List<WealthySource>();
      }
   }

   public class WealthySourceData
   {
      [JsonProperty("moneyFrom")]
      public List<MoneyFrom> MoneyFroms { get; set; }

      public WealthySourceData()
      {
         MoneyFroms = new List<MoneyFrom>();
      }
   }

   public class WealthySourceDropdown
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("data")]
      public WealthySourceData Data { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
   }
}
