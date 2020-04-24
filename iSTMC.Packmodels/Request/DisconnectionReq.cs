namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class DisconnectionReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "reqStatus")]
      public string ReqStatus { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "timeOut")]
      public string TimeOut { get; set; }

      public DisconnectionReq()
      {
         ReqStatus = "1";
         TimeOut = "60";
      }
   }
}
