namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "body")]
   public class ADLoginReq : Body
   {
      [Newtonsoft.Json.JsonProperty(PropertyName = "userId")]
      public string UserId { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "userPwd")]
      public string UserPwd { get; set; }

      public ADLoginReq()
      {
         UserId = "";
         UserPwd = "";
      }
   }
}
