namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class IdentityDataSaveReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "data")]
      public string Data { get; set; }

      public IdentityDataSaveReq()
      {
         Data = "";
      }
   }
}

