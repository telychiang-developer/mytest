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
   [Description("圖片轉base64文字For 2004-3,4,9,10,12")]
   [CanRegister]
   public sealed class ImgFileToBase64Map : KioskCodeActivityBase
   {
      public ImgFileToBase64Map() : base() { }

      [Description("輸入證件檔圖片存放目錄")]
      [RequiredArgument]
      public InArgument<string> InputDir { get; set; }

      [Description("圖檔檔名")]
      [RequiredArgument]
      public InArgument<string> ImgFileName { get; set; }

      [Description("JSON欄位名稱")]
      [RequiredArgument]
      public InArgument<string> JsonFieldName { get; set; }

      [Description("轉換報文結果物件存放DataKey")]
      [RequiredArgument]
      public InArgument<string> OutputResultDataKey { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("IDFrontUVImgMap activity is executing...");

         string inputDir = context.GetValue(InputDir);
         string imgFileName = context.GetValue(ImgFileName);
         string jsonFieldName = context.GetValue(JsonFieldName);
         string outputResultDataKey = context.GetValue(OutputResultDataKey);

         string asmdir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         string businessdir = System.IO.Path.GetDirectoryName(asmdir);

         string filepath = System.IO.Path.Combine(businessdir, inputDir, imgFileName);

         byte[] bytFile = System.IO.File.ReadAllBytes(filepath);
         string base64Str = Convert.ToBase64String(bytFile);

         string json = "{ \"" + jsonFieldName + "\" : \"" + base64Str + "\" }";
         this.TransactionDataCache.Set(outputResultDataKey, json);

         Logger.Info("ImgFileToBase64Map activity executed.");
      }
   }
}
