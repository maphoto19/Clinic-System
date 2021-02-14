using Clinic_System.Models;
using Clinic_System.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Clinic_System.Controllers
{
    public class AuthController : Controller
    {
        private ClinicSysDbEntities db = new ClinicSysDbEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin user, string ReturnUrl = "")
        {
            if (!string.IsNullOrEmpty(user.EmailID))
            {
                if (!string.IsNullOrEmpty(user.Password))
                {
                    if (ModelState.IsValid)
                    {
                        string message = "";

                        using (ClinicSysDbEntities dc = new ClinicSysDbEntities())
                        {
                            var userData = dc.Users.Where(a => a.Email == user.EmailID).FirstOrDefault();

                            if (userData != null)
                            {
                                if (!userData.IsEmailVerified)
                                {
                                    ViewBag.Message = "Please verify your email first";
                                    return View();
                                }

                                if (string.Compare(Encryption.PasswordHash(user.Password), userData.Password) == 0)
                                {
                                    int timeout = user.RememberMe ? 525600 : 20; 
                                    var ticket = new FormsAuthenticationTicket(user.EmailID, user.RememberMe, timeout);
                                    string encrypted = FormsAuthentication.Encrypt(ticket);
                                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                                    cookie.HttpOnly = true;
                                    Response.Cookies.Add(cookie);


                                    if (Url.IsLocalUrl(ReturnUrl))
                                    {
                                        return Redirect(ReturnUrl);
                                    }
                                    else
                                    {
                                        Session["FirstName"] = userData.Name;
                                        Session["LastName"] = userData.LastName;
                                        Session["ID"] = userData.ID_Number;
                                        Session["Address"] = userData.Address;
                                        Session["Phone"] = userData.Phone;
                                        Session["Email"] = userData.Email;



                                        //Analyics
                                        Session["AppointmentCount"] = dc.PatientAppointments.ToList().Count();
                                        Session["UserCount"] = dc.Users.ToList().Count();
                                        Session["PatientCount"] = dc.Patients.ToList().Count();
                                        Session["StockCount"] = dc.Inventories.ToList().Count();
                                        Session["CommentCount"] = dc.Comments.ToList().Count();
                                        Session["PractitionerCount"] = dc.MedicalPractitioners.ToList().Count();
                                        Session["ClinicCount"] = dc.HealthClinics.ToList().Count();

                                        return RedirectToAction("Launcher", "Home");

                                    }
                                }
                                else
                                {
                                    message = "Invalid credential provided";
                                }
                            }
                            else
                            {
                                message = "Invalid credential provided";
                            }
                        }
                        ViewBag.Message = message;
                        return View();
                    }

                }
                else
                {
                    ViewBag.Message = "Error: Password is a required field!";
                }
            }
            else
            {
                ViewBag.Message = "Error: Username/Email is a required field!";

            }

            return View();

        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Auth/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress(EmailData.FROM_EMAIL, "Clinic System");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = EmailData.PASSWORD;

            string subject = "";
            string body = "";

            if (emailFor == "VerifyAccount")
            {
                subject = "Registration Confirmation!";

                body = "<br/><br/> Account successfully created" +
                   " thanks for registering with us. Please click on the below link to verify your account" +
                   " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Need to reset your password? No problem! Just click the button</b> " +
                    "below to and you'll be good to go. If not - kindly ignore this email.<br/><br/><a href=" + link + ">Reset Password link</a>";
            }

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
        public ActionResult PasswordReset(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (ClinicSysDbEntities db = new ClinicSysDbEntities())
            {
                var user = db.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    PasswordResetViewModel model = new PasswordResetViewModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }


        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            string message = "";
            bool status = false;

            if (string.IsNullOrEmpty(email))
            {
                message = "Error: Email is a required field!";
            }
            else
            {
                using (ClinicSysDbEntities db = new ClinicSysDbEntities())
                {
                    var account = db.Users.Where(a => a.Email == email).FirstOrDefault();
                    if (account != null)
                    {
                        string resetCode = Guid.NewGuid().ToString();
                        SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                        account.ResetPasswordCode = resetCode;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "Reset password link successfully sent to your email.";
                    }
                    else
                    {
                        message = "Error: Account not found!";
                    }
                }

            }

            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordReset(PasswordResetViewModel pass)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (ClinicSysDbEntities dc = new ClinicSysDbEntities())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == pass.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Encryption.PasswordHash(pass.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Error!";
            }
            ViewBag.Message = message;
            return View(pass);
        }

        public ActionResult Registration()
        {
            ViewBag.EmployeeTypeID = new SelectList(db.EmployeeTypes, "Id", "TypeEmp");
            ViewBag.GenderID = new SelectList(db.Genders, "Id", "Name");
            ViewBag.TitleID = new SelectList(db.UserTitles, "Id", "TitleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode,ResetPasswordCode")] User user)
        {
             bool Status = false;
             string message = "";

            ViewBag.GenderID = new SelectList(db.Genders, "Id", "Name", user.GenderID);
            ViewBag.TitleID = new SelectList(db.UserTitles, "Id", "TitleName", user.TitleID);


            if (ModelState.IsValid)
            {

                #region 
                var isExist = HelperClass.IsMailValid(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View();

                }


                #endregion

                #region Generate Activation Code 
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                user.Password = Encryption.PasswordHash(user.Password);
                user.ConfirmPassword = Encryption.PasswordHash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;

                #region Save to Database
                using (ClinicSysDbEntities db = new ClinicSysDbEntities())
                {

                    try
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                        TempData["successMessage"] = $"User successfully saved";

                        SendVerificationLinkEmail(user.Email, user.ActivationCode.ToString());
                        message = " Registration successfully done. Account activation link " +
                            " has been sent to your email: " + user.Email + "\nKindly click on the link to activate your account.\n" +
                            " Clinic-System";
                        Status = true;

                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                   
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;

            return View(user);
        }
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (ClinicSysDbEntities dc = new ClinicSysDbEntities())
            {
                dc.Configuration.ValidateOnSaveEnabled = false;
                var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Request Not Valid!";
                }
            }
            ViewBag.Status = Status;
            return View();
        }
    }
}
