namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class DeliveryCodeReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "stmPort")]
      public string StmPort { get; set; }

      public DeliveryCodeReq()
      {
         StmPort = "";
      }
   }
}
