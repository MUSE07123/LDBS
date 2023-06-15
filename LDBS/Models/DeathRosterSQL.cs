using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace LDBS.Models
{
    public class DeathRosterSQL
    {
        public long ROWID { get; set; } //序號
        public string ID { get; set; } //身分證(主鍵)
        public string Name { get; set; } //名字
        public string Gender { get; set; } //性別
        public int Age { get; set; } //年齡(享年)
        public string Death_cause { get; set; } //死亡原因
        public DateTime Birth_Date { get; set; } //出生日期 YYYY-MM-DD HH:MI:SS
        public DateTime Death_Date { get; set; } //死亡日期 YYYY-MM-DD HH:MI:SS
        public string Drath_location { get; set; } //死亡地點
        public string State { get; set; } //狀態
        public string Remark { get; set; } //備註

        public List<DeathRosterSQL> GetDeathRosterSQL()
        {
            List<DeathRosterSQL> DeathRosterlist = new List<DeathRosterSQL>();

            //1.新建連接對象
            SqlConnection connection = new SqlConnection();
            //2.連接字串
            connection.ConnectionString = @"Data Source=.\MSSQLSERVER_2019;Initial Catalog=LDBS ;Integrated Security=SSPI;";
            //查詢LDBS_DeathRoster資料表
            SqlCommand sqlCommand = new SqlCommand("select ROW_NUMBER() OVER (ORDER BY　Death_Date) as ROWID,* from LDBS_DeathRoster");
            sqlCommand.Connection = connection;
            connection.Open(); //開資料庫

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DeathRosterSQL deathRosterSQL = new DeathRosterSQL
                    {
                        ROWID = reader.GetInt64(reader.GetOrdinal("ROWID")),
                        ID = reader.GetString(reader.GetOrdinal("ID")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Gender = reader.GetString(reader.GetOrdinal("Gender")),
                        Age = reader.GetInt32(reader.GetOrdinal("Age")),
                        Death_cause = reader.GetString(reader.GetOrdinal("Death_cause")),
                        Birth_Date = reader.GetDateTime(reader.GetOrdinal("Birth_Date")),
                        Death_Date = reader.GetDateTime(reader.GetOrdinal("Death_Date")),
                        Drath_location = reader.GetString(reader.GetOrdinal("Drath_location")),
                        State = reader.GetString(reader.GetOrdinal("State")),
                        Remark = reader.GetString(reader.GetOrdinal("Remark")),
                    };
                    DeathRosterlist.Add(deathRosterSQL);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空！");
            }
            //關閉連線
            connection.Close();
            connection.Dispose();

            return DeathRosterlist;
        }

        public List<DeathRosterSQL> SetRemarkChange(string ID, string Remark)
        {
            List<DeathRosterSQL> DeathRosterlist = new List<DeathRosterSQL>();

            //1.新建連接對象
            SqlConnection connection = new SqlConnection();
            //2.連接字串
            connection.ConnectionString = @"Data Source=.\MSSQLSERVER_2019;Initial Catalog=LDBS ;Integrated Security=SSPI;";
            //查詢LDBS_DeathRoster資料表的ID
            SqlCommand sqlCommand = new SqlCommand("UPDATE LDBS_DeathRoster SET Remark = @Remark where ID = @ID");
            sqlCommand.Connection = connection;
            connection.Open(); //開資料庫

            // 使用參數化填值
            sqlCommand.Parameters.AddWithValue("@ID", ID);
            sqlCommand.Parameters.AddWithValue("@Remark", Remark);

            // 執行資料庫查詢動作
            SqlDataAdapter da = new SqlDataAdapter(sqlCommand);      //查詢資料
            DataSet Ds = new DataSet();
            da.Fill(Ds);

            //關閉連線
            connection.Close();
            connection.Dispose();

            return DeathRosterlist;
        }

    }
}