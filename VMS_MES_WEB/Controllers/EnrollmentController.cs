using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using VMS_MES_WEB.Models;

namespace VMS_MES_WEB.Controllers
{
    public class EnrollmentController : Controller
    {
        public string value = "";

        [HttpGet]
        public ActionResult Index()
        {
            return View(new Enroll());
        }

        [HttpPost]
        public ActionResult Index(Enroll e)
        {
            if (Request.HttpMethod == "POST")
            {
                Enroll er = new Enroll();
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["local"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_EnrollDetail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FirstName", e.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", e.LastName);
                        cmd.Parameters.AddWithValue("@Password", e.Password);
                        cmd.Parameters.AddWithValue("@Gender", e.Gender);
                        cmd.Parameters.AddWithValue("@Email", e.Email);
                        cmd.Parameters.AddWithValue("@Phone", e.PhoneNumber);
                        cmd.Parameters.AddWithValue("@SecurityAnwser", e.SecurityAnwser);
                        cmd.Parameters.AddWithValue("@status", "INSERT");
                        conn.Open();
                        ViewData["result"] = cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            return View(new Enroll());
        }
    }
}