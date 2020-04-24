using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PdfSync
{
   public class SyncConfig
   {
      [XmlAttribute]
      public string ApiUrl { get; set; }
      [XmlAttribute]
      public DateTime LastUpdated { get; set; }
      [XmlArray]
      public SyncPdf[] SyncPdfs { get; set; }
   }

   public class SyncPdf
   {
      [XmlAttribute]
      public string DocKey { get; set; }
      [XmlAttribute]
      public string DocName { get; set; }
      [XmlAttribute]
      public string Url { get; set; }
      [XmlAttribute]
      public string LocalName { get; set; }
   }
}
