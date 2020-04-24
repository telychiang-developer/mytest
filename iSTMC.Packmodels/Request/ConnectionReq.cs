namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class ConnectionReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "timeOut")]
      public string TimeOut { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "serUserId")]
      public string SerUserId { get; set; }

      public ConnectionReq()
      {
         TimeOut = "60";
         SerUserId = "";
      }
   }
}
