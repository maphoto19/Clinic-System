using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Clinic_System.Models;

namespace Clinic_System.Controllers
{
    public class PatientAppointmentController : Controller
    {
        private ClinicSysDbEntities db = new ClinicSysDbEntities();

        // GET: PatientAppointment
        public ActionResult Index()
        {
            var patientAppointments = db.PatientAppointments.Include(p => p.Gender).Include(p => p.HealthOption).Include(p => p.MedicalPractitioner).Include(p => p.TimePreference).Include(p => p.UserTitle);
            return View(patientAppointments.ToList());
        }

        // GET: PatientAppointment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientAppointment patientAppointment = db.PatientAppointments.Find(id);
            if (patientAppointment == null)
            {
                return HttpNotFound();
            }
            return View(patientAppointment);
        }

        // GET: PatientAppointment/Create
        public ActionResult Create()
        {
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name");
            ViewBag.OptionId = new SelectList(db.HealthOptions, "Id", "CheckUpOption");
            ViewBag.PractId = new SelectList(db.MedicalPractitioners, "Id", "Name");
            ViewBag.TimeId = new SelectList(db.TimePreferences, "Id", "TimePreference1");
            ViewBag.TitleId = new SelectList(db.UserTitles, "Id", "TitleName");
            return View();
        }

        // POST: PatientAppointment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LastName,Address,Email,TimeId,OptionId,GenderId,TitleId,Date,Symptoms,Cellphone,Cellphone2,StudentNumber,ID_Number,Comorbidity,PractId")] PatientAppointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.PatientAppointments.Add(appointment);
                db.SaveChanges();
                SendMail(appointment.Email, appointment.Date.ToString("dddd, dd MMMM yyyy"), appointment.Name, appointment.Address, "Ruth Mompati");
                return RedirectToAction("Index");
            }

            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", appointment.GenderId);
            ViewBag.OptionId = new SelectList(db.HealthOptions, "Id", "CheckUpOption", appointment.OptionId);
            ViewBag.PractId = new SelectList(db.MedicalPractitioners, "Id", "Name", appointment.PractId);
            ViewBag.TimeId = new SelectList(db.TimePreferences, "Id", "TimePreference1", appointment.TimeId);
            ViewBag.TitleId = new SelectList(db.UserTitles, "Id", "TitleName", appointment.TitleId);
            return View(appointment);
        }

        // GET: PatientAppointment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientAppointment patientAppointment = db.PatientAppointments.Find(id);
            if (patientAppointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", patientAppointment.GenderId);
            ViewBag.OptionId = new SelectList(db.HealthOptions, "Id", "CheckUpOption", patientAppointment.OptionId);
            ViewBag.PractId = new SelectList(db.MedicalPractitioners, "Id", "Name", patientAppointment.PractId);
            ViewBag.TimeId = new SelectList(db.TimePreferences, "Id", "TimePreference1", patientAppointment.TimeId);
            ViewBag.TitleId = new SelectList(db.UserTitles, "Id", "TitleName", patientAppointment.TitleId);
            return View(patientAppointment);
        }

        // POST: PatientAppointment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LastName,Address,Email,TimeId,OptionId,GenderId,TitleId,Date,Symptoms,Cellphone,Cellphone2,StudentNumber,ID_Number,Comorbidity,PractId")] PatientAppointment patientAppointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientAppointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GenderId = new SelectList(db.Genders, "Id", "Name", patientAppointment.GenderId);
            ViewBag.OptionId = new SelectList(db.HealthOptions, "Id", "CheckUpOption", patientAppointment.OptionId);
            ViewBag.PractId = new SelectList(db.MedicalPractitioners, "Id", "Name", patientAppointment.PractId);
            ViewBag.TimeId = new SelectList(db.TimePreferences, "Id", "TimePreference1", patientAppointment.TimeId);
            ViewBag.TitleId = new SelectList(db.UserTitles, "Id", "TitleName", patientAppointment.TitleId);
            return View(patientAppointment);
        }

        // GET: PatientAppointment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientAppointment patientAppointment = db.PatientAppointments.Find(id);
            if (patientAppointment == null)
            {
                return HttpNotFound();
            }
            return View(patientAppointment);
        }

        // POST: PatientAppointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientAppointment patientAppointment = db.PatientAppointments.Find(id);
            db.PatientAppointments.Remove(patientAppointment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [NonAction]

        public void SendMail(string email, string date, string name, string address, string dr)
        {
            string subject;
            string body;

            var fromEmail = new MailAddress(EmailData.FROM_EMAIL, "Clinic System");
            var toEmail = new MailAddress(email);
            var fromEmailPassword = EmailData.PASSWORD;

            subject = "Appointment Confirmation";

            body = "<b>Name<b/>: "+ name + "<br/><b>Address<b/>: " + address +"<br/><br/>We are writing to tell you we have booked your appointment with Dr: " + dr +
               " on " + date + ".<br/><br/>Please bring along your ID/Student/Staff card.<br/>If there's any changes kindly inform us in time" +
               " <br/><br/>Kindly inform us beforehand if you are unavailable. <br/> Regards<br/>Admin";

            var smtp = new SmtpClient
            {
                Host = EmailData.SMTP_HOST_GMAIL,
                Port = Convert.ToInt32(EmailData.SMTP_PORT_GMAIL),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })

                smtp.Send(message);
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
