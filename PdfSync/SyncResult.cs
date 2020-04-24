using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PdfSync
{
   public class SyncResult
   {
      [JsonProperty("message")]
      public string Message { get; set; }
      [JsonProperty("code")]
      public string Code { get; set; }
      [JsonProperty("data")]
      public SyncData Data { get; set; }
   }

   public class SyncData
   {
      [JsonProperty("visaagreement")]
      public SyncValue VisaAgreement { get; set; }
      [JsonProperty("ecodeagreement")]
      public SyncValue eCodeAgreement { get; set; }
      [JsonProperty("opaccagreement")]
      public SyncValue OpaccAgreement { get; set; }
      [JsonProperty("cardagreement")]
      public SyncValue CardAgreement { get; set; }
      [JsonProperty("webankagreement")]
      public SyncValue WebankAgreement { get; set; }
   }

   public class SyncValue
   {
      [JsonProperty("name")]
      public string Name { get; set; }
      [JsonProperty("url")]
      public string Url { get; set; }
   }
}
