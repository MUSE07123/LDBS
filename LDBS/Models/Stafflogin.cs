using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace LDBS.Models
{
    public class Stafflogin
    {
        private string strSQL = "";  //SQL指令
        /// <summary>
        /// 登入參數
        /// </summary>
        public class DoLoginIn
        {
            public string StaffNumber { get; set; }
            public string Password { get; set; }
        }
        /// <summary>
        /// 登入回傳
        /// </summary>
        /// 
        public class DoLoginOut
        {
            public string ErrMsg { get; set; }
            public string ResultMsg { get; set; }
        }



        /// <summary>
        /// 檢查員工帳號
        /// </summary>
        public bool StaffNumber_Check(string StaffNumber){

            bool boolretuen;

            try {
                //1.新建連接對象
                SqlConnection connection = new SqlConnection();
                //2.連接字串
                connection.ConnectionString = @"Data Source=LAPTOP-NBI92C1M\SQLEXPRESS;Initial Catalog=LDBS ;Integrated Security=SSPI;";
                connection.Open(); //開資料庫
                if (connection.State == ConnectionState.Open) {

                    // 檢查帳號是否存在
                    strSQL = "";
                    strSQL += "select StaffNumber from LDBS_StaffLogin where StaffNumber = @StaffNumber "; //SQL指令
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = strSQL;
                    cmd.Connection = connection;

                    // 使用參數化填值
                    cmd.Parameters.AddWithValue("@StaffNumber", StaffNumber);

                    // 執行資料庫查詢動作
                    SqlDataAdapter da = new SqlDataAdapter(cmd);      //查詢資料
                    DataSet Ds = new DataSet();
                    da.Fill(Ds);

                    if (Ds.Tables[0].Rows.Count == 1)
                    {
                        boolretuen = true;        //確認有此筆資料
                    }
                    else
                    {
                        boolretuen = false;       //查無此資料
                    }

                    //關閉連線
                    connection.Close();
                    connection.Dispose();

                    return boolretuen;
                }
                else
                {
                    return false;       //失敗
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
        /// <summary>
        /// 檢查員工密碼(如果正確則跳轉) //未測試
        /// </summary>
        public bool ＵserPassword_check(string StaffNumber,string Password) {
            
            bool boolretuen;
            try {
                //1.新建連接對象
                SqlConnection connection = new SqlConnection();
                //2.連接字串
                connection.ConnectionString = @"Data Source=LAPTOP-NBI92C1M\SQLEXPRESS;Initial Catalog=LDBS ;Integrated Security=SSPI;";
                connection.Open(); //開資料庫
                strSQL = "";
                strSQL += "select StaffNumber,ＵserPassword from LDBS_StaffLogin where StaffNumber = @StaffNumber and ＵserPassword  = @Password";
                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strSQL;
                cmd.Connection = connection;
                // 使用參數化填值
                cmd.Parameters.AddWithValue("@StaffNumber", StaffNumber);
                cmd.Parameters.AddWithValue("@Password", Password);
                
                // 執行資料庫查詢動作
                SqlDataAdapter da = new SqlDataAdapter(cmd);      //查詢資料
                DataSet Ds = new DataSet();
                da.Fill(Ds);

                if (Ds.Tables[0].Rows.Count == 1)
                {
                    boolretuen = true;          //確認有此筆資料
                }
                else
                {
                    boolretuen = false;        //查無此資料
                }
                return boolretuen;

            }
            catch (Exception ex) 
            {
                throw (new Exception(ex.Message));
            }

            
        }


    }
}