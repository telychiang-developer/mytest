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
   [Description("載入行業分類資訊至物件")]
   [CanRegister]
   public sealed class LoadIndustry : KioskCodeActivityBase
   {
      public LoadIndustry() : base() { }

      [Description("行業分類資訊JSON存放DataKey")]
      [RequiredArgument]
      public InArgument<string> DataCacheKey { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("LoadIndustry activity is executing...");

         string dataKey = context.GetValue(DataCacheKey);

         if (this.TransactionDataCache.ContainsKey(dataKey))
         {
            string json = this.TransactionDataCache[dataKey].ToString();
            try
            {
               var industryDropdown = JsonConvert.DeserializeObject<IndustryDropdown>(json);

               this.TransactionDataCache.Set(dataKey, industryDropdown);
            }
            catch (Exception ex)
            {
               Logger.Error(ex, $"LoadIndustry error: {ex.Message}");
               this.TransactionDataCache.Remove(dataKey);
            }
         }

         Logger.Info("LoadIndustry activity executed.");
      }
   }
}
