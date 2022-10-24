using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LDBS.Models;
using Newtonsoft.Json;
using static LDBS.Models.StaffSetting;
using System.Web.Configuration;

namespace LDBS.Controllers
{
    public class MemberController : Controller
    {
        string strNum = "Aa";

        // GET: Member
        public ActionResult Index()
        {
            string name = (string)TempData["str"];
            //return Content(name);
            return View();
        }

        //2022/10/24
        #region 第一次登入重設密碼 
        public ActionResult DoMemberOneLogin(DoSettingIn inModel) //宣告Models：StaffSetting裡的類別DoSettingIn及DoSettingrOut
        {
            DoSettingrOut outModel = new DoSettingrOut();

            if (string.IsNullOrEmpty(inModel.UserPassword) || string.IsNullOrEmpty(inModel.UserPassword2))
            {
                outModel.ErrMsg = "請輸入資料";
            }
            else if (inModel.UserPassword != inModel.UserPassword2)
            {
                outModel.ErrMsg = "密碼不一致，請重新輸入!";
            }
            else if (inModel.UserPassword == strNum + inModel.StaffNumber)
            {
                outModel.ErrMsg = "不可輸入預設密碼!";
            }
            else
            {
                SqlConnection conn = null;
                try
                {
                    // 資料庫連線
                    string connStr = WebConfigurationManager.ConnectionStrings["LDBSDB"].ConnectionString;
                    conn = new SqlConnection();
                    conn.ConnectionString = connStr;
                    conn.Open();

                    SqlCommand cmd = new SqlCommand();
                    
                    // 異動資料至資料庫
                    string sql = @"UPDATE LDBS_StaffLogin set UserPassword = @UserPassword where StaffNumber = @StaffNumber";
                    cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;

                    // 使用參數化填值
                    cmd.Parameters.AddWithValue("@StaffNumber", inModel.StaffNumber);
                    cmd.Parameters.AddWithValue("@UserPassword", inModel.UserPassword);

                    // 執行資料庫更新動作
                    cmd.ExecuteNonQuery(); //異動到資料庫一定要加

                    outModel.ResultMsg = "密碼更新完成!";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn != null)
                    {
                        //關閉資料庫連線
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            // 輸出json
            ContentResult resultJson = new ContentResult();
            resultJson.ContentType = "application/json"; //ContentType :取得或設定內容的類型
            resultJson.Content = JsonConvert.SerializeObject(outModel); //方法Content()：回傳文字內容 
            return resultJson;
        }
        #endregion


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
                                TempData["str"] = StaffAccount;//2022/10/24
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