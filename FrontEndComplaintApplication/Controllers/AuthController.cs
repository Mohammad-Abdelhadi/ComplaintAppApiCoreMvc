using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
// using Azure;
using FrontEndComplaintApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FrontEndComplaintApplication.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7107/")

            };
        }


        [HttpGet]
        public async Task<IActionResult> Index(int Id)
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");
            HttpResponseMessage response = null; // Declare the variable here

            if (!string.IsNullOrEmpty(userObjectJson))
            {
                // Deserialize the JSON string to extract the user ID
                var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);

                // Access the user ID
                int userId = userObject.Id;
                string role = userObject.Role;

                if (role == "user")
                {
                    response = await _httpClient.GetAsync($"api/Complaint/GetUserComplaints/{userId}");
                }
                else
                {
                    response = await _httpClient.GetAsync($"/api/Complaint/GetComplaints/{userId}");
                }

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var complaintsData = JsonConvert.DeserializeObject<List<Complaint>>(jsonContent);

                    // Pass userObjectJson and complaintsData directly to the view
                    ViewBag.UserObjectJson = userObjectJson;
                    return View(complaintsData);
                }

                // Handle unsuccessful API response
                // Returning a simple message for illustration purposes
                return Ok("Failed to fetch user complaints.");
            }

            // Handle the case where userObjectJson is empty or null (no user data in the session)
            // Redirect to a login page or handle the error as appropriate for your application
            return RedirectToAction("Login");
        }




        [HttpGet]
        public IActionResult Register()
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");

            // If the user is already logged in, redirect them to the index page
            if (!string.IsNullOrEmpty(userObjectJson))
            {
                return RedirectToAction("Index", "auth"); // Assuming "Home" is your controller for the index page
            }

            return View();
        }
        public IActionResult Create()
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");

            if (!string.IsNullOrEmpty(userObjectJson))
            {
                var user = JsonConvert.DeserializeObject<User>(userObjectJson);
                ViewBag.UserObjectJson = user.Id; // Assuming UserId is the property you want to pass to the view
            }

            // Initialize Demands property with an empty list
            var complaint = new Complaint
            {
                Demands = new List<Demand>()
            };

            return View(complaint);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Complaint complaint)
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");
            var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);

            try
            {
                // Save image to a specific directory within the project
                string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }
                string uniqueId = Guid.NewGuid().ToString().Substring(0, 5);

                // Save the file
                string fileName = uniqueId + Path.GetExtension(complaint.File.FileName);
                string filePath = Path.Combine(uploadDirectory, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await complaint.File.CopyToAsync(fileStream);
                }

                // Set the FileName property of the model to the file name
                complaint.FileName = fileName;
                complaint.UserId = userObject.Id;

                // Serialize the complaint object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(complaint), Encoding.UTF8, "application/json");

                // Send a POST request 
                HttpResponseMessage response = await _httpClient.PostAsync("api/Complaint/sendcomplaint", jsonContent);

                // Handle the API response
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Replace "SuccessView" with your actual success view name
                }
                else
                {
                    // Handle API error and return appropriate view
                    return View("Index"); // Replace "ErrorView" with your actual error view name
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                return BadRequest($"Failed to create. Error: {ex.Message}");
            }
        }


        public IActionResult CreateArabic()
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");

            if (!string.IsNullOrEmpty(userObjectJson))
            {
                var user = JsonConvert.DeserializeObject<User>(userObjectJson);
                ViewBag.UserObjectJson = user.Id; // Assuming UserId is the property you want to pass to the view
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArabic(Complaint complaint)
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");
            var userObject = JsonConvert.DeserializeObject<User>(userObjectJson);

            if (complaint.File != null && complaint.File.Length > 0 && ModelState.IsValid)
            {
                try
                {
                    // Save image to a specific directory within the project
                    string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }
                    string uniqueId = Guid.NewGuid().ToString().Substring(0, 5);
                    // complaint -> user id , complaintid .
                    // demand -> id complaintid 

                    // Save the file
                    string fileName = uniqueId + Path.GetExtension(complaint.File.FileName);
                    string filePath = Path.Combine(uploadDirectory, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await complaint.File.CopyToAsync(fileStream);
                    }

                    // Set the FileName property of the model to the file name
                    complaint.FileName = fileName;
                    complaint.UserId = userObject.Id;

                    // Serialize the complaint object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(complaint), Encoding.UTF8, "application/json");

                    // Send a POST request 
                    HttpResponseMessage response = await _httpClient.PostAsync("api/Complaint/sendcomplaint", jsonContent);

                    // Handle the API response
                    if (response.IsSuccessStatusCode)
                    {
                        // Extract data from response if necessary
                        // var responseData = await response.Content.ReadAsStringAsync();
                        // Pass necessary data to the view
                        return RedirectToAction("Index"); // Replace "SuccessView" with your actual success view name
                    }
                    else
                    {
                        // Handle API error and return appropriate view
                        return View("ErrorView"); // Replace "ErrorView" with your actual error view name
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions here
                    return BadRequest($"Failed to create. Error: {ex.Message}");
                }
            }
            else
            {
                // Handle invalid model state or missing file
                return BadRequest("Invalid model state or file is missing.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Register(User req)
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");

            if (ModelState.IsValid)
            {
                try

                {
                    // Serialize the RegisterReq object to JSON
                    var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                    // Send a POST request 
                    HttpResponseMessage response = await _httpClient.PostAsync("api/Auth/register", jsonContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var userData = await response.Content.ReadAsStringAsync();
                        // Assuming userData is a JSON string representing user data received from the API
                        ViewBag.UserObjectJson = userObjectJson;

                        return RedirectToAction("Login");
                    }

                    else
                    {
                        ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");

                        return View();
                    }

                }

                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred during registration.");
                }
            }
            return View();
        }

        public IActionResult Login()
        {
            var userObjectJson = HttpContext.Session.GetString("UserObject");

            // If the user is already logged in, redirect them to the index page
            if (!string.IsNullOrEmpty(userObjectJson))
            {
                return RedirectToAction("Index"); // Assuming "Home" is your controller for the index page
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel req)
        {
            if (ModelState.IsValid)
            {
                // Serialize the RegisterReq object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(req), Encoding.UTF8, "application/json");
                // Send a POST request 
                HttpResponseMessage response = await _httpClient.PostAsync("api/Auth/Login", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var userData = await response.Content.ReadAsStringAsync();
                    var userObject = JsonConvert.DeserializeObject<User>(userData);
                    // Serialize the user object to JSON
                    var userJson = JsonConvert.SerializeObject(userObject);
                    // Store the JSON string in the session
                    HttpContext.Session.SetString("UserObject", userJson);
                    // Redirect to the Index action
                    return RedirectToAction("Index");
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
                    return View();
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AcceptView(int id, bool isApproved)
        {
            try
            {
                var data = new { IsApproved = isApproved };
                var json = JsonConvert.SerializeObject(data);
                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"api/Complaint/EditComplaint/{id}?isApproved={isApproved}", jsonContent);


                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest(new { success = false, error = "Failed to update complaint status." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        public async Task<IActionResult> Reject(int id, bool isApproved)
        {
            try
            {
                var data = new { IsApproved = isApproved };
                var json = JsonConvert.SerializeObject(data);
                var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync($"api/Complaint/EditComplaint/{id}?isApproved={isApproved}", jsonContent);


                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest(new { success = false, error = "Failed to update complaint status." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }


    }
}

