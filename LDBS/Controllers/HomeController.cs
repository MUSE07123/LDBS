using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LDBS.Models;

namespace LDBS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        //test1
        #region 變數宣告
        private Stafflogin Stafflogin = new Stafflogin();
        #endregion

        public ActionResult Index2()
        {
            Stafflogin.StaffNumber_Check("1110831001");
            //Stafflogin.ＵserPassword_check("1110831001", "00000001");
            return Content("<html><body><h1> 某段訊息<h1><body><html>");
        }
    }
}