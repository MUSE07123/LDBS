using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static LDBS.Models.StaffSetting;
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

        public ActionResult DeathRoster()
        {
            DeathRosterSQL deathRosterSQL = new DeathRosterSQL();
            List<DeathRosterSQL> deathRosterSQLlist = deathRosterSQL.GetDeathRosterSQL();
            ViewBag.deathRosterlist = deathRosterSQLlist;

            return View();
        }

        public ActionResult PermissionList()
        {


            return View();
        }

        string strNum = "Aa";//預設密碼開頭

        // GET: PermissionSetting
        #region 權限設定頁面載入動作
        public ActionResult PermissionSetting()
        {
            return View();
        }
        #endregion

        // POST: PermissionSetting
        #region 權限設定
        public ActionResult DoPermissionSetting(DoSettingIn inModel) //宣告Models：StaffSetting裡的類別DoSettingIn及DoSettingrOut
        {
            DoSettingrOut outModel = new DoSettingrOut();

            if (string.IsNullOrEmpty(inModel.UserName) || string.IsNullOrEmpty(inModel.StaffNumber) || string.IsNullOrEmpty(inModel.Email))
            {
                outModel.ErrMsg = "請輸入資料";
            }
            else if (!Regex.IsMatch(inModel.StaffNumber, @"^[0-9]{10}$"))
            {
                outModel.ErrMsg = "帳號請輸入數字10碼";
            }
            else if (!Regex.IsMatch(inModel.Email, @"^[a-zA-Z0-9]{3,20}@[a-zA-Z0-9]{2,10}.[a-zA-Z0-9]{2,10}.?[a-zA-Z0-9]{1,5}$"))
            {
                if (!Regex.IsMatch(inModel.Email, @"^[a-zA-Z0-9]{3,20}"))
                {
                    outModel.ErrMsg = "請輸入英數字至少3碼最多20碼!";
                } 
                else if (!Regex.IsMatch(inModel.Email, @"@[a-zA-Z0-9]{2,10}.[a-zA-Z0-9]{2,10}.?[a-zA-Z0-9]{1,5}$"))
                {
                    outModel.ErrMsg = "請輸入正確Email格式!";
                }
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

                    // 檢查帳號是否存在
                    string sql = "select StaffNumber from LDBS_StaffLogin where StaffNumber = @StaffNumber";
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.Connection = conn;

                    // 使用參數化填值
                    cmd.Parameters.AddWithValue("@StaffNumber", inModel.StaffNumber);

                    // 執行資料庫查詢動作
                    SqlDataAdapter adpt = new SqlDataAdapter();
                    adpt.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    adpt.Fill(ds);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        outModel.ErrMsg = "此登入帳號已存在";
                    }
                    else
                    {
                        // 異動資料至資料庫
                        sql = @"INSERT INTO LDBS_StaffLogin (StaffNumber, UserName,Titile,UserPassword,Email,Permission)
                                VALUES (@StaffNumber,@UserName,@Titile,@UserPassword,@Email,@Permission)";
                        cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = sql;

                        // 使用參數化填值
                        cmd.Parameters.AddWithValue("@StaffNumber", inModel.StaffNumber);
                        cmd.Parameters.AddWithValue("@UserName", inModel.UserName);
                        cmd.Parameters.AddWithValue("@UserPassword", strNum + inModel.StaffNumber); //密碼預設Aa+員編(帳號)
                        cmd.Parameters.AddWithValue("@Email", inModel.Email);
                        cmd.Parameters.AddWithValue("@Titile", inModel.Titile);
                        cmd.Parameters.AddWithValue("@Permission", inModel.Permission);

                        // 執行資料庫更新動作
                        cmd.ExecuteNonQuery(); //異動到資料庫一定要加

                        outModel.ResultMsg = "註冊完成";
                    }
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