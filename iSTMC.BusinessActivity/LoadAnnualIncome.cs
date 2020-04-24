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
   [Description("載入年收入資訊至物件")]
   [CanRegister]
   public sealed class LoadAnnualIncome : KioskCodeActivityBase
   {
      public LoadAnnualIncome() : base() { }

      [Description("年收入資訊JSON存放DataKey")]
      [RequiredArgument]
      public InArgument<string> DataCacheKey { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("LoadAnnualIncome activity is executing...");

         string dataKey = context.GetValue(DataCacheKey);

         if (this.TransactionDataCache.ContainsKey(dataKey))
         {
            string json = this.TransactionDataCache[dataKey].ToString();
            try
            {
               var annualIncomeDropdown = JsonConvert.DeserializeObject<AnnualIncomeDropdown>(json);

               this.TransactionDataCache.Set(dataKey, annualIncomeDropdown);
            }
            catch (Exception ex)
            {
               Logger.Error(ex, $"LoadAnnualIncome error: {ex.Message}");
               this.TransactionDataCache.Remove(dataKey);
            }
         }

         Logger.Info("LoadAnnualIncome activity executed.");
      }
   }
}
