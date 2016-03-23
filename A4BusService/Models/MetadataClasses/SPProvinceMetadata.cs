// File Name : SPDriverController.cs
// A metadata class modifying province model
//
// Author : Sam Sangkyun Park
// Date Created : Oct. 28, 2015
// Revision History : Version 1 created : Oct. 28, 2015

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SPClassLibrary;

namespace A4BusService.Models
{
    //A partial class that uses SPProvinceMetadata type
    [MetadataType(typeof(SPProvinceMetadata))]
    public partial class province
    {
    }

    //metadata class for the province model
    // - contains the fields as targets for annotations
    public class SPProvinceMetadata
    {
        public string provinceCode { get; set; }

        [Display(Name = "Province")]
        public string name { get; set; }
        public string countryCode { get; set; }
        public string taxCode { get; set; }
        public double taxRate { get; set; }
        public string capital { get; set; }
    }
}