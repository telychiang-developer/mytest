namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class HostDataReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "data")]
      public string Data { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "timeOut")]
      public string TimeOut { get; set; }

      public HostDataReq()
      {
         Data = null;
         TimeOut = "60";
      }
   }
}
