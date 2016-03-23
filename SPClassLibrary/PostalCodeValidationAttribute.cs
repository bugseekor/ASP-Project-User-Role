// File Name : PostalCodeValidationAttribute.cs
// A validation class that validates user's postal code input
//
// Author : Sam Sangkyun Park
// Date Created : Nov. 17, 2015


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace SPClassLibrary
{
    /// <summary>
    /// Checking Canadian postal code format
    /// allows many spaces in the middle
    /// allows lower/upper case
    /// allows only valid first letter
    /// shows error message along with DisplayName when it is wrong
    /// </summary>
    public class PostalCodeValidationAttribute : ValidationAttribute
    {
        public PostalCodeValidationAttribute()
        {
            ErrorMessage = "{0} does not match the pattern A3A 3A3";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex pattern = new Regex(@"[A-CEGHJ-NPR-Za-ceghj-npr-z]\d[A-Za-z] *\d[A-Za-z]\d");
            if(value == null || value.ToString() == "" || pattern.IsMatch(value.ToString()))
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
