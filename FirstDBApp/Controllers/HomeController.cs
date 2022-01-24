using FirstDBApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;

namespace FirstDBApp.Controllers
{
    public class HomeController : Controller
    {

        SqlConnection connection = new SqlConnection("Server = DESKTOP-67ROKOP; Database=AnyDb;User Id = tanvir; Password=tanvir9909;");
        SqlCommand command = new SqlCommand();
        SqlDataReader dr;
        public IActionResult Index()
        {
            List<EmployeeModel> employees = new List<EmployeeModel>();

            connection.Open();
            command.Connection = connection;
            command.CommandText = "Select * from EmployeesTB";
            dr = command.ExecuteReader();

            while (dr.Read())
            {
                var emp = new EmployeeModel
                {
                    id = dr.GetInt32(0),
                    name = dr.GetString(1),
                    email = dr.GetString(2),
                    phone = dr.GetString(3),

                };
                employees.Add(emp);
            }

            ViewBag.emplist = employees;

            return View();
        }
        [HttpPost]
        public IActionResult AddNewRecord(int id, string name, string email, string phone)
        {
            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Insert into EmployeesTB values(" + id + ",'" + name + "','" + email + "','" + phone + "')";
                

                command.ExecuteNonQuery();

                connection.Close();

                TempData["message"] = "Data Saved Successfully.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {
                if(connection.State==System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                TempData["message"] = "Data Save Failed";
                return RedirectToAction("Index", "Home");
            }
        }

        
    }
}