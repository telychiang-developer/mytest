using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.Packmodels
{
   public class Statement
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }
   }

   public class StatementData
   {
      [JsonProperty("statement")]
      public List<Statement> Statements { get; set; }

      public StatementData()
      {
         Statements = new List<Statement>();
      }
   }

   public class StatementDropdown
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("data")]
      public StatementData Data { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
   }
}
