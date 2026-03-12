using Microsoft.AspNetCore.Mvc;
using Minutes_Of_Meeting.DbConfig;
using Minutes_Of_Meeting.Models;
using System.Data;
using System.Data.SqlClient;
using BCrypt.Net;
using Microsoft.Data.SqlClient;

namespace Minutes_Of_Meeting.Controllers
{
    public class AuthController : Controller
    {
        private readonly Db_Connection db_Connection;

        public AuthController(Db_Connection db_Connection)
        {
            this.db_Connection = db_Connection;
        }

        public IActionResult Index()
        {
            return View("First_page");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (SqlConnection con = db_Connection.CreateConnection())
            using (SqlCommand cmd = new SqlCommand("sp_LoginUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", model.Email);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        ViewBag.Error = "User not found";
                        return View(model);
                    }

                    string status = reader["Status"].ToString();
                    if (status == "NOT_FOUND")
                    {
                        ViewBag.Error = "User not found";
                        return View(model);
                    }

                    string hashedPasswordFromDb = reader["Password"].ToString();
                    // verify
                    bool isValid = BCrypt.Net.BCrypt.Verify(model.Password, hashedPasswordFromDb);
                    if (!isValid)
                    {
                        ViewBag.Error = "Invalid password";
                        return View(model);
                    }

                    // login success -> set session
                    HttpContext.Session.SetString("UserEmail", model.Email);
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 📝 REGISTER POST (SP)
        [HttpPost]
        public IActionResult Register(Users user)
        {
            // ✅ Validate model
            if (!ModelState.IsValid)
                return View(user);

            // ✅ Check password match
            if (user.Password != user.ConfirmPassword)
            {
                ViewBag.Error = "Passwords do not match";
                return View(user);
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            using (SqlConnection con = db_Connection.CreateConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_RegisterUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

                    con.Open();
                    string result = cmd.ExecuteScalar()?.ToString();

                    if (result == "EXISTS")
                    {
                        ViewBag.Error = "User already exists";
                        return View(user);
                    }
                    HttpContext.Session.SetString("UserEmail", user.Email);
                }
            }
            return RedirectToAction("Login");
        }
        // 🚪 LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}