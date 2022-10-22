using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;  //驗證登入狀態需引入

namespace LDBS.Controllers
{
    
    public class StaffController : Controller
    {
        // GET: Staff
        [Authorize]  //驗證測試     
        public ActionResult Index()
        {
            try {
                ////登出表單驗證票卷
                FormsAuthentication.SignOut();

                ////跳轉回使用者登入頁面
                //Response.Redirect("~/Home/Index");
            }
            catch { }

            return View();
        }
    }
}