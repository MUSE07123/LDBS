using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LDBS.Models;

namespace LDBS.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeathRoster()
        {
            DeathRosterSQL deathRosterSQL = new DeathRosterSQL();
            List<DeathRosterSQL> deathRosterSQLlist = deathRosterSQL.GetDeathRosterSQL();
            ViewBag.deathRosterlist = deathRosterSQLlist;

            return View();
        }

        public ActionResult DeathRosterDetails(string ID)
        {
            DeathRosterSQL deathRosterSQL = new DeathRosterSQL();
            ViewBag.id = ID;
            Boolean IDisTrue = false;

            if (ID != null)
            {
                string RemarkChange = Request.Form["RemarkChange"]; //儲存view編輯按鈕回傳的備註
                if (RemarkChange != null) //備註有修改再執行
                {
                    //修改對應ID的備註
                    List<DeathRosterSQL> setRemark = deathRosterSQL.SetRemarkChange(ID, RemarkChange);
                }

                //讀取sql放在最後  備註有修改的話資料才會刷新
                List<DeathRosterSQL> deathRosterSQLlist = deathRosterSQL.GetDeathRosterSQL();

                foreach (DeathRosterSQL deathRosterSQL1 in deathRosterSQLlist)
                {
                    if (deathRosterSQL1.ID == ID)
                    {
                        IDisTrue = true;
                    }
                }

                if (IDisTrue == true)
                {
                    ViewBag.deathRosterlist = deathRosterSQLlist;
                    return View();
                }
                else
                {
                    return Content("<script >alert('查無此資料，請回死亡名單確認');window.open('" + Url.Content("/Staff/DeathRoster") + "','_self')</script >", "text/html");
                }

            }
            else
            {
                return Content("<script >alert('查無此資料，請回死亡名單確認');window.open('" + Url.Content("/Staff/DeathRoster") + "','_self')</script >", "text/html");
            }

        }

        public ActionResult PermissionList()
        {


            return View();
        }

        public ActionResult PermissionSetting()
        {


            return View();
        }
    }
}