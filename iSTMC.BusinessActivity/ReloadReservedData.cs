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
using iSTMC.Packmodels;

namespace iSTMC.Activity
{
   [Description("載入保留戶資料並還原個資輸入內容")]
   [CanRegister]
   public sealed class ReloadReservedData : KioskCodeActivityBase
   {
      public ReloadReservedData() : base() { }

      [Description("保留戶資料JSON")]
      [RequiredArgument]
      public InArgument<string> ReservedData { get; set; }

      [Description("個人資料第一頁DataKey(地址)")]
      [RequiredArgument]
      public InArgument<string> PersonalDataKey1 { get; set; }

      [Description("個人資料第二頁DataKey(就業資訊)")]
      [RequiredArgument]
      public InArgument<string> PersonalDataKey2 { get; set; }

      [Description("個人資料第三頁DataKey(資金來源)")]
      [RequiredArgument]
      public InArgument<string> PersonalDataKey3 { get; set; }

      [Description("個人資料第四頁DataKey(財富累積來源)")]
      [RequiredArgument]
      public InArgument<string> PersonalDataKey4 { get; set; }

      [Description("個人資料第五頁DataKey(申請金融卡及聯行代收付)")]
      [RequiredArgument]
      public InArgument<string> PersonalDataKey5 { get; set; }

      [Description("個人資料第六頁DataKey(申請個人網路銀行)")]
      [RequiredArgument]
      public InArgument<string> PersonalDataKey6 { get; set; }

      [Description("個人資料第七頁DataKey(聯徵及共同行銷條款)")]
      [RequiredArgument]
      public InArgument<string> PersonalDataKey7 { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("ReloadReservedData activity is executing...");

         string reservedData = context.GetValue(ReservedData);
         string personalDataKey1 = context.GetValue(PersonalDataKey1);
         string personalDataKey2 = context.GetValue(PersonalDataKey2);
         string personalDataKey3 = context.GetValue(PersonalDataKey3);
         string personalDataKey4 = context.GetValue(PersonalDataKey4);
         string personalDataKey5 = context.GetValue(PersonalDataKey5);
         string personalDataKey6 = context.GetValue(PersonalDataKey6);
         string personalDataKey7 = context.GetValue(PersonalDataKey7);
         
         if(string.IsNullOrEmpty(personalDataKey1))
         {
            Logger.Error($"ReloadReservedData: object of PersonalDataKey1 is null or empty!");
            throw new ApplicationException($"ReloadReservedData: object of PersonalDataKey1 is null or empty");
         }

         if (string.IsNullOrEmpty(personalDataKey2))
         {
            Logger.Error($"ReloadReservedData: object of PersonalDataKey2 is null or empty!");
            throw new ApplicationException($"ReloadReservedData: object of PersonalDataKey2 is null or empty");
         }

         if (string.IsNullOrEmpty(personalDataKey3))
         {
            Logger.Error($"ReloadReservedData: object of PersonalDataKey3 is null or empty!");
            throw new ApplicationException($"ReloadReservedData: object of PersonalDataKey3 is null or empty");
         }

         if (string.IsNullOrEmpty(personalDataKey4))
         {
            Logger.Error($"ReloadReservedData: object of PersonalDataKey4 is null or empty!");
            throw new ApplicationException($"ReloadReservedData: object of PersonalDataKey4 is null or empty");
         }

         if (string.IsNullOrEmpty(personalDataKey5))
         {
            Logger.Error($"ReloadReservedData: object of PersonalDataKey5 is null or empty!");
            throw new ApplicationException($"ReloadReservedData: object of PersonalDataKey5 is null or empty");
         }

         if (string.IsNullOrEmpty(personalDataKey6))
         {
            Logger.Error($"ReloadReservedData: object of PersonalDataKey6 is null or empty!");
            throw new ApplicationException($"ReloadReservedData: object of PersonalDataKey6 is null or empty");
         }

         if (string.IsNullOrEmpty(personalDataKey7))
         {
            Logger.Error($"ReloadReservedData: object of PersonalDataKey7 is null or empty!");
            throw new ApplicationException($"ReloadReservedData: object of PersonalDataKey7 is null or empty");
         }

         Logger.Info($"PersonalDataKey1: {personalDataKey1}");
         Logger.Info($"PersonalDataKey2: {personalDataKey2}");
         Logger.Info($"PersonalDataKey3: {personalDataKey3}");
         Logger.Info($"PersonalDataKey4: {personalDataKey4}");
         Logger.Info($"PersonalDataKey5: {personalDataKey5}");
         Logger.Info($"PersonalDataKey6: {personalDataKey6}");
         Logger.Info($"PersonalDataKey7: {personalDataKey7}");

         if (!string.IsNullOrEmpty(reservedData))
         {
            try
            {
               var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(reservedData);

               ReloadPersonalData(personalDataKey1, jsonObj);
               ReloadPersonalData(personalDataKey2, jsonObj);
               ReloadPersonalData(personalDataKey3, jsonObj);
               ReloadPersonalData(personalDataKey4, jsonObj);
               ReloadPersonalData(personalDataKey5, jsonObj);
               ReloadPersonalData(personalDataKey6, jsonObj);
               ReloadPersonalData(personalDataKey7, jsonObj);
            }
            catch (Exception ex)
            {
               Logger.Error(ex, $"ReloadReservedData catch deserializeObject error: {ex.Message}");
               throw new ApplicationException($"ReloadReservedData: deserializeObject with error: {ex.Message}");
            }
         }
         else
         {
            Logger.Error($"ReloadReservedData: object of ReservedData is null or empty!");
            throw new ApplicationException($"ReloadReservedData: object of ReservedData is null or empty");
         }

         Logger.Info("ReloadReservedData activity executed.");
      }

      private void ReloadPersonalData(string dataKey, Dictionary<string, string> jsonObj)
      {
         if(this.TransactionDataCache.ContainsKey(dataKey))
            this.TransactionDataCache.Remove(dataKey);

         if (jsonObj.ContainsKey(dataKey))
         {
            string base64Str = jsonObj[dataKey];
            byte[] bytData = Convert.FromBase64String(base64Str);
            string json = Encoding.UTF8.GetString(bytData);
            this.TransactionDataCache.Set(dataKey, json);
         }
      }
   }
}
