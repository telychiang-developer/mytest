namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class InfoCheckReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "timeOut")]
      public string TimeOut { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "data")]
      public string Data { get; set; }

      public InfoCheckReq()
      {
         TimeOut = "60";
         Data = "";
      }
   }
}
