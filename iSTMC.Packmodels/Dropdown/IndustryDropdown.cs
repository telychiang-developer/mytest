using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.Packmodels
{
   public class IndustrySubClass
   {
      [JsonProperty("indRiskLvl")]
      public string RiskLevel { get; set; }

      [JsonProperty("megaCode")]
      public string MegaCode { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }
   }

   public class IndustryMajorClass
   {
      [JsonProperty("indsubclass")]
      public List<IndustrySubClass> SubClasses { get; set; }

      [JsonProperty("name")]
      public string Name { get; set; }

      public IndustryMajorClass()
      {
         SubClasses = new List<IndustrySubClass>();
      }
   }

   public class IndustryCompanyType
   {
      [JsonProperty("companyType")]
      public string CompanyType { get; set; }

      [JsonProperty("indmclass")]
      public List<IndustryMajorClass> MajorClasses { get; set; }

      public IndustryCompanyType()
      {
         MajorClasses = new List<IndustryMajorClass>();
      }
   }

   public class IndustryData
   {
      [JsonProperty("companyTypes")]
      public List<IndustryCompanyType> CompanyTypes { get; set; }

      public IndustryData()
      {
         CompanyTypes = new List<IndustryCompanyType>();
      }
   }

   public class IndustryDropdown
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("data")]
      public IndustryData Data { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
   }
}
