using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PdfSync
{
   public class ImagePageInfo
   {
      [XmlAttribute]
      public string FilePath { get; set; }
   }

   public class PdfImageFiles
   {
      public List<ImagePageInfo> ImagePageInfos { get; set; }

      public PdfImageFiles()
      {
         ImagePageInfos = new List<ImagePageInfo>();
      }
   }
}
