using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "business")]
   public class BusinessRep
   {
      public BusinessRep()
      {

      }

      public static BusinessRep FromJson(string json)
      {
         var business = JObject.Parse(json)["business"];
         var head = business["head"];
         var body = business["body"];
         MessageType mType = (MessageType)Enum.Parse(typeof(MessageType), head["messageType"].ToString());

         BusinessRep rep = new BusinessRep();
         rep.Head = Newtonsoft.Json.JsonConvert.DeserializeObject<HeadRep>(head.ToString());

         switch (mType)
         {
            case MessageType.ActionNotify:
               rep.Body = JsonConvert.DeserializeObject<ActionNotifyRep>(body.ToString());
               break;
            case MessageType.ConnectSendCancel:
               rep.Body = JsonConvert.DeserializeObject<ConnectionRep>(body.ToString());
               break;
            case MessageType.InfoCheck:
               rep.Body = JsonConvert.DeserializeObject<InfoCheckRep>(body.ToString());
               break;
            case MessageType.HostData:
               rep.Body = JsonConvert.DeserializeObject<HostDataRep>(body.ToString());
               break;
            default:
               break;
         }

         return rep;
      }

      [Newtonsoft.Json.JsonProperty(PropertyName = "head")]
      public HeadRep Head { get; set; }

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
