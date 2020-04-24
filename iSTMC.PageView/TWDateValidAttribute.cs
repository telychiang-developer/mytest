using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSTMC.PageView
{
   public class TWDateValidAttribute : ValidationAttribute
   {
      protected override ValidationResult IsValid(object value, ValidationContext validationContext)
      {
         string data = value != null ? value.ToString().Trim() : null;

         if (data != null && !ValidationUtils.ValidateTWDate(data))
            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
         else
            return ValidationResult.Success;
      }
   }
}
