﻿using System;
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
    public class CommentController : Controller
    {
        private ClinicSysDbEntities db = new ClinicSysDbEntities();

        // GET: Comment
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Campu).Include(c => c.CommentType).Include(c => c.CommunicationMethod).Include(c => c.MedicalPractitioner).Include(c => c.Priority);
            return View(comments.ToList());
        }

        // GET: Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name");
            ViewBag.CommentTypeId = new SelectList(db.CommentTypes, "Id", "Type");
            ViewBag.CommentMethodID = new SelectList(db.CommunicationMethods, "Id", "Method");
            ViewBag.AttendedBy = new SelectList(db.MedicalPractitioners, "Id", "Name");
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Type");
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CampusId,CommentTypeId,PriorityId,Phone,CommentMethodID,Complaint,AttendedBy,Email")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", comment.CampusId);
            ViewBag.CommentTypeId = new SelectList(db.CommentTypes, "Id", "Type", comment.CommentTypeId);
            ViewBag.CommentMethodID = new SelectList(db.CommunicationMethods, "Id", "Method", comment.CommentMethodID);
            ViewBag.AttendedBy = new SelectList(db.MedicalPractitioners, "Id", "Name", comment.AttendedBy);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Type", comment.PriorityId);
            return View(comment);
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", comment.CampusId);
            ViewBag.CommentTypeId = new SelectList(db.CommentTypes, "Id", "Type", comment.CommentTypeId);
            ViewBag.CommentMethodID = new SelectList(db.CommunicationMethods, "Id", "Method", comment.CommentMethodID);
            ViewBag.AttendedBy = new SelectList(db.MedicalPractitioners, "Id", "Name", comment.AttendedBy);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Type", comment.PriorityId);
            return View(comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CampusId,CommentTypeId,PriorityId,Phone,CommentMethodID,Complaint,AttendedBy,Email")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampusId = new SelectList(db.Campus, "Id", "Name", comment.CampusId);
            ViewBag.CommentTypeId = new SelectList(db.CommentTypes, "Id", "Type", comment.CommentTypeId);
            ViewBag.CommentMethodID = new SelectList(db.CommunicationMethods, "Id", "Method", comment.CommentMethodID);
            ViewBag.AttendedBy = new SelectList(db.MedicalPractitioners, "Id", "Name", comment.AttendedBy);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Type", comment.PriorityId);
            return View(comment);
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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
