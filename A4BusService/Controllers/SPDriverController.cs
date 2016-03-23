// File Name : SPDriverController.cs
// A full CRUD MVC Controller that shows and creates driver record
//
// Author : Sam Sangkyun Park
// Date Created : Oct. 28, 2015
// Revision History : Version 1 created : Oct. 28, 2015
//                  : Version 2 updated : Nov. 11, 2015 - DeleteConfirmed sends its control to Index when it succeeds
//                  : Version 3 updated : Dec. 8, 2015 - Localization


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A4BusService.Models;
using SPClassLibrary;
using A4BusService.App_GlobalResources;

namespace A4BusService.Controllers
{
    public class SPDriverController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        /// <summary>
        /// set language from cookie
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

        }

        // GET: Driver
        /// <summary>
        /// Drivers listing
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var drivers = db.drivers.Include(d => d.province).OrderBy(a=>a.fullName);
            return View(drivers.ToList());
        }

        // GET: Driver/Details/5
        /// <summary>
        /// Detail ActionResult showing a driver record details
        /// </summary>
        /// <param name="id">selected driver id</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // GET: Driver/Create
        /// <summary>
        /// Create ActionResult to show input form of data
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            //ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a => a.name), "provinceCode", "name");
            return View();
        }

        // POST: Driver/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Post-Create ActionResult adding new driver record
        /// If it works, listing screen is displayed with newly added recored
        /// If not, input screen is displayed again with exception message
        /// </summary>
        /// <param name="driver">model:driver</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "driverId,firstName,lastName,fullName,homePhone,workPhone,street,city,postalCode,provinceCode,dateHired")] driver driver)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.drivers.Add(driver);
                    db.SaveChanges();
                    //TempData["message"] = "driver added: " + driver.fullName;
                    TempData["message"] = string.Format(SPTranslations.XRecordCreated, SPTranslations.Driver + driver.fullName);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                ModelState.AddModelError("",
                    string.Format(SPTranslations.XRecordCreatingException, SPTranslations.Driver + driver.fullName) +
                    ex.GetBaseException().Message);
            }

            return View(driver);
        }

        // GET: Driver/Edit/5
        /// <summary>
        /// Edit ActionResult showing a driver recored selected
        /// </summary>
        /// <param name="id">selected driver id</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a=>a.name), "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }

        // POST: Driver/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Post-Edit ActionResult to update edited data
        /// It it works, listing screen is displayed with success message
        /// It not, edit screen is displayed again with exception message
        /// </summary>
        /// <param name="driver">model:driver</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "driverId,firstName,lastName,fullName,homePhone,workPhone,street,city,postalCode,provinceCode,dateHired")] driver driver)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(driver).State = EntityState.Modified;
                    db.SaveChanges();
                    //TempData["message"] = "driver updated: " + driver.fullName;
                    TempData["message"] = string.Format(SPTranslations.XRecordEdited, SPTranslations.Driver + driver.fullName);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                //AddModelError("", "update driver threw and exception " + ex.GetBaseException().Message);
                ModelState.AddModelError("",
                    string.Format(SPTranslations.XRecordEdited, SPTranslations.Driver + driver.fullName) +
                    ex.GetBaseException().Message);
            }
            ViewBag.provinceCode = new SelectList(db.provinces.OrderBy(a=>a.name), "provinceCode", "name", driver.provinceCode);
            return View(driver);
        }

        // GET: Driver/Delete/5
        /// <summary>
        /// Delete ActionResult showing a record selected
        /// </summary>
        /// <param name="id">selected driver id</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            driver driver = db.drivers.Find(id);
            if (driver == null)
            {
                return HttpNotFound();
            }
            return View(driver);
        }

        // POST: Driver/Delete/5
        /// <summary>
        /// Post-Delete Delete ActionResult deleting a record selected
        /// </summary>
        /// <param name="id">selected driver id</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                driver driver = db.drivers.Find(id);
                db.drivers.Remove(driver);
                db.SaveChanges();
                //TempData["message"] = "driver deleted: " + driver.fullName;
                TempData["message"] = string.Format(SPTranslations.XRecordDeleted, SPTranslations.Driver + driver.fullName);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                //ModelState.AddModelError("", "delete driver threw and exception " + ex.GetBaseException().Message);
                //TempData["message"] = "delete driver threw and exception " + ex.GetBaseException().Message;
                TempData["message"] = string.Format(SPTranslations.XRecordDeletingException, SPTranslations.Driver);
            }

            //When this succeeds, Index controller is executed with success message
            //Delete(id);
            //return View();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
