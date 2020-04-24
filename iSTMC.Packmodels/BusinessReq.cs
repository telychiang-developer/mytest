using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "business")]
   public class BusinessReq
   {
      public BusinessReq()
      {

      }

      public static BusinessReq FromJson(string json)
      {
         var business = JObject.Parse(json)["business"];
         var head = business["head"];
         var body = business["body"];
         MessageType mType = (MessageType)Enum.Parse(typeof(MessageType), head["messageType"].ToString());

         int subType = -1;
         if (!string.IsNullOrEmpty(head["subType"].ToString()))
            subType = int.Parse(head["subType"].ToString());

         BusinessReq req = new BusinessReq();
         req.Head = Newtonsoft.Json.JsonConvert.DeserializeObject<HeadReq>(head.ToString());

         switch (mType)
         {
            case MessageType.DeliveryCode:
               req.Body = JsonConvert.DeserializeObject<DeliveryCodeReq>(body.ToString());
               break;
            case MessageType.ConnectSendCancel:
               if (subType == (int)SubType1000.Apply)
                  req.Body = JsonConvert.DeserializeObject<ConnectionReq>(body.ToString());
               else
                  req.Body = JsonConvert.DeserializeObject<DisconnectionReq>(body.ToString());
               break;
            case MessageType.HostData:
               req.Body = JsonConvert.DeserializeObject<HostDataReq>(body.ToString());
               break;
            default:
               break;
         }

         return req;
      }

      [Newtonsoft.Json.JsonProperty(PropertyName = "head")]
      public HeadReq Head { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "body")]
      public Body Body { get; set; }
      
      public string ToJSON()
      {
         var root = new
         {
            business = this
         };

         return Newtonsoft.Json.JsonConvert.SerializeObject(root);
      }
   }
}
