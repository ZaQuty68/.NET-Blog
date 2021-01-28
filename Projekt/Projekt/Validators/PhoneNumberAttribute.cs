using Projekt.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Projekt.Validators
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public string GetErrorMessage() =>
            $"Proper phone number format is: xxx-xxx-xxx";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (User)validationContext.ObjectInstance;
            if(user.PhoneNumber == null)
            {
                return new ValidationResult(GetErrorMessage());
            }
            Regex rx = new Regex(@"^\d{3}-\d{3}-\d{3}$", RegexOptions.Compiled);
            if (rx.IsMatch(user.PhoneNumber))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(GetErrorMessage());
        }
    }
}