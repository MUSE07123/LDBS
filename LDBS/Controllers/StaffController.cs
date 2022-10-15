using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LDBS.Models;

namespace LDBS.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeathRoster()
        {
            DeathRosterSQL deathRosterSQL = new DeathRosterSQL();
            List<DeathRosterSQL> deathRosterSQLlist = deathRosterSQL.GetDeathRosterSQL();
            ViewBag.deathRosterlist = deathRosterSQLlist;

            return View();
        }
    }
}