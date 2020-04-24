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
   [Description("轉換證件OCR欄位For 2004-2,9,11報文使用")]
   [CanRegister]
   public sealed class IDOCRResultMap : KioskCodeActivityBase
   {
      public IDOCRResultMap() : base() { }

      [Description("輸入證件檔圖片存放目錄")]
      [RequiredArgument]
      public InArgument<string> InputDir { get; set; }

      [Description("輸入證件類型(1: 身分證正面, 2: 身分證反面, 3: 第二證件")]
      [RequiredArgument]
      public InArgument<int> ImageType { get; set; }

      [Description("證件正或反面白光照檔名")]
      [RequiredArgument]
      public InArgument<string> FileName { get; set; }

      [Description("OCR辨識結果物件存放DataKey")]
      [RequiredArgument]
      public InArgument<string> OCRResultDataKey { get; set; }

      [Description("轉換報文結果物件存放DataKey")]
      [RequiredArgument]
      public InArgument<string> OutputResultDataKey { get; set; }

      protected override void Execute(CodeActivityContext context)
      {
         base.Execute(context);

         Logger.Info("IDOCRResultMap activity is executing...");

         string inputDir = context.GetValue(InputDir);
         int imgType = context.GetValue(ImageType);
         string fileName = context.GetValue(FileName);
         string ocrResultDataKey = context.GetValue(OCRResultDataKey);
         string outputResultDataKey = context.GetValue(OutputResultDataKey);

         string asmdir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         string businessdir = System.IO.Path.GetDirectoryName(asmdir);

         IDImgOCRResult ocrResult = null;

         if (this.TransactionDataCache.ContainsKey(ocrResultDataKey))
         {
            ocrResult = this.TransactionDataCache[ocrResultDataKey] as IDImgOCRResult;
         }
         else
            Logger.Warn($"IDOCRResultMap: object of OCRResultDataKey({ocrResultDataKey}) is not found!");


         string filepath = System.IO.Path.Combine(businessdir, inputDir, fileName);

         if (!System.IO.File.Exists(filepath))
         {
            Logger.Warn($"IDOCRResultMap: File not found with: {filepath}");
            throw new ApplicationException($"IDOCRResultMap: File not found with: {filepath}");
         }

         /*
          * 若使用此System.IO.File.ReadAllBytes讀檔取得檔案內容，並轉換Base64資料暫存於DataCache+後續需要進行反序列化時，在弱點掃描會有高風險問題(Deserialization of untrusted data)
          * byte[] bytFile = System.IO.File.ReadAllBytes(filepath);
          * string base64Str = Convert.ToBase64String(bytFile);
          * this.TransactionDataCache.Set("__IDImgOCRResult", json);
          * var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIDImgOCRResult);
          * -----------------------------------------------------------
          * 為了避免上述的高風險問題，需調整為以下方式讀檔。
          */

         FileStream fileStream = null;
         byte[] bytFile = null;
         string base64Str = "";
         try
         {
            // 讀取證件圖檔
            fileStream = new FileStream(filepath, FileMode.Open);
            bytFile = new byte[fileStream.Length];
            fileStream.Read(bytFile, 0, bytFile.Length);

            // 證件圖檔轉換base64
            base64Str = Convert.ToBase64String(bytFile);
         }
         catch (Exception ex)
         {
            Logger.Warn("證件圖檔轉換base64失敗。");
            Logger.Error(ex, ex.Message);
         }
         finally
         {
            if (fileStream != null)
            {
               fileStream.Close();
               fileStream.Dispose();
            }
         }

         string issueDate = "";
         DateTime? issued = null;
         DateTime? dob = null;
         string issueLoc = "";
         string issueFlag = "";

         if (ocrResult != null)
         {
            try
            {
               issueDate = string.IsNullOrEmpty(ocrResult.Issued) ? "" : ocrResult.Issued.Substring(0, ocrResult.Issued.IndexOf("("));
               issued = ocrResult.GetDateString(issueDate);
               dob = ocrResult.GetDateString(ocrResult.DateOfBirth);

               if (ocrResult.Issued.IndexOf("(") > -1 && ocrResult.Issued.IndexOf(")") > -1)
               {
                  int start = ocrResult.Issued.IndexOf("(") + 1;
                  int end = ocrResult.Issued.LastIndexOf(")") - start;

                  issueLoc = ocrResult.Issued.Substring(start, end);
               }

               if (ocrResult.Issued.IndexOf(")") > -1)
                  issueFlag = ocrResult.Issued.Substring(ocrResult.Issued.IndexOf(")") + 1);
            }
            catch (Exception ex)
            {
               Logger.Warn("OCR辨識結果解析失敗，使用空白預設值。");
               Logger.Error(ex, ex.Message);
            }
         }

         string issueFlagType = "";
         if (issueFlag == "初發")
            issueFlagType = "1";
         else if (issueFlag == "補發")
            issueFlagType = "2";
         else if (issueFlag == "換發")
            issueFlagType = "3";

         if (imgType == 1)
         {
            if (this.TransactionDataCache.ContainsKey("__IDImgOCRResult"))
               this.TransactionDataCache.Remove("__IDImgOCRResult");

            var model = new
            {
               posWhiIdPic = base64Str,
               custName = ocrResult != null ? ocrResult.Name : "",
               custId = ocrResult != null ? ocrResult.ID : "",
               birthday = dob != null && dob.HasValue ? string.Format("{0:D3}{1:D2}{2:D2}", dob.Value.Year - 1911, dob.Value.Month, dob.Value.Day) : "",
               iDIssueDt = issued != null && issued.HasValue ? string.Format("{0:D3}{1:D2}{2:D2}", issued.Value.Year - 1911, issued.Value.Month, issued.Value.Day) : "",
               iDIssueLoc = issueLoc,
               iDIssueFlag = issueFlagType,
               iDPicFlag = "1"
            };

            this.TransactionDataCache.Set(outputResultDataKey, JsonConvert.SerializeObject(model));
         }
         else if (imgType == 2)
         {
            var model = new
            {
               negWhiIdPic = base64Str,
               address = ocrResult != null ? ocrResult.Address : ""
            };

            this.TransactionDataCache.Set(outputResultDataKey, JsonConvert.SerializeObject(model));
         }
         else if (imgType == 3)
         {
            var model = new
            {
               secPic = base64Str,
               secCert = "H",
               secCertId = ocrResult != null ? ocrResult.SN : ""
            };

            this.TransactionDataCache.Set(outputResultDataKey, JsonConvert.SerializeObject(model));
         }

         if (!this.TransactionDataCache.ContainsKey("__IDImgOCRResult"))
         {
            var ocrdata = new
            {
               CustName = ocrResult != null ? ocrResult.Name : "",
               Birthday = dob != null && dob.HasValue ? dob.Value.ToString("yyyy/MM/dd") : "",
               IssueDate = issued != null && issued.HasValue ? issued.Value.ToString("yyyy/MM/dd") : "",
               IssueLoc = issueLoc,
               IssueFlag = issueFlagType,
               HasPhoto = "Y",
               PID = ocrResult != null ? ocrResult.ID : "",
               Address = ocrResult != null ? ocrResult.Address : "",
               SecondIDType = "H",
               SecondIDSN = ocrResult != null ? ocrResult.SN : ""
            };

            string json = JsonConvert.SerializeObject(ocrdata);
            this.TransactionDataCache.Set("__IDImgOCRResult", json);
         }
         else
         {
            if (this.TransactionDataCache["__IDImgOCRResult"] == null)
               throw new ApplicationException("__IDImgOCRResult is null!");

            string jsonIDImgOCRResult = this.TransactionDataCache["__IDImgOCRResult"].ToString();
            if (string.IsNullOrWhiteSpace(jsonIDImgOCRResult))
               throw new ApplicationException("__IDImgOCRResult is empty!");

            var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonIDImgOCRResult);

            string json = "";
            if (imgType == 1)
            {
               var ocrdata = new
               {
                  CustName = jsonObj["CustName"],
                  Birthday = jsonObj["Birthday"],
                  IssueDate = jsonObj["IssueDate"],
                  IssueLoc = jsonObj["IssueLoc"],
                  IssueFlag = jsonObj["IssueFlag"],
                  HasPhoto = jsonObj["HasPhoto"],
                  PID = jsonObj["PID"],
                  Address = jsonObj["Address"],
                  SecondIDType = jsonObj["SecondIDType"],
                  SecondIDSN = jsonObj["SecondIDSN"]
               };

               json = JsonConvert.SerializeObject(ocrdata);
               this.TransactionDataCache.Set("__IDImgOCRResult", json);
            }
            else
            {
               if (!this.TransactionDataCache.ContainsKey("OCRConfirmData"))
                  throw new ApplicationException("OCRConfirmData is not found!");

               string jsonOCRConfirm = this.TransactionDataCache["OCRConfirmData"].ToString();
               var jsonObjOCRConfirm = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonOCRConfirm);

               if (imgType == 2)
               {
                  var ocrdata = new
                  {
                     CustName = jsonObjOCRConfirm["ChineseName"],
                     Birthday = jsonObjOCRConfirm["BirthYear"] + jsonObjOCRConfirm["BirthMonth"] + jsonObjOCRConfirm["BirthDay"],
                     IssueDate = jsonObj["IssueDate"],
                     IssueLoc = jsonObj["IssueLoc"],
                     IssueFlag = jsonObj["IssueFlag"],
                     HasPhoto = jsonObj["HasPhoto"],
                     PID = jsonObjOCRConfirm["PID"],
                     Address = ocrResult != null ? ocrResult.Address : "",
                     SecondIDType = jsonObj["SecondIDType"],
                     SecondIDSN = jsonObj["SecondIDSN"]
                  };

                  json = JsonConvert.SerializeObject(ocrdata);
                  this.TransactionDataCache.Set("__IDImgOCRResult", json);
               }
               else if (imgType == 3)
               {
                  var ocrdata = new
                  {
                     CustName = jsonObjOCRConfirm["ChineseName"],
                     Birthday = jsonObjOCRConfirm["BirthYear"] + jsonObjOCRConfirm["BirthMonth"] + jsonObjOCRConfirm["BirthDay"],
                     IssueDate = jsonObj["IssueDate"],
                     IssueLoc = jsonObj["IssueLoc"],
                     IssueFlag = jsonObj["IssueFlag"],
                     HasPhoto = jsonObj["HasPhoto"],
                     PID = jsonObjOCRConfirm["PID"],
                     Address = jsonObj["Address"],
                     SecondIDType = jsonObj["SecondIDType"],
                     SecondIDSN = ocrResult != null ? ocrResult.SN : ""
                  };

                  json = JsonConvert.SerializeObject(ocrdata);
                  this.TransactionDataCache.Set("__IDImgOCRResult", json);
               }
            }
         }

         Logger.Info("IDOCRResultMap activity executed.");

      }
   }
}
