namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class BeginTransReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "stmNo2")]
      public string StmNo2 { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "timeOut")]
      public string TimeOut { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "data")]
      public string Data { get; set; }

      public BeginTransReq()
      {
         StmNo2 = "";
         TimeOut = "60";
         Data = "";
      }
   }
}
