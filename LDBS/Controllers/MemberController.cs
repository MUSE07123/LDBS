using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Security;  //驗證登入狀態需引入
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LDBS.Models;

namespace LDBS.Controllers
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        #region 變數宣告
        private StaffloginCheck StaffloginCheck = new StaffloginCheck();
        #endregion
        
        public ActionResult Stafflogin()
        {
            //輸入的帳號密碼
            string StaffAccount = Request.Form["StaffAccount"];
            string StaffPassword = Request.Form["StaffPassword"];
            //1110831001  00000001
            try
            {  //檢查輸入欄位是否有空值
                if (StaffAccount == null || StaffPassword == null) { }
                else
                {
                    if (StaffAccount == "" || StaffPassword == "")
                    {
                        ViewBag.msg = "請輸入帳號密碼！";
                    }
                    else
                    {
                        //檢查帳號是否正確
                        if (StaffloginCheck.StaffNumber_Check(StaffAccount) == true)
                        {
                            ViewBag.Msg = "";
                            //檢查密碼是否正確
                            if (StaffloginCheck.UserPassword_check(StaffAccount, StaffPassword) == true)
                            {
                                ViewBag.Msg = "";
                                ViewBag.Account = StaffAccount;
                                TempData["str"] = StaffAccount;//傳值到登入後頁面

                                //驗證通過，建立一張 ticket
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                    1,                                  //票證的版本號碼  
                                    StaffAccount,                       //使用者名稱
                                    DateTime.Now,                       //票證發行時間
                                    DateTime.Now.AddMinutes(3),         //票證有效時間(登入內時間)
                                    false,                              //如果票證將存放於持續性Cookie中，則為true
                                    "",                                 //使用者資訊
                                    FormsAuthentication.FormsCookiePath //票證存放於Cookie中時的路徑
                                    );

                                //cookie加密驗證票字串
                                string encTicket = FormsAuthentication.Encrypt(ticket);
                                //建立Cookie
                                HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                                httpCookie.HttpOnly = true;

                                //cookie 寫入 response
                                Response.Cookies.Add(httpCookie);

                                //FormsAuthentication.SetAuthCookie(StaffAccount, false);

                                //登入成功
                                return RedirectToAction("Index", "Staff");
                            }
                            else
                            {
                                ViewBag.Msg = "密碼錯誤！";
                            }
                        }
                        else
                        {
                            //回傳帳號錯誤
                            ViewBag.Msg = "此帳號不存在！";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            };

            return View();
        }
    }
}