using democode.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace democode.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User model)
        {
            if (model.Email == null || _userService.IsEmailTaken(model.Email))
            {
                ModelState.AddModelError("Email", "That email is already in use");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            UserRole role;
            if (model.Id == "a")
            {
                role = UserRole.Admin;
            }
            else if (model.Id == "t")
            {
                role = UserRole.Teacher;
            }
            else
            {
                role = UserRole.Student;
            }

            var user = _userService.Register(model.Id ?? string.Empty, model.Name ?? string.Empty, model.Email ?? string.Empty, model.Password ?? string.Empty, role);

            if (user == null)
            {
                ModelState.AddModelError("", "There was an error registering the user");
                return View(model);
            }

            return RedirectToAction("Index", "Dashboard"); // Changed from "Index" to "Index", "Dashboard"
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = _userService.GetUser(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid email or password.");
                return View("Index", model);
            }

            // Save user details in session
            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));

            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }

        // Other actions...
    }
}
