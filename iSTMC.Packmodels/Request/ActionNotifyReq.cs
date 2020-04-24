namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class ActionNotifyReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "timeOut")]
      public string TimeOut { get; set; }

      public ActionNotifyReq()
      {
         TimeOut = "60";
      }
   }
}
