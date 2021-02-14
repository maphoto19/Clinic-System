using System;
using Clinic_System.Models.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Clinic_System.Models
{
    public static class HelperClass
    {
        public static bool IsMailValid(string email)
        {
            using (ClinicSysDbEntities db = new ClinicSysDbEntities())
            {
                var getMail = db.Users.Where(a => a.Email == email).FirstOrDefault();
                return getMail != null;
            }
        }

    }
}