using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.Packmodels
{
   public class MonthlyAvg
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }
   }

   public class MonthlyAvgData
   {
      [JsonProperty("expectedMonthly")]
      public List<MonthlyAvg> MonthlyAvgs { get; set; }

      public MonthlyAvgData()
      {
         MonthlyAvgs = new List<MonthlyAvg>();
      }
   }

   public class MonthlyAvgDropdown
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("data")]
      public MonthlyAvgData Data { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
   }
}
