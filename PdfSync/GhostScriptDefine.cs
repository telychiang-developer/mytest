using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfSync
{
   class GhostScriptDefine
   {
   }

   public enum ErrorCode
   {
      Success = 0,
      FatalError = -100
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
}
