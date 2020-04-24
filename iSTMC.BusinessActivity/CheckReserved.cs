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
   [Description("判斷是否為保留戶")]
   [CanRegister]
   public sealed class CheckReserved : KioskCodeActivityBase
   {
      public CheckReserved() : base() { }

      [Description("查驗資料JSON")]
      [RequiredArgument]
      public InArgument<string> VerifyData { get; set; }

      [Description("是否為保留戶")]
      public OutArgument<bool> IsReserved { get; set; }

      [Description("保留戶原交易序號")]
      public OutArgument<string> OldBusinessId { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("CheckReserved activity is executing...");

         bool bReserved = false;  
         string oldBusiId = "";   

         string verifyData = context.GetValue(VerifyData);

         if (string.IsNullOrEmpty(verifyData))
         {
            Logger.Error($"CheckReserved: object of VerifyData is null or empty!");
            throw new ApplicationException($"CheckReserved: object of VerifyData null or empty!");
         }

         var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(verifyData);

         if(!jsonObj.ContainsKey("reservedH"))
         {
            Logger.Error($"Cannot find reservedH key in VerifyData.");
            throw new ApplicationException("reservedH is not found.");
         }

         bReserved = jsonObj["reservedH"] == "Y";

         if (bReserved)
         {
            if (!jsonObj.ContainsKey("busiIDOld"))
            {
               Logger.Error($"Cannot find busiIDOld key in VerifyData.");
               throw new ApplicationException("busiIDOld is not found.");
            }

            oldBusiId = jsonObj["busiIDOld"];

            Logger.Info($"檢查確認為保留戶，保留戶原交易序號: {oldBusiId}");
         }
         else
            Logger.Info("檢查確認並非為保留戶");

         context.SetValue(IsReserved, bReserved);
         context.SetValue(OldBusinessId, oldBusiId);

         Logger.Info("CheckReserved activity executed.");
      }
   }
}
