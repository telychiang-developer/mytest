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

namespace iSTMC.Activity
{
   [Description("檢查是否符合續保條件")]
   [CanRegister]
   public sealed class VerifyCase : KioskCodeActivityBase
   {
      public VerifyCase() : base() { }

      [Description("輸入續保方案查詢結果")]
      public InArgument<string> ProductData { get; set; }

      [Description("是否符合續保條件")]
      public OutArgument<bool> IsAvailable { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("VerifyCase activity is executing...");

         string productData = context.GetValue(ProductData);

         var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(productData);

         var products = jsonObj["caseList"] as IEnumerable<object>;

         bool bAvailable = false;

         if (products != null && products.Count() > 0)
            bAvailable = true;

         this.IsAvailable.Set(context, bAvailable);

         Logger.Info("VerifyCase activity executed.");
      }
   }
}
