using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PdfSync
{
   class PdfToImageConfig
   {
      private AntiAliasMode m_antiAliasMode;
      private int m_dpi;
      private GridFitMode m_gridFitMode;
      private ImageFormat m_imageFormat;
      private PaperSize m_paperSize;
      private PdfTrimMode m_trimMode;
      private string m_extension;

      public const string s_PDFFile = "PDFFile";
      public const string s_path = "path";
      public const string s_imgFile = "ImgFile";
      public const string s_dpi = "Dpi";
      public const string s_antiAliasMode = "AntiAliasMode";
      public const string s_gridFitMode = "GridFitMode";
      public const string s_imageFormat = "ImageFormat";
      public const string s_paperSize = "PaperSize";
      public const string s_trimMode = "PdfTrimMode";
      public const string s_extension = "extension";
      public const string s_PDFToImgConfig = "PDFToImgConfig";

      public PdfToImageConfig()
      {
         this.Dpi = 150;
         this.GridFitMode = GridFitMode.Topological;
         this.AntiAliasMode = AntiAliasMode.High;
         this.ImageFormat = ImageFormat.Png24;
         this.PaperSize = PaperSize.Default;
         this.TrimMode = PdfTrimMode.CropBox;
      }

      [DefaultValue(typeof(AntiAliasMode), "High")]
      public AntiAliasMode AntiAliasMode
      {
         get { return m_antiAliasMode; }
         set
         {
            m_antiAliasMode = value;
         }
      }

      [DefaultValue(150)]
      public int Dpi
      {
         get { return m_dpi; }
         set
         {
            m_dpi = value;
         }
      }

      [DefaultValue(typeof(GridFitMode), "Topological")]
      public GridFitMode GridFitMode
      {
         get { return m_gridFitMode; }
         set
         {
            m_gridFitMode = value;
         }
      }

      [DefaultValue(typeof(PdfTrimMode), "CropBox")]
      public PdfTrimMode TrimMode
      {
         get { return m_trimMode; }
         set
         {
            m_trimMode = value;
         }
      }

      [DefaultValue(typeof(PaperSize), "Default")]
      public PaperSize PaperSize
      {
         get { return m_paperSize; }
         set
         {
            m_paperSize = value;
         }
      }

      [DefaultValue(typeof(ImageFormat), "Png24")]
      public ImageFormat ImageFormat
      {
         get { return m_imageFormat; }
         set
         {
            m_imageFormat = value;
         }
      }

      public string Extension
      {
         get { return m_extension; }
         set { m_extension = value; }
      }

      public bool Load(string argCfgPath)
      {
         if (!File.Exists(argCfgPath))
         {
            return false;
         }

         try
         {
            XmlDocument doc = new XmlDocument();
            doc.Load(argCfgPath);
            XmlNode root = doc.DocumentElement;

            if (root.Attributes[s_extension] != null && !string.IsNullOrWhiteSpace(root.Attributes[s_extension].Value))
               m_extension = root.Attributes[s_extension].Value;
            else
               m_extension = ".jpg";
         }
         catch (Exception ex)
         {
            return false;
         }

         return true;
      }

   }

   public enum ErrorCode
   {
      Success = 0,
      FatalError = -100
   }

   public enum AntiAliasMode
   {
      None = 0,
      Low = 1,
      Medium = 2,
      High = 4,
   }

   public enum GhostScriptCommand
   {
      ColorScreen,
      DitherPPI,
      Interpolate,
      NoInterpolate,
      TextAlphaBits,
      GraphicsAlphaBits,
      AlignToPixels,
      GridToFitTT,
      UseCIEColor,
      NoCIE,
      NoSubstituteDeviceColors,
      NoPSICC,
      NoTransparency,
      NoTN5044,
      DoPS,

      FixedMedia,
      FixedResolution,
      Orient1,
      DeviceWidthPoints,
      DeviceHeightPoint,
      DefaultPaperSize,

      DiskFonts,
      LocalFonts,
      NoCCFonts,
      NoFontMap,
      NoFontPath,
      NoPlatformFonts,
      NoNativeFontMap,
      FontMap,
      FontPath,
      SubstituteFont,
      OldCFFParser,

      GenericResourceDirectory,
      FontResourceDirectory,

      Batch,
      NoPagePrompt,
      NoPause,
      NoPrompt,
      NoCancel,
      Quiet,
      ShortErrors,
      StandardOut,
      TTYPause,

      NoDisplay,
      Device,
      OutputFile,
      IgnoreMultipleCopies,

      EPSCrop,
      EPPSFitPage,
      NoEPS,

      DefaultGrayProfile,
      DefaultRGBProfile,
      DefaultCMYKProfile,
      DeviceNProfile,
      ProofProfile,
      DeviceLinkProfile,
      NamedProfile,
      OutputICCProfile,
      RenderIntent,
      GraphicICCProfile,
      GraphicIntent,
      ImageICCProfile,
      ImageIntent,
      TextICCProfile,
      TextIntent,
      OverrideICC,
      OverrideRI,
      SourceObjectICC,
      DeviceGrayToK,
      ICCProfilesDirectory,

      DelayBind,
      PdfMarks,
      JobServer,
      NoBind,
      NoCache,
      NoGC,
      NoOuterSave,
      NoSafer,
      DelayedSave = NoSafer,
      Safer,
      Strict,
      SystemDictionaryWritable,

      FirstPage,
      LastPage,
      PDFFitPage,
      Printed,
      UseCropBox,
      UseTrimBox,
      PDFPassword,
      ShowAnnotations,
      ShowAcroForm,
      NoUserInit,
      RenderTTNotDef,

      Resolution,
      PaperSize,
      Silent,

      InputFile
   }

   public enum GridFitMode
   {
      None,
      SkipPatentedInstructions,
      Topological,
      Mixed
   }

   public enum ImageFormat
   {
      Unknown,
      BitmapMono,
      Bitmap8,
      Bitmap24,
      Jpeg,
      PngMono,
      Png8,
      Png24,
      TiffMono,
      Tiff24,
   }

   public enum PaperSize
   {
      Default,

      LedgerPortrait,
      Ledger,
      Legal,
      Letter,
      LetterSmall,
      ArchE,
      ArchD,
      ArchC,
      ArchB,
      ArchA,

      A0,
      A1,
      A2,
      A3,
      A4,
      A5,
      A6,
      A7,
      A8,
      A9,
      A10,
      B0,
      B1,
      B2,
      B3,
      B4,
      B5,
      B6,
      C0,
      C1,
      C2,
      C3,
      C4,
      C5,
      C6,

      Foolscap,
      HalfLetter,
      Hagaki
   }

   public enum PdfTrimMode
   {
      PaperSize,
      TrimBox,
      CropBox
   }

}
