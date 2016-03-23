// File Name : SPLanguageController.cs
// Language selector controller for localization
//
// Author : Sam Sangkyun Park
// Date Created : Dec. 8, 2015
// Revision History : Version 1 created : Dec. 8, 2015


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A4BusService.Controllers
{
    public class SPLanguageController : Controller
    {
        /// <summary>
        /// set language from cookie or as default
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Request.Cookies["language"] != null)
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo(Request.Cookies["language"].Value);

                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture(Request.Cookies["language"].Value);
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture =
                    new System.Globalization.CultureInfo("en");

                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture("en");
            }

        }
        
        // GET: SPLanguage
        /// <summary>
        /// show available languages on dropdown menu
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeLanguage()
        {
            SelectListItem en = new SelectListItem() { Text = "English", Value = "en" };
            SelectListItem de = new SelectListItem() { Text = "Deutsche", Value = "de" };
            SelectListItem zh = new SelectListItem() { Text = "中文", Value = "zh" };

            en.Selected = true;

            SelectListItem[] language = new SelectListItem[] { en, de, zh };
            ViewBag.language = language;

            if (Request.UrlReferrer != null)
                Response.Cookies.Add(new HttpCookie("returnURL", Request.UrlReferrer.PathAndQuery));
            else
                Response.Cookies.Remove("returnURL");

            return View();
        }

        /// <summary>
        /// store selected language to cookie
        /// </summary>
        /// <param name="language"></param>
        [HttpPost]
        public void ChangeLanguage(string language)
        {
            Response.Cookies.Add(new HttpCookie("language", language));
            if (Request.Cookies["returnURL"] != null)
                Response.Redirect(Request.Cookies["returnURL"].Value);
            else
                Response.Redirect("/");
        }
    }
}