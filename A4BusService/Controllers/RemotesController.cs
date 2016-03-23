// File Name : RemotesController.cs
// A remote controller that checks province code input
//
// Author : Sam Sangkyun Park
// Date Created : Nov. 17, 2015
// Revision History : Version 1 created : Nov. 17, 2015
//                  : Version 2 updated : Dec.  8, 2015 : localization

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A4BusService.Models;
using System.Text.RegularExpressions;
using A4BusService.App_GlobalResources;

namespace A4BusService.Controllers
{
    public class RemotesController : Controller
    {
        BusServiceContext db = new BusServiceContext();
        // GET: Remotes

        /// <summary>
        /// Checks provice code input
        /// Allows only two letters
        /// If the code is not on file, it shows an error
        /// When it gets exception, it shows the exception message along with proper message
        /// </summary>
        /// <param name="provinceCode">provinceCode</param>
        /// <returns>JsonResult</returns>
        public JsonResult provinceCodeCheck(string provinceCode)
        {
            Regex regex = new Regex(@"[A-Za-z][A-Za-z]");
            if(!regex.IsMatch(provinceCode))
            {
                //return Json("Province Code should be two letters", JsonRequestBehavior.AllowGet);
                return Json(
                    string.Format(SPTranslations.XShould2Letters, SPTranslations.provinceCode),
                    JsonRequestBehavior.AllowGet);
            }

            try
            {
                var province_found = db.provinces.Find(provinceCode);
                if (province_found == null)
                {
                    //return Json("Province Code is not on file", JsonRequestBehavior.AllowGet);
                    return Json(
                        string.Format(SPTranslations.XNotOnFile, SPTranslations.provinceCode),
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                //return Json("finding province threw and exception " + ex.GetBaseException().Message);
                return Json(
                    string.Format(SPTranslations.XRecordFindingException, SPTranslations.provinceCode),
                    ex.GetBaseException().Message);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}