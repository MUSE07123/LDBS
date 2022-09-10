using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LDBS.Models;

namespace LDBS.Controllers
{
    public class Stafflogin_Controller : Controller
    {
        // GET: Stafflogin_

        #region 變數宣告
        private Stafflogin Stafflogin = new Stafflogin();
        #endregion

        public ActionResult Login()
        {

            Stafflogin.StaffNumber_Check("1110831001");
            //Stafflogin.ＵserPassword_check("1110831001", "00000001");


            return View();
        }
    }
}