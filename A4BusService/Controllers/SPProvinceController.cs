// File Name : SPProvinceController.cs
// A full CRUD MVC Controller that shows and creates Province
//
// Author : Sam Sangkyun Park
// Date Created : Oct. 28, 2015
// Revision History : Version 1 created : Oct. 28, 2015

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using A4BusService.Models;

namespace A4BusService.Controllers
{
    public class SPProvinceController : Controller
    {
        private BusServiceContext db = new BusServiceContext();

        // GET: Province
        /// <summary>
        /// Provinces Listing
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var provinces = db.provinces.Include(p => p.country);
            return View(provinces.ToList());
        }

        // GET: Province/Details/5
        /// <summary>
        /// Detail ActionResult showing a province record details
        /// </summary>
        /// <param name="id">selected province code</param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // GET: Province/Create
        /// <summary>
        /// Create ActionResult to show input form of data
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name");
            return View();
        }

        // POST: Province/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Post-Create ActionResult adding new province record
        /// </summary>
        /// <param name="province">model:province</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "provinceCode,name,countryCode,taxCode,taxRate,capital")] province province)
        {
            if (ModelState.IsValid)
            {
                db.provinces.Add(province);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name", province.countryCode);
            return View(province);
        }

        // GET: Province/Edit/5
        /// <summary>
        /// Edit ActionResult showing a province recored selected
        /// </summary>
        /// <param name="id">selected province code</param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name", province.countryCode);
            return View(province);
        }

        // POST: Province/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Post-Edit ActionResult to update edited data
        /// </summary>
        /// <param name="province">model:province</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "provinceCode,name,countryCode,taxCode,taxRate,capital")] province province)
        {
            if (ModelState.IsValid)
            {
                db.Entry(province).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.countryCode = new SelectList(db.countries, "countryCode", "name", province.countryCode);
            return View(province);
        }

        // GET: Province/Delete/5
        /// <summary>
        /// Delete ActionResult showing a record selected
        /// </summary>
        /// <param name="id">selected province code</param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            province province = db.provinces.Find(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // POST: Province/Delete/5
        /// <summary>
        /// Post-Delete Delete ActionResult deleting a record selected
        /// </summary>
        /// <param name="id">selected province code</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            province province = db.provinces.Find(id);
            db.provinces.Remove(province);
            db.SaveChanges();
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
