// File Name : DateNotInFutureAttribute.cs
// A validation class that validates user's "date hired" input
//
// Author : Sam Sangkyun Park
// Date Created : Nov. 17, 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SPClassLibrary
{
    /// <summary>
    /// Allows null and empty string
    /// Input date should be some day before today(now)
    /// otherwise, it shows error message along with attribute display name
    /// </summary>
    public class DateNotInFutureAttribute : ValidationAttribute
    {
        public DateNotInFutureAttribute()
        {
            ErrorMessage = "{0} cannot be in the future";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value.ToString() == "" || (DateTime)value <= DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(string.Format(ErrorMessage, validationContext.DisplayName));
            }

        }
    }
}
