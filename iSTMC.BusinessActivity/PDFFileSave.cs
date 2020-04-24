using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using iSTMC.Common;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace iSTMC.Activity
{
   [Description("將Base64字串儲存至檔案")]
   [CanRegister]
   public sealed class PDFFileSave : KioskCodeActivityBase
   {
      [Description("待轉換JSON字串")]
      [RequiredArgument]
      public InArgument<string> JsonData { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("PDFFileSave activity is executing...");

         string asmdir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         string businessdir = System.IO.Path.GetDirectoryName(asmdir);

         string jsonData = context.GetValue(JsonData);
         string destDir = System.IO.Path.Combine(businessdir, "Outputs");

         try
         {
            if (!string.IsNullOrEmpty(jsonData))
            {
               var jsonObj = JsonConvert.DeserializeObject<IEnumerable<object>>(jsonData);

               if (jsonObj != null)
               {
                  Logger.Info($"PDFFileSave: {jsonObj.Count()} of file to be saved.");

                  foreach (JObject pdf in jsonObj)
                  {
                     string subType = pdf["subType"].ToString();
                     string data = pdf["data"].ToString();
                     string fullpath = System.IO.Path.Combine(destDir, $"PDF_{subType}.pdf");

                     byte[] bytData = Convert.FromBase64String(data);
                     Logger.Info($"{fullpath} with length {bytData.Length} is going to save...");
                     System.IO.File.WriteAllBytes(fullpath, bytData);
                  }
               }
               else
                  Logger.Error("Downloaded PDF json data is not an array of type.");
            }
            else
               Logger.Error("PDF data is not found.");
         }
         catch (Exception ex)
         {
            Logger.Error("PDFFileSave error with: {0}", ex.Message);
            Logger.Error("StackTrace: {0}", ex.StackTrace);
         }

         Logger.Info("PDFFileSave activity executed.");
      }
   }
}
