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
   [Description("載入財富來源資訊至物件")]
   [CanRegister]
   public sealed class LoadWealthySource : KioskCodeActivityBase
   {
      public LoadWealthySource() : base() { }

      [Description("財富來源資訊JSON存放DataKey")]
      [RequiredArgument]
      public InArgument<string> DataCacheKey { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("LoadWealthySource activity is executing...");

         string dataKey = context.GetValue(DataCacheKey);

         if (this.TransactionDataCache.ContainsKey(dataKey))
         {
            string json = this.TransactionDataCache[dataKey].ToString();
            try
            {
               var wealthySourceDropdown = JsonConvert.DeserializeObject<WealthySourceDropdown>(json);

               this.TransactionDataCache.Set(dataKey, wealthySourceDropdown);
            }
            catch (Exception ex)
            {
               Logger.Error(ex, $"LoadWealthySource error: {ex.Message}");
               this.TransactionDataCache.Remove(dataKey);
            }
         }

         Logger.Info("LoadWealthySource activity executed.");
      }
   }
}
