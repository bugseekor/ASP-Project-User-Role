﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace A4BusService
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //added for translation
            ClientDataTypeModelValidatorProvider.ResourceClassKey = "SPTranslations";
            DefaultModelBinder.ResourceClassKey = "SPTranslations"; 

        }

        /// <summary>
        /// set language from cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (Request.Cookies["language"] != null)
            {
                string language = Request.Cookies["language"].Value;

                System.Threading.Thread.CurrentThread.CurrentUICulture = new
                    System.Globalization.CultureInfo(language);
                System.Threading.Thread.CurrentThread.CurrentCulture =
                    System.Globalization.CultureInfo.CreateSpecificCulture(language);
            }
    }
    
}


}
