using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static LDBS.Models.StaffSetting;

namespace LDBS.Controllers
{
    public class StaffController : Controller
    {
        string strNum = "Aa";//預設密碼開頭

        // GET: Staff
        public ActionResult Index()
        {
            string name = (string)TempData["str"]; //GET從登入傳入的值再載入頁面
            //return Content();
            return View();
        }

        //2022/10/24
        //POST: Staff
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
                    cmd.Parameters.AddWithValue("@StaffNumber", inModel.StaffNumber);//inModel.StaffNumber
                    cmd.Parameters.AddWithValue("@UserPassword", inModel.UserPassword); //密碼預設Aa+員編(帳號)

                    // 執行資料庫更新動作
                    cmd.ExecuteNonQuery(); //異動到資料庫一定要加

                    outModel.ResultMsg = "密碼更新完成!";
                    //}
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
    }
}