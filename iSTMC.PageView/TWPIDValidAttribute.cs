using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace iSTMC.PageView
{
   public class TWPIDValidAttribute : ValidationAttribute
   {
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
         string pid = value != null ? value.ToString().Trim() : null;

         // TW ID有可能輸入超過10碼,加判斷超過時,只檢核前10碼要符合ID規格
         if (string.IsNullOrWhiteSpace(pid) == false && pid.Length > 10)
            pid = pid.Substring(0, 10);

         if (!ValidationUtils.ValidateTWPID(pid))
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
         else
            return ValidationResult.Success;
      }
   }
}
