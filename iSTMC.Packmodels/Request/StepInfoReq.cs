namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class StepInfoReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "timeOut")]
      public string TimeOut { get; set; }

      public StepInfoReq()
      {
         TimeOut = "30";
      }
   }
}
