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
   [Description("載入資金來源資訊至物件")]
   [CanRegister]
   public sealed class LoadFundSource : KioskCodeActivityBase
   {
      public LoadFundSource() : base() { }

      [Description("資金來源資訊JSON存放DataKey")]
      [RequiredArgument]
      public InArgument<string> DataCacheKey { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("LoadFundSource activity is executing...");

         string dataKey = context.GetValue(DataCacheKey);

         if (this.TransactionDataCache.ContainsKey(dataKey))
         {
            string json = this.TransactionDataCache[dataKey].ToString();
            try
            {
               var fundSourceDropdown = JsonConvert.DeserializeObject<FundSourceDropdown>(json);

               this.TransactionDataCache.Set(dataKey, fundSourceDropdown);
            }
            catch (Exception ex)
            {
               Logger.Error(ex, $"LoadFundSource error: {ex.Message}");
               this.TransactionDataCache.Remove(dataKey);
            }
         }

         Logger.Info("LoadFundSource activity executed.");
      }
   }
}
