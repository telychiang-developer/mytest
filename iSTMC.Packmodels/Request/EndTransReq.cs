namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class EndTransReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "data")]
      public string Data { get; set; }

      public EndTransReq()
      {
         Data = "";
      }
   }
}
