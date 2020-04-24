using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.Packmodels
{
   public class JobPosition
   {
      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("riskLvl")]
      public string RiskLevel { get; set; }

      [JsonProperty("sn")]
      public string SN { get; set; }
   }

   public class IndustryPosition
   {
      [JsonProperty("indRiskLvl")]
      public string RiskLevel { get; set; }

      [JsonProperty("position")]
      public List<JobPosition> JobPositions { get; set; }

      public IndustryPosition()
      {
         JobPositions = new List<JobPosition>();
      }
   }

   public class JobPositionData
   {
      [JsonProperty("indPosition")]
      public List<IndustryPosition> IndustryPositions { get; set; }

      public JobPositionData()
      {
         IndustryPositions = new List<IndustryPosition>();
      }
   }

   public class JobPositionDropdown
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("data")]
      public JobPositionData Data { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
   }
}
