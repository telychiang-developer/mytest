using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PdfSync
{
   class Pdf2ImageConverter
   {
      private PdfToImageConfig _settings = new PdfToImageConfig();
      private Dictionary<string, List<string>> _pdf2ImgCache;

      public Pdf2ImageConverter()
      {
         _pdf2ImgCache = new Dictionary<string, List<string>>();
      }

      private int GetPdfPageCount(string fileName)
      {
         return Regex.Matches(File.ReadAllText(fileName), @"/Type\s*/Page[^s]").Count;
      }

      protected virtual IDictionary<GhostScriptCommand, object> GetConversionArguments(
          string pdfFileName, string outputImageFileName, int pageNum, string password, PdfToImageConfig settings)
      {
         Dictionary<GhostScriptCommand, object> arguments;

         arguments = new Dictionary<GhostScriptCommand, object>();

         arguments.Add(GhostScriptCommand.Silent, null);
         arguments.Add(GhostScriptCommand.Safer, null);
         arguments.Add(GhostScriptCommand.Batch, null);
         arguments.Add(GhostScriptCommand.NoPause, null);

         arguments.Add(GhostScriptCommand.Device, GhostScriptAPI.GetDeviceName(settings.ImageFormat));
         arguments.Add(GhostScriptCommand.OutputFile, outputImageFileName);

         arguments.Add(GhostScriptCommand.FirstPage, pageNum);
         arguments.Add(GhostScriptCommand.LastPage, pageNum);

         arguments.Add(GhostScriptCommand.UseCIEColor, null);

         if (settings.AntiAliasMode != AntiAliasMode.None)
         {
            arguments.Add(GhostScriptCommand.TextAlphaBits, settings.AntiAliasMode);
            arguments.Add(GhostScriptCommand.GraphicsAlphaBits, settings.AntiAliasMode);
         }

         arguments.Add(GhostScriptCommand.GridToFitTT, settings.GridFitMode);

         if (settings.TrimMode != PdfTrimMode.PaperSize)
            arguments.Add(GhostScriptCommand.Resolution, settings.Dpi.ToString());

         switch (settings.TrimMode)
         {
            case PdfTrimMode.PaperSize:
               if (settings.PaperSize != PaperSize.Default)
               {
                  arguments.Add(GhostScriptCommand.FixedMedia, true);
                  arguments.Add(GhostScriptCommand.PaperSize, settings.PaperSize);
               }
               break;
            case PdfTrimMode.TrimBox:
               arguments.Add(GhostScriptCommand.UseTrimBox, true);
               break;
            case PdfTrimMode.CropBox:
               arguments.Add(GhostScriptCommand.UseCropBox, true);
               break;
         }

         if (!string.IsNullOrEmpty(password))
            arguments.Add(GhostScriptCommand.PDFPassword, password);

         arguments.Add(GhostScriptCommand.InputFile, pdfFileName);

         return arguments;
      }

      public void Dispose()
      {
         if (_pdf2ImgCache != null)
         {
            _pdf2ImgCache.Clear();
         }
      }

      public List<string> ConvertPdfPageToImage(string pdfFilePath, string pdfPassword, string outputdir)
      {
         if (_pdf2ImgCache.ContainsKey(pdfFilePath))
         {
            return _pdf2ImgCache[pdfFilePath];
         }

         List<string> imgPathList = new List<string>();

         try
         {
            string exedir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string businessdir = System.IO.Path.Combine(exedir, "Business");

            string path = System.IO.Path.Combine(businessdir, outputdir);
            if (!Directory.Exists(path))
            {
               Directory.CreateDirectory(path);
            }

            int pageCount = GetPdfPageCount(pdfFilePath);
            for (int pageNum = 1; pageNum <= pageCount; pageNum++)
            {
               GhostScriptAPI api = new GhostScriptAPI();
               string _imgFileFullPath = string.Format(@"{0}\{1}{2}", path, Guid.NewGuid(), _settings.Extension);
               api.Execute(GetConversionArguments(pdfFilePath, _imgFileFullPath, pageNum, pdfPassword, _settings));
               imgPathList.Add(_imgFileFullPath);
               api.Dispose();
            }
         }
         catch (Exception ex)
         {
            return null;
         }

         _pdf2ImgCache.Add(pdfFilePath, imgPathList);

         return imgPathList;
      }

      public void ClearCache()
      {
         if (_pdf2ImgCache != null)
         {
            foreach (List<string> imgList in _pdf2ImgCache.Values)
            {
               try
               {
                  foreach (string img in imgList)
                  {
                     File.Delete(img);
                  }
               }
               catch (Exception ex)
               {
               }
            }
            _pdf2ImgCache.Clear();
         }
      }

      public void ClearCache(string pdfPath)
      {
         if (_pdf2ImgCache.ContainsKey(pdfPath))
         {
            foreach (string img in _pdf2ImgCache[pdfPath])
            {
               File.Delete(img);
            }
            _pdf2ImgCache.Remove(pdfPath);
         }
      }

      public PdfToImageConfig Settings
      {
         get { return _settings; }
      }

      //public ILogger Logger { get; set; }
      //public bool IsProtected { get; set; }
   }
}
