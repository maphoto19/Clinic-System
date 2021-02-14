using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Clinic_System.Models;

namespace Clinic_System.Controllers
{
    public class ClinicController : Controller
    {
        private ClinicSysDbEntities db = new ClinicSysDbEntities();

        // GET: Clinic
        public ActionResult Index()
        {
            var healthClinics = db.HealthClinics.Include(h => h.Campu).Include(h => h.ClinicType);
            return View(healthClinics.ToList());
        }

        // GET: Clinic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HealthClinic healthClinic = db.HealthClinics.Find(id);
            if (healthClinic == null)
            {
                return HttpNotFound();
            }
            return View(healthClinic);
        }

        // GET: Clinic/Create

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name");
            ViewBag.ClinicTypeId = new SelectList(db.ClinicTypes, "Id", "TypeName");
            return View();
        }

        // POST: Clinic/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Physical_Address,ResAddress,Phone,Email,CampusId,ClinicTypeId")] HealthClinic healthClinic)
        {
            if (ModelState.IsValid)
            {
                db.HealthClinics.Add(healthClinic);
                db.SaveChanges();
                TempData["successMessage"] = $"Item successfully added";
                return RedirectToAction("Index");
            }

            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", healthClinic.CampusId);
            ViewBag.ClinicTypeId = new SelectList(db.ClinicTypes, "Id", "TypeName", healthClinic.ClinicTypeId);
            return View(healthClinic);
        }

        // GET: Clinic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HealthClinic healthClinic = db.HealthClinics.Find(id);
            if (healthClinic == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", healthClinic.CampusId);
            ViewBag.ClinicTypeId = new SelectList(db.ClinicTypes, "Id", "TypeName", healthClinic.ClinicTypeId);
            return View(healthClinic);
        }

        // POST: Clinic/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Physical_Address,ResAddress,Phone,Email,CampusId,ClinicTypeId")] HealthClinic healthClinic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(healthClinic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", healthClinic.CampusId);
            ViewBag.ClinicTypeId = new SelectList(db.ClinicTypes, "Id", "TypeName", healthClinic.ClinicTypeId);
            return View(healthClinic);
        }

        // GET: Clinic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HealthClinic healthClinic = db.HealthClinics.Find(id);
            if (healthClinic == null)
            {
                return HttpNotFound();
            }
            return View(healthClinic);
        }

        // POST: Clinic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HealthClinic healthClinic = db.HealthClinics.Find(id);
            db.HealthClinics.Remove(healthClinic);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RemoveAsset(int id)
        {
            HealthClinic healthClinic = db.HealthClinics.Find(id);
            db.HealthClinics.Remove(healthClinic);
            db.SaveChanges();
            TempData["successMessage"] = $"Asset successfully deleted";
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
