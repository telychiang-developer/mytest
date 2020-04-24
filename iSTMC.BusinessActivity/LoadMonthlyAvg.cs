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
   [Description("載入預期月平均交易金額資訊至物件")]
   [CanRegister]
   public sealed class LoadMonthlyAvg : KioskCodeActivityBase
   {
      public LoadMonthlyAvg() : base() { }

      [Description("預期月平均交易金額資訊JSON存放DataKey")]
      [RequiredArgument]
      public InArgument<string> DataCacheKey { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("LoadMonthlyAvg activity is executing...");

         string dataKey = context.GetValue(DataCacheKey);

         if (this.TransactionDataCache.ContainsKey(dataKey))
         {
            string json = this.TransactionDataCache[dataKey].ToString();
            try
            {
               var monthlyAvgDropdown = JsonConvert.DeserializeObject<MonthlyAvgDropdown>(json);

               this.TransactionDataCache.Set(dataKey, monthlyAvgDropdown);
            }
            catch (Exception ex)
            {
               Logger.Error(ex, $"LoadMonthlyAvg error: {ex.Message}");
               this.TransactionDataCache.Remove(dataKey);
            }
         }

         Logger.Info("LoadMonthlyAvg activity executed.");
      }
   }
}
