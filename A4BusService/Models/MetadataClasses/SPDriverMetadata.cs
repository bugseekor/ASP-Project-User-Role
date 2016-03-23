// File Name : SPDriverMetadata.cs
// A metadata class modifying driver model
//
// Author : Sam Sangkyun Park
// Date Created : Oct. 28, 2015
// Revision History : Version 1 created : Oct. 28, 2015
//                  : Version 2 updated : Nov. 17, 2015 - SPClassLibrary, RemotesController, self-validation
//                  : Version 3 updated : Dec. 8, 2015 - Localization

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SPClassLibrary;
using System.Web.Mvc;
using A4BusService.App_GlobalResources;


//namespace A4BusService.Models.MetadataClasses
namespace A4BusService.Models
{
    //A partial class that uses SPDriverMetadata type
    [MetadataType(typeof(SPDriverMetadata))]
    public partial class driver : IValidatableObject
    {
        BusServiceContext db = new BusServiceContext();

        /// <summary>
        /// Model self Validate method derived from IValidatableObject
        /// </summary>
        /// <param name="validationContext">validationContext</param>
        /// <returns>IEnumerable<ValidationResult></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //First and last name are shifted to lower case & capitalised
            lastName = SPValidations.Capitalise(lastName);
            firstName = SPValidations.Capitalise(firstName);
            fullName = lastName + ", " + firstName;
            
            //Takes only digits from homePhone input by user
            homePhone = new String(homePhone.Where(Char.IsNumber).ToArray());
            if (homePhone.Length != 10)
            {
                yield return new ValidationResult(
                    //"home phone should have 10 digits",
                    string.Format(SPTranslations.XShold10Digits, SPTranslations.homePhone),
                    new string[] { SPTranslations.homePhone });
            }
            else
            {
                homePhone = homePhone.Substring(0, 3) + "-" + homePhone.Substring(3, 3) + "-" +
                    homePhone.Substring(6, 4);
            }

            if(provinceCode != null)
            {
                provinceCode = provinceCode.ToUpper();
            }

            //Postal Code is shifted to upper case and eliminates redundant spaces in the middle
            //assuming it already has proper format by class library checking [PostalCodeValidationAttribute]
            postalCode = postalCode.Substring(0, 3) + " " + postalCode.Substring(postalCode.Length - 3, 3);
            postalCode = postalCode.ToUpper();

            yield return ValidationResult.Success;
        }
    }

    //metadata class for the driver model
    // - contains the fields as targets for annotations
    public class SPDriverMetadata
    {
        //this everything but virtual members
        public int driverId { get; set; }
        
        [Display(Name = "firstName", ResourceType = typeof(SPTranslations))]
        [Required(ErrorMessageResourceType = typeof(SPTranslations), ErrorMessageResourceName = "Required")]
        public string firstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(SPTranslations), ErrorMessageResourceName = "Required")]
        [Display(Name = "lastName", ResourceType = typeof(SPTranslations))]
        public string lastName { get; set; }

        [Display(Name = "fullName", ResourceType = typeof(SPTranslations))]
        public string fullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(SPTranslations), ErrorMessageResourceName = "Required")]
        [Display(Name = "homePhone", ResourceType = typeof(SPTranslations))]
        public string homePhone { get; set; }

        [Display(Name = "workPhone", ResourceType = typeof(SPTranslations))]
        public string workPhone { get; set; }

        [Display(Name = "street", ResourceType = typeof(SPTranslations))]
        public string street { get; set; }

        [Display(Name = "city", ResourceType = typeof(SPTranslations))]
        public string city { get; set; }

        [Required(ErrorMessageResourceType = typeof(SPTranslations), ErrorMessageResourceName = "Required")]
        [Display(Name = "postalCode", ResourceType = typeof(SPTranslations))]
        //from class library inside of this solution
        [PostalCodeValidation]
        public string postalCode { get; set; }


        [Display(Name = "provinceCode", ResourceType = typeof(SPTranslations))]
        [Remote("provinceCodeCheck", "Remotes")]
        public string provinceCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(SPTranslations), ErrorMessageResourceName = "Required")]
        [DisplayFormat(DataFormatString="{0: dd MMM yyyy}", ApplyFormatInEditMode=(true))]
        [Display(Name = "dateHired", ResourceType = typeof(SPTranslations))]
        //from class library inside of this solution
        [DateNotInFuture]
        public Nullable<System.DateTime> dateHired { get; set; }

    }
}