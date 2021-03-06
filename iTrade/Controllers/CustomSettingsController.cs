﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using iTrade.Models;

namespace iTrade.Controllers
{
    public class CustomSettingsController : Controller
    {
        private StarDbContext db = new StarDbContext();

        // GET: CustomSettings
        public ActionResult Index()
        {
            return View(db.CustomSettings.ToList());
        }

        public ActionResult Outlets()
        {
            return View(db.CompanyBranches.ToList());
        }

        // GET: CustomSettings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomSetting customSetting = db.CustomSettings.Find(id);
            if (customSetting == null)
            {
                return HttpNotFound();
            }
            return View(customSetting);
        }

        // GET: CustomSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomSettingID,Name,TextValue,ModifiedBy,ModifiedOn")] CustomSetting customSetting)
        {
            if (ModelState.IsValid)
            {
                db.CustomSettings.Add(customSetting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customSetting);
        }

        // GET: CustomSettings/Edit/5
        public ActionResult Edit()
        {
            CustomSetting customSetting = db.CustomSettings.Where(x => x.Name == "CostCode").FirstOrDefault();
            if (customSetting != null)
                ViewBag.CostCodeSetting = customSetting.TextValue;
            else
                ViewBag.CostCodeSetting = "";
            return View();
        }

        // POST: CustomSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomSettingID,Name,TextValue,ModifiedBy,ModifiedOn")] CustomSetting customSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customSetting);
        }

        // GET: CustomSettings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomSetting customSetting = db.CustomSettings.Find(id);
            if (customSetting == null)
            {
                return HttpNotFound();
            }
            return View(customSetting);
        }

        // POST: CustomSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomSetting customSetting = db.CustomSettings.Find(id);
            db.CustomSettings.Remove(customSetting);
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

        public JsonResult UpdateCostCodeSetting(string IsEnabled)
        {
            CustomSetting customSetting = db.CustomSettings.Where(x => x.Name == "CostCode").FirstOrDefault();
            if (customSetting == null)
            {
                customSetting = new CustomSetting();
                customSetting.Name = "CostCode";
                customSetting.TextValue = IsEnabled;
                customSetting.ModifiedBy = User.Identity.Name;
                customSetting.ModifiedOn = DateTime.Now;
                db.CustomSettings.Add(customSetting);
                db.SaveChanges();
            }
            else
            {
                customSetting.TextValue = IsEnabled;
                customSetting.ModifiedBy = User.Identity.Name;
                customSetting.ModifiedOn = DateTime.Now;
                db.Entry(customSetting).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new
            {
                isRedirect = true
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OutletEdit(int? id)
        {
            var co = new CompanyBranch();

            if (id == null || id == 0)
            {
                co.BranchName = "Branch name";
                var comp = db.Companies.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                if (comp != null)
                {
                    co.CompID = comp.ID;
                }

                var flag = db.CompanyBranches.Any(x => x.IsDefault == true);
                if (flag)
                {
                    co.IsDefault = true;
                }
                else
                {
                    co.IsDefault = false;

                }

                co.IsActive = true;
                co.CreatedBy = User.Identity.Name;
                co.CreatedOn = DateTime.Now;

                if (ModelState.IsValid)
                {
                    db.CompanyBranches.Add(co);
                    db.SaveChanges();
                }

                return RedirectToAction("OutletEdit", new { id = co.BranchID });

            }

            co = db.CompanyBranches.Find(id);

            if (co == null)
            {
                return HttpNotFound();
            }

            return View(co);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OutletEdit(CompanyBranch comp)
        {
            comp.ModifiedBy = User.Identity.Name;
            comp.ModifiedOn = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(comp).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Outlets");
            }

            return View(comp);

        }
    }
}
