using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.Packmodels
{
   public class AddressTown
   {
      [JsonProperty("name")]
      public string Name { get; set; }
      [JsonProperty("zipcode")]
      public string ZipCode { get; set; }
   }

   public class AddressCity
   {
      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("town")]
      public List<AddressTown> Towns { get; set; }

      public AddressCity()
      {
         Towns = new List<AddressTown>();
      }
   }

   public class AddressData
   {
      [JsonProperty("adr")]
      public List<AddressCity> Cities { get; set; }

      public AddressData()
      {
         Cities = new List<AddressCity>();
      }
   }

   public class AddressDropdown
   {
      [JsonProperty("code")]
      public string Code { get; set; }

      [JsonProperty("data")]
      public AddressData Data { get; set; }

      [JsonProperty("message")]
      public string Message { get; set; }
   }
}
