using Newtonsoft.Json;
using iSTMC.Common;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using iSTMC.ImageOCRProtocol;

namespace iSTMC.Activity
{
   [Description("轉換身分核對資料for 2004-5")]
   [CanRegister]
   public sealed class IDDataConfirmMap : KioskCodeActivityBase
   {
      public IDDataConfirmMap() : base() { }

      [Description("身分核對資料存放DataKey")]
      [RequiredArgument]
      public InArgument<string> IDConfirmDataKey { get; set; }

      [Description("轉換報文結果物件存放DataKey")]
      [RequiredArgument]
      public InArgument<string> OutputResultDataKey { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("IDDataConfirmMap activity is executing...");

         string idConfirmDataKey = context.GetValue(IDConfirmDataKey);
         string outputResultDataKey = context.GetValue(OutputResultDataKey);

         if (!this.TransactionDataCache.ContainsKey(idConfirmDataKey))
         {
            Logger.Error($"IDDataConfirmMap: object of IDConfirmDataKey({idConfirmDataKey}) is not found!");
            throw new ApplicationException($"IDDataConfirmMap: object of IDConfirmDataKey({idConfirmDataKey}) is not found!");
         }

         string json = this.TransactionDataCache[idConfirmDataKey].ToString();

         var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

         string custName = "";
         string custEnName = "";
         string custId = "";
         string birthYear = "";
         string birthMonth = "";
         string birthDay = "";

         foreach (var key in jsonObj.Keys)
         {
            switch (key)
            {
               case "ChineseName":
                  custName = jsonObj[key];
                  break;
               case "EnglishName":
                  custEnName = jsonObj[key];
                  break;
               case "PID":
                  custId = jsonObj[key];
                  break;
               case "BirthYear":
                  birthYear = jsonObj[key];
                  break;
               case "BirthMonth":
                  birthMonth = jsonObj[key];
                  break;
               case "BirthDay":
                  birthDay = jsonObj[key];
                  break;
               default:
                  break;
            }
         }

         string birthday = string.Format("{0:D3}{1:D2}{2:D2}", int.Parse(birthYear), int.Parse(birthMonth), int.Parse(birthDay));

         string jsonData = "{ \"custName\" : \"" + custName + "\", \"custEnName\" : \""+ custEnName + "\", \"custId\" : \""+ custId + "\", \"birthday\" : \""+ birthday + "\" }";

         this.TransactionDataCache.Set(outputResultDataKey, jsonData);

         Logger.Info("IDDataConfirmMap activity executed.");
      }
   }
}
