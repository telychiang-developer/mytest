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
   [Description("從OCR結果中取身分證中文姓名For 2009-4")]
   [CanRegister]
   public sealed class GetIDNameMap : KioskCodeActivityBase
   {
      public GetIDNameMap() : base() { }

      [Description("OCR辨識結果物件存放DataKey")]
      [RequiredArgument]
      public InArgument<string> OCRResultDataKey { get; set; }

      [Description("轉換報文結果物件存放DataKey")]
      [RequiredArgument]
      public InArgument<string> OutputResultDataKey { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("GetIDNameMap activity is executing...");

         string ocrResultDataKey = context.GetValue(OCRResultDataKey);
         string outputResultDataKey = context.GetValue(OutputResultDataKey);

         if (!this.TransactionDataCache.ContainsKey(ocrResultDataKey))
         {
            Logger.Error($"GetIDNameMap: object of OCRResultDataKey({ocrResultDataKey}) is not found!");
            throw new ApplicationException($"GetIDNameMap: object of OCRResultDataKey({ocrResultDataKey}) is not found!");
         }

         IDImgOCRResult ocrResult = this.TransactionDataCache[ocrResultDataKey] as IDImgOCRResult;

         string json = "{ \"custName\" : \"" + ocrResult.Name + "\" }";
         this.TransactionDataCache.Set(outputResultDataKey, json);

         Logger.Info("GetIDNameMap activity executed.");
      }
   }
}
