using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VMS_MES_WEB.Models;

namespace VMS_MES_WEB.Controllers
{
    public class UserLoginController : Controller
    {
        public string status;

        [HttpGet]
        public ActionResult Index()
        {
            return View(new Enroll());
        }

        [HttpPost]
        public ActionResult Index(Enroll e)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["local"].ConnectionString))
            {
                string sql = "select Email,Password from Enrollment where Email=@Email and Password=@Password";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", e.Email);
                    cmd.Parameters.AddWithValue("@Password", e.Password);                   
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (sdr.Read())
                    {
                        Session["Email"] = e.Email.ToString();
                        return RedirectToAction("index","Capacity");
                    }
                    else
                    {
                        ViewData["Message"] = "User Login Details Failed!!";
                    }
                    if (e.Email.ToString() != null)
                    {
                        Session["Email"] = e.Email.ToString();
                        status = "1";
                    }
                    else
                    {
                        status = "3";
                    }

                    conn.Close();
                }
                return View();
            }
            
        }

        [HttpGet]
        public ActionResult Welcome(Enroll e)
        {
            Enroll user = new Enroll();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["local"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetEnrollmentDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 30).Value = Session["Email"].ToString();
                    //cmd.Parameters.AddWithValue("@Email", "jinhojung0905@gmail.com");
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    List<Enroll> userlist = new List<Enroll>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Enroll uobj = new Enroll();
                        uobj.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                        uobj.FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        uobj.LastName = ds.Tables[0].Rows[i]["LastName"].ToString();
                        uobj.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                        uobj.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        uobj.PhoneNumber = ds.Tables[0].Rows[i]["Phone"].ToString();
                        uobj.SecurityAnwser = ds.Tables[0].Rows[i]["SecurityAnwser"].ToString();
                        uobj.Gender = ds.Tables[0].Rows[i]["Gender"].ToString();

                        userlist.Add(uobj);

                    }
                    user.Enrollsinfo = userlist;
                }
                con.Close();

            }
            return View(user);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "UserLogin");
        }

        [HttpGet]
        public ActionResult Main()
        {
            if(Session["Email"] ==null)
                return RedirectToAction("Index", "UserLogin");
            return View();
        }
    }
}