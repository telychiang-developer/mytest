using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace iSTMC.Packmodels
{
   [Newtonsoft.Json.JsonObject(Title = "head")]
   public class HeadRep
   {

      public HeadRep()
      {

      }

      [Newtonsoft.Json.JsonProperty(PropertyName = "messageType")]
      public string MessageType { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "stmNo")]
      public string StmNo { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "time")]
      public string Time { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "busiId")]
      public string BusiId { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "responseStatus")]
      public string ResponseStatus { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "responseStatusMsg")]
      public string ResponseStatusMsg { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "reserveField")]
      public string ReserveField { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "txseq")]
      public string TxSeq { get; set; }

      [Newtonsoft.Json.JsonProperty(PropertyName = "subType")]
      public string SubType { get; set; }
   }
}
