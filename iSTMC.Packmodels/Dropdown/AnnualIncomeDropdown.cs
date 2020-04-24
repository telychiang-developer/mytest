using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.Packmodels
{
   public class AnnualIncome
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }
   }

   public class AnnualIncomeData
   {
      [JsonProperty("annuIncome")]
      public List<AnnualIncome> AnnualIncomes { get; set; }

      public AnnualIncomeData()
      {
         AnnualIncomes = new List<AnnualIncome>();
      }
   }

   public class AnnualIncomeDropdown
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("data")]
      public AnnualIncomeData Data { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
   }
}
