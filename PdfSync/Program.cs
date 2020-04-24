using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using iSTMC.Common;
using Newtonsoft.Json;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace PdfSync
{
   class Program
   {
      static TraceLogger g_log = new TraceLogger("PdfSync", "PSC", TraceLogger.LevelByName("INFO"));

      static void Main(string[] args)
      {
         int nResult = 0;
         bool bOK = false;
         string executePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
         string stmcRootPath = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(executePath));
         string configPath = System.IO.Path.Combine(stmcRootPath, @"Config\SyncConfig.xml");
         string pdfPath = System.IO.Path.Combine(stmcRootPath, @"Business\PDF");
         string pdfImgPath = System.IO.Path.Combine(stmcRootPath, @"Business\PDFImages");
         string tempPath = System.IO.Path.Combine(pdfPath, "TEMP");
         SyncConfig config = null;
         // variables for Pdf2Image use
         string cfgpath = System.IO.Path.Combine(stmcRootPath, @"Config\Pdf2ImgConfig.xml");

         g_log.TraceInfo("********");
         g_log.TraceInfo("PdfSync is starting...");

         for ( ; ; )
         {
            // 檢查configuration file存不存在，不存在則無法執行，Abort!
            if (!System.IO.File.Exists(configPath))
            {
               g_log.TraceError("ERROR: SyncConfig.xml is not found in path: {0}", System.IO.Path.GetDirectoryName(configPath));
               nResult = -1;
               break;
            }
            g_log.TraceInfo("SyncConfig.xml has been found in path: {0}", System.IO.Path.GetDirectoryName(configPath));

            try
            {
               // 連接AP server，模擬取得文件版控，目前 dummy access。
               config = ConfigSerializer<SyncConfig>.ReadFile(configPath);
               g_log.TraceInfo("SyncConfig deserialized succeed.");

               bOK = RestfulAPIRequest(ref config);
               if (!bOK)
               {
                  nResult = -2;
                  break;
               }

               // 存取成功後，更新最後存取時間回configuration file。
               config.LastUpdated = DateTime.Now;
               ConfigSerializer<SyncConfig>.WriteFile(configPath, config);
               g_log.TraceInfo("SyncConfig.xml has been rewrite and saved.");
            }
            catch (Exception ex)
            {
               g_log.TraceFatal("FATAL: SyncConfig deserialized failed with error: {0}", ex.Message);
               g_log.TraceError("Error stack trace : {0}", ex.StackTrace);
               g_log.TraceInfo("PdfSync process will be end with error...");
               nResult = -2;
               break;
            }

            // 建立TEMP目錄存放下載的文件
            g_log.TraceInfo("Create temporary folder to store downloaded pdf files...");

            if (!System.IO.Directory.Exists(tempPath))
            {
               // 目錄不存在，創建一個目錄。
               System.IO.Directory.CreateDirectory(tempPath);
            }
            else
            {
               // 目錄已存在，將目錄內所有檔案刪除。
               Array.ForEach(System.IO.Directory.GetFiles(tempPath), System.IO.File.Delete);
            }

            // 先下載文件到TEMP目錄
            g_log.TraceInfo("Start to download pdf files...");

            string PreviousName = null;      // Business\PDF\*.pdf
            string LocalName = null;
            System.IO.FileInfo fi;
            bool IsNeedUpdate = false;       // 預設文件未更版
            WebClient wc = new WebClient();

            // 先以下列方式避開客戶不明原因無信任自家的憑證。
            // 由於客戶為內網環境，所以尚可先用此方式處理，但仍需請客戶排解其根本問題。
            if (PassCertificateValidation())
            {
               ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            foreach (var ps in config.SyncPdfs)
            {
               g_log.TraceInfo("Download PDF file with url: {0}", ps.Url);
               try
               {
                  LocalName = System.IO.Path.Combine(tempPath, ps.LocalName);
                  wc.DownloadFile(ps.Url, LocalName);
                  fi = new System.IO.FileInfo(LocalName);
                  if (fi.Length <= 0)
                  {
                     // 下載下來的文件，檔案是空檔即為無效的。後面流程就中止，維持上次的文件運作。
                     g_log.TraceError("Document ({0}) is empty.", LocalName);
                     nResult = -3;
                     break;
                  }
                  else
                  {
                     // 檢查下載的文件和上次的文件是否相同，如果已經有一份不相同了，全部都要更版。
                     PreviousName = System.IO.Path.Combine(pdfPath, ps.LocalName);
                     if (!IsFileEqual(LocalName, PreviousName))
                     {
                        g_log.TraceInfo("{0}...Different!", ps.LocalName);
                        IsNeedUpdate = true;
                     }
                     else
                     {
                        g_log.TraceInfo("{0}...Same file.  Ignore.", ps.LocalName);
                     }
                  }
               }
               catch (WebException e)
               {
                  if (((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.NotFound)
                  {
                     // configuration裡設定的文件，缺一不可。後面流程中止，維持上次的文件運作。
                     g_log.TraceError("Document not found - {0}", ps.Url);
                     nResult = -3;
                     break;
                  }
               }
               g_log.TraceInfo("PDF file saved to file: {0}", ps.LocalName);
            }

            if (nResult != 0)       // error occurred, abort 
            {
               break;
            }

            if (!IsNeedUpdate)      // 所有文件未更版，無須後續Pdf2Image
            {
               g_log.TraceInfo("All PDFs unchanged, no need to update.");
            }
            else
            {
               g_log.TraceInfo("Backup current pdf files...");

               // 建立或清空BAK目錄，以存放目前的PDF文件
               string bakPath = System.IO.Path.Combine(pdfPath, "BAK");
               if (!System.IO.Directory.Exists(bakPath))
               {
                  System.IO.Directory.CreateDirectory(bakPath);
               }
               else
               {
                  Array.ForEach(System.IO.Directory.GetFiles(bakPath), System.IO.File.Delete);
               }

               // 將PDF目錄下的所有文件複製到BAK目錄保留。
               Array.ForEach(System.IO.Directory.GetFiles(pdfPath), (filepath) =>
               {
                  string fileName = System.IO.Path.GetFileName(filepath);
                  System.IO.File.Copy(filepath, System.IO.Path.Combine(bakPath, fileName));
                  g_log.TraceInfo("PDF file: {0} is backup.", fileName);
               });
               Array.ForEach(System.IO.Directory.GetFiles(pdfPath), System.IO.File.Delete);

               // 將TEMP目錄下新下載的文件複製到PDF目錄。
               Array.ForEach(System.IO.Directory.GetFiles(tempPath), (filepath) =>
               {
                  string fileName = System.IO.Path.GetFileName(filepath);
                  System.IO.File.Copy(filepath, System.IO.Path.Combine(pdfPath, fileName));
                  g_log.TraceInfo("PDF file: {0} is updated.", fileName);
               });

               // 清空PDFImages目錄。
               // 如此一來，令後面流程依pdf產生圖檔。
               g_log.TraceInfo("Delete all files in PDFImages directory...");
               Array.ForEach(System.IO.Directory.GetFiles(pdfImgPath), System.IO.File.Delete);
            }

            //  
            // POC
            string pathToXmlFile = System.IO.Path.Combine(pdfImgPath, "PdfImageFiles.xml");
            bool regenImages = false;

            if (!System.IO.File.Exists(pathToXmlFile))
            {
               g_log.TraceWarn("Miss {0}", pathToXmlFile);
               regenImages = true;
            }
            else
            {
               g_log.TraceInfo("PdfImageFiles.xml is existed and begin to check images existence...");

               PdfImageFiles pdfImageList = ConfigSerializer<PdfImageFiles>.ReadFile(pathToXmlFile);

               foreach (var p in pdfImageList.ImagePageInfos)
               {
                  if (!System.IO.File.Exists(p.FilePath))
                  {
                     g_log.TraceWarn("Miss {0}", p.FilePath);
                     regenImages = true;
                     break;
                  }
               }
            }

            if (!regenImages)
            {
               g_log.TraceInfo("All images is ready, no need to regenerate.");
               break;
            }

            g_log.TraceInfo("Clean '{0}' directory...", pdfImgPath);
            Array.ForEach(System.IO.Directory.GetFiles(pdfImgPath), System.IO.File.Delete);

            g_log.TraceInfo("Start to convert PDF to images...");

            Pdf2ImageConverter conv = new Pdf2ImageConverter();
            List<string> totalImgFiles = new List<string>();

            conv.Settings.Load(cfgpath);
            g_log.TraceInfo("Extension: {0}", conv.Settings.Extension);

            //string testPDF = System.IO.Path.Combine(pdfPath, "2.pdf");
            //List<string> imgFiles = conv.ConvertPdfPageToImage(testPDF, "", pdfImgPath);
            Array.ForEach(System.IO.Directory.GetFiles(pdfPath), (filepath) =>
            {
               g_log.TraceInfo("Converting {0} ...", filepath);
               List<string> imgFiles = conv.ConvertPdfPageToImage(filepath, "", pdfImgPath);
               g_log.TraceInfo("PDF file: {0} converted.  ({1}-page)", filepath, imgFiles.Count);
               if (imgFiles.Count > 0)
               {
                  totalImgFiles.AddRange(imgFiles);
               }
            });

            conv.Dispose();

            PdfImageFiles pdfImageFiles = new PdfImageFiles();
            foreach (string path in totalImgFiles)
            {
               pdfImageFiles.ImagePageInfos.Add(new ImagePageInfo() { FilePath = path });
            }

            ConfigSerializer<PdfImageFiles>.WriteFile(System.IO.Path.Combine(pdfImgPath, "PdfImageFiles.xml"), pdfImageFiles);

            g_log.TraceInfo("Completed to convert PDF to images.");

            break;      // normal termination of flow
         }

         // 移除TEMP目錄
         if (System.IO.Directory.Exists(tempPath))
         {
            Array.ForEach(System.IO.Directory.GetFiles(tempPath), System.IO.File.Delete);
            System.IO.Directory.Delete(tempPath);
         }

         if (nResult == 0)    // no error occurred
         {
            g_log.TraceInfo("PdfSync process finished and exit.");
         }
         else
         {
            g_log.TraceError("ERROR: PdfSync process abort with error: {0}...", nResult);
         }
      }

      static bool RestfulAPIRequest(ref SyncConfig config)
      {
         g_log.TraceInfo("RestfulAPIRequest will be executed to url: {0}", config.ApiUrl);

         try
         {
            using (HttpClient httpClient = new HttpClient())
            {
               httpClient.Timeout = TimeSpan.FromSeconds(30);
               HttpRequestMessage request = new HttpRequestMessage();
               request.Method = HttpMethod.Get;
               request.RequestUri = new Uri(config.ApiUrl);

               var response = httpClient.SendAsync(request).Result;

               g_log.TraceInfo("RestfulAPIRequest has been sent.");

               int statusCode = (int)response.StatusCode;

               if (response.IsSuccessStatusCode)
               {
                  g_log.TraceInfo("RestfulAPIRequest response succeed with statusCode: {0}", statusCode);

                  string result = response.Content.ReadAsStringAsync().Result;

                  try
                  {
                     var syncResult = JsonConvert.DeserializeObject<SyncResult>(result);

                     if (syncResult.Code == "success")
                     {
                        var syncPdf = config.SyncPdfs.SingleOrDefault(x => x.DocKey == "visaagreement");
                        if (syncPdf != null)
                        {
                           if (syncResult.Data.VisaAgreement.Name != syncPdf.DocName)
                              syncPdf.DocName = syncResult.Data.VisaAgreement.Name;
                           if (syncResult.Data.VisaAgreement.Url != syncPdf.Url)
                              syncPdf.Url = syncResult.Data.VisaAgreement.Url;

                           g_log.TraceInfo("VisaAgreement has been reconfig.");
                        }
                        else
                           g_log.TraceWarn("VisaAgreement is not in config.");

                        syncPdf = config.SyncPdfs.SingleOrDefault(x => x.DocKey == "ecodeagreement");
                        if (syncPdf != null)
                        {
                           if (syncResult.Data.eCodeAgreement.Name != syncPdf.DocName)
                              syncPdf.DocName = syncResult.Data.eCodeAgreement.Name;
                           if (syncResult.Data.eCodeAgreement.Url != syncPdf.Url)
                              syncPdf.Url = syncResult.Data.eCodeAgreement.Url;

                           g_log.TraceInfo("eCodeAgreement has been reconfig.");
                        }
                        else
                           g_log.TraceWarn("eCodeAgreement is not in config.");


                        syncPdf = config.SyncPdfs.SingleOrDefault(x => x.DocKey == "opaccagreement");
                        if (syncPdf != null)
                        {
                           if (syncResult.Data.OpaccAgreement.Name != syncPdf.DocName)
                              syncPdf.DocName = syncResult.Data.OpaccAgreement.Name;
                           if (syncResult.Data.OpaccAgreement.Url != syncPdf.Url)
                              syncPdf.Url = syncResult.Data.OpaccAgreement.Url;

                           g_log.TraceInfo("OpaccAgreement has been reconfig.");
                        }
                        else
                           g_log.TraceWarn("OpaccAgreement is not in config.");


                        syncPdf = config.SyncPdfs.SingleOrDefault(x => x.DocKey == "cardagreement");
                        if (syncPdf != null)
                        {
                           if (syncResult.Data.CardAgreement.Name != syncPdf.DocName)
                              syncPdf.DocName = syncResult.Data.CardAgreement.Name;
                           if (syncResult.Data.CardAgreement.Url != syncPdf.Url)
                              syncPdf.Url = syncResult.Data.CardAgreement.Url;

                           g_log.TraceInfo("CardAgreement has been reconfig.");
                        }
                        else
                           g_log.TraceWarn("CardAgreement is not in config.");


                        syncPdf = config.SyncPdfs.SingleOrDefault(x => x.DocKey == "webankagreement");
                        if (syncPdf != null)
                        {
                           if (syncResult.Data.WebankAgreement.Name != syncPdf.DocName)
                              syncPdf.DocName = syncResult.Data.WebankAgreement.Name;
                           if (syncResult.Data.WebankAgreement.Url != syncPdf.Url)
                              syncPdf.Url = syncResult.Data.WebankAgreement.Url;

                           g_log.TraceInfo("WebankAgreement has been reconfig.");
                        }
                        else
                           g_log.TraceWarn("WebankAgreement is not in config.");

                        return true;
                     }
                     else
                     {
                        g_log.TraceError("RestfulAPIRequest result code: {0}", syncResult.Code);
                        g_log.TraceInfo("PdfSync process will be end due to not success code.");
                        return false;
                     }
                  }
                  catch (Exception ex)
                  {
                     g_log.TraceError("RestfulAPIRequest result deserialized with error: {0}", ex.Message);
                     g_log.TraceInfo("PdfSync process will be end due to request error.");
                     return false;
                  }
               }
               else
               {
                  g_log.TraceError("RestfulAPIRequest result http fail with statusCode: {0}", statusCode);
                  g_log.TraceInfo("PdfSync process will be end due to request error.");
                  return false;
               }
            }
         }
         catch (Exception ex)
         {
            g_log.TraceError("RestfulAPIRequest executed failed with error: {0}", ex.Message);
            g_log.TraceError("Error stack trace : {0}", ex.StackTrace);
            g_log.TraceInfo("PdfSync process will be end with error...");
            return false;
         }
      }

      // 採用整份檔案SHA256雜湊法計算值比對。
      // true : two files are same
      // false : two files are different
      static bool IsFileEqual(string file1, string file2)
      {
         bool isEqual = false;

         for (; ; )          // sequential flow
         {
            // 檢查欲比對的兩個檔案是否都存在，缺一不可。
            if (!System.IO.File.Exists(file1) || !System.IO.File.Exists(file2))
            {
               break;
            }

            // 由於 SHA1 的衝突問題，Microsoft 建議您使用以 SHA256 或更好的加密方式為基礎的安全性模型。
            var hash = System.Security.Cryptography.HashAlgorithm.Create(@"SHA256");

            // 計算第一個檔案的雜湊值
            var stream = new System.IO.FileStream(file1, System.IO.FileMode.Open);
            byte[] hashByte_1 = hash.ComputeHash(stream);
            stream.Close();
            g_log.TraceInfo("{0} => [{1}]", file1, BitConverter.ToString(hashByte_1));

            // 計算第二個檔案的雜湊值
            stream = new System.IO.FileStream(file2, System.IO.FileMode.Open);
            byte[] hashByte_2 = hash.ComputeHash(stream);
            stream.Close();
            g_log.TraceInfo("{0} => [{1}]", file2, BitConverter.ToString(hashByte_2));

            // 比較兩個雜湊值
            if (BitConverter.ToString(hashByte_1) != BitConverter.ToString(hashByte_2))
            {
               break;
            }

            isEqual = true;
            break;
         }

         return isEqual;
      }

      static bool PassCertificateValidation()
      {
         bool pass = false;

         try
         {
            var appSettings = ConfigurationManager.AppSettings;
            if (appSettings.Count > 0)
            {
               if (appSettings["ServerCertificateValidation"].CompareTo("N") == 0)
               {
                  pass = true;
               }
            }
         }
         catch (ConfigurationErrorsException)
         {
            g_log.TraceError("Error reading app settings");
         }

         return pass;
      }

   }
   
}
