using DoctorAp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DoctorAp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _connectionString = "Server=tcp:group31.database.windows.net,1433;Initial Catalog=37;Persist Security Info=False;User ID=sqladmin;Password=Blackbird127;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Stocks()
        {
            List<Stocks> stocks = FetchStocks();
            return View(stocks);
        }

        public IActionResult Links()
        {
            List<Contacts> contacts = FetchContacts();
            return View(contacts);
        }

        private List<Contacts> FetchContacts()
        {
            List<Contacts> contacts = new List<Contacts>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand("SELECT TOP (1000) * FROM [dbo].[AspNetUsers]", con))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contacts.Add(new Contacts()
                            {
                                Id = reader["Id"].ToString(),
                                Firstname = reader["FirstName"].ToString(),
                                Lastname = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch contacts");
                // Handle or log the exception as per your requirement
            }

            return contacts;
        }

        private List<Stocks> FetchStocks()
        {
            List<Stocks> stocks = new List<Stocks>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand command = new SqlCommand("SELECT TOP (1000) * FROM [dbo].[ItemsLead]", con))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stocks.Add(new Stocks()
                            {
                                Item = reader["Item"].ToString(),
                                Quantity = reader["Quantity"].ToString()
                                // Add more properties as needed
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch stocks");
                // Handle or log the exception as per your requirement
            }

            return stocks;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
