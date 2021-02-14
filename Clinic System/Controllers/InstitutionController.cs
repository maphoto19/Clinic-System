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
    public class InstitutionController : Controller
    {
        private ClinicSysDbEntities db = new ClinicSysDbEntities();

        // GET: Institution
        public ActionResult Index()
        {
            var institutions = db.Institutions.Include(i => i.Campu).Include(i => i.InstitutionType);
            return View(institutions.ToList());
        }

        // GET: Institution/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            return View(institution);
        }

        // GET: Institution/Create
        public ActionResult Create()
        {
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.InstitutionTypes, "Id", "Type");
            return View();
        }

        // POST: Institution/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CampusId,Address,TypeId")] Institution institution)
        {
            if (ModelState.IsValid)
            {
                db.Institutions.Add(institution);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", institution.CampusId);
            ViewBag.TypeId = new SelectList(db.InstitutionTypes, "Id", "Type", institution.TypeId);
            return View(institution);
        }

        // GET: Institution/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", institution.CampusId);
            ViewBag.TypeId = new SelectList(db.InstitutionTypes, "Id", "Type", institution.TypeId);
            return View(institution);
        }

        // POST: Institution/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CampusId,Address,TypeId")] Institution institution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(institution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", institution.CampusId);
            ViewBag.TypeId = new SelectList(db.InstitutionTypes, "Id", "Type", institution.TypeId);
            return View(institution);
        }

        // GET: Institution/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Institution institution = db.Institutions.Find(id);
            if (institution == null)
            {
                return HttpNotFound();
            }
            return View(institution);
        }

        // POST: Institution/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Institution institution = db.Institutions.Find(id);
            db.Institutions.Remove(institution);
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
