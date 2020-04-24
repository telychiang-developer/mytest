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
   [Description("判斷是否需設定通提密碼")]
   [CanRegister]
   public sealed class CheckNeedPswd : KioskCodeActivityBase
   {
      public CheckNeedPswd() : base() { }

      [Description("申請資料存放DataKey")]
      [RequiredArgument]
      public InArgument<string> ApplyDataKey { get; set; }

      [Description("是否需設定通提密碼")]
      public OutArgument<bool> RequirePassword { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("CheckNeedPswd activity is executing...");

         bool bApplyUniPayment = false;  

         string applyDataKey = context.GetValue(ApplyDataKey);

         if (!this.TransactionDataCache.ContainsKey(applyDataKey))
         {
            Logger.Error($"CheckNeedPswd: object of ApplyDataKey({applyDataKey}) is not found!");
            throw new ApplicationException($"CheckNeedPswd: object of ApplyDataKey({applyDataKey}) is not found!");
         }

         string json = this.TransactionDataCache[applyDataKey].ToString();
         var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

         if(!jsonObj.ContainsKey("ApplyUniPayment"))
         {
            Logger.Error($"Cannot find ApplyUniPayment key in {applyDataKey} with json: {json}");
            throw new ApplicationException("ApplyUniPayment is not found.");
         }

         bApplyUniPayment = jsonObj["ApplyUniPayment"] == "Y";

         if (bApplyUniPayment)
            Logger.Info("檢查確認有申請申請聯行代收付，須設定通提密碼");
         else
            Logger.Info("檢查確認沒有申請申請聯行代收付，不須設定通提密碼");

         context.SetValue(RequirePassword, bApplyUniPayment);

         Logger.Info("CheckNeedPswd activity executed.");
      }
   }
}
