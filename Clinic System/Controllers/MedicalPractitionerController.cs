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
    public class MedicalPractitionerController : Controller
    {
        private ClinicSysDbEntities db = new ClinicSysDbEntities();

        // GET: MedicalPractitioner
        public ActionResult Index()
        {
            var medicalPractitioners = db.MedicalPractitioners.Include(m => m.Gender).Include(m => m.Race).Include(m => m.Speciality).Include(m => m.Type);
            return View(medicalPractitioners.ToList());
        }

        // GET: MedicalPractitioner/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalPractitioner medicalPractitioner = db.MedicalPractitioners.Find(id);
            if (medicalPractitioner == null)
            {
                return HttpNotFound();
            }
            return View(medicalPractitioner);
        }

        // GET: MedicalPractitioner/Create
        public ActionResult Create()
        {
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name");
            ViewBag.RaceId = new SelectList(db.Races, "Id", "Type");
            ViewBag.SpecialityId = new SelectList(db.Specialities, "Id", "Name");
            ViewBag.PractitionerTypeId = new SelectList(db.Types, "Id", "Name");
            return View();
        }

        // POST: MedicalPractitioner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Cell,Email,Address,RaceId,GenderId,IsRegistered,SpecialityId,PractitionerTypeId")] MedicalPractitioner medicalPractitioner)
        {
            if (ModelState.IsValid)
            {
                db.MedicalPractitioners.Add(medicalPractitioner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", medicalPractitioner.GenderId);
            ViewBag.RaceId = new SelectList(db.Races, "Id", "Type", medicalPractitioner.RaceId);
            ViewBag.SpecialityId = new SelectList(db.Specialities, "Id", "Name", medicalPractitioner.SpecialityId);
            ViewBag.PractitionerTypeId = new SelectList(db.Types, "Id", "Name", medicalPractitioner.PractitionerTypeId);
            return View(medicalPractitioner);
        }

        // GET: MedicalPractitioner/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalPractitioner medicalPractitioner = db.MedicalPractitioners.Find(id);
            if (medicalPractitioner == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", medicalPractitioner.GenderId);
            ViewBag.RaceId = new SelectList(db.Races, "Id", "Type", medicalPractitioner.RaceId);
            ViewBag.SpecialityId = new SelectList(db.Specialities, "Id", "Name", medicalPractitioner.SpecialityId);
            ViewBag.PractitionerTypeId = new SelectList(db.Types, "Id", "Name", medicalPractitioner.PractitionerTypeId);
            return View(medicalPractitioner);
        }

        // POST: MedicalPractitioner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Cell,Email,Address,RaceId,GenderId,IsRegistered,SpecialityId,PractitionerTypeId")] MedicalPractitioner medicalPractitioner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalPractitioner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", medicalPractitioner.GenderId);
            ViewBag.RaceId = new SelectList(db.Races, "Id", "Type", medicalPractitioner.RaceId);
            ViewBag.SpecialityId = new SelectList(db.Specialities, "Id", "Name", medicalPractitioner.SpecialityId);
            ViewBag.PractitionerTypeId = new SelectList(db.Types, "Id", "Name", medicalPractitioner.PractitionerTypeId);
            return View(medicalPractitioner);
        }

        // GET: MedicalPractitioner/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalPractitioner medicalPractitioner = db.MedicalPractitioners.Find(id);
            if (medicalPractitioner == null)
            {
                return HttpNotFound();
            }
            return View(medicalPractitioner);
        }

        // POST: MedicalPractitioner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalPractitioner medicalPractitioner = db.MedicalPractitioners.Find(id);
            db.MedicalPractitioners.Remove(medicalPractitioner);
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
