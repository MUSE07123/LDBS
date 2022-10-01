using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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