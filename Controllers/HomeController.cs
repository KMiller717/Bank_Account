using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bank_Account.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Bank_Account.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context {get; set;}
        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User registered_user)
        {  
            
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u=> u.Email == registered_user.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View("Index");
                }
                else{
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    registered_user.Password = Hasher.HashPassword(registered_user, registered_user.Password);
                    _context.Users.Add(registered_user);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("UserId", registered_user.UserId);
                    return Redirect($"Account/{registered_user.UserId}");
                }

            }  
            return View("Index");

        }
        [HttpGet("Account/{UserId}")]
        public IActionResult Account()
        {
            int? userid = HttpContext.Session.GetInt32("UserId");
            if (userid == null){
                return View("Index");
            }
            else 
            {
                ViewBag.User = _context.Users.Include(a=>a.Accounts).FirstOrDefault(a => a.UserId == userid);
                return View();
            }
            
        }

        [HttpPost("Add_Value")]
        public IActionResult Add_Value(Account new_trans)
        {
            
            User loggedIn = _context.Users.Include(a=>a.Accounts).FirstOrDefault(a => a.UserId == new_trans.UserId);
            if ((loggedIn.Sum() + new_trans.Amount) < 0){
                ViewBag.User = loggedIn;
                ModelState.AddModelError("Amount", "Amount must be greater than 0");
                return View("Account", new_trans);
            }
            _context.Accounts.Add(new_trans);
            _context.SaveChanges();


            return Redirect($"Account/{new_trans.UserId}");
        }


        [HttpPost("Login")]
        public IActionResult Login(LoginUser log_user)
        {
            if (ModelState.IsValid){
                User userInDb = _context.Users.FirstOrDefault(user => user.Email ==log_user.LoginEmail);
                
                if (userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Email does not exist.");
                    return View("Index");
                }
                else 
                {
                    var hash = new PasswordHasher<LoginUser>();
                    var result = hash.VerifyHashedPassword(log_user, userInDb.Password, log_user.LoginPassword);
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    if (result == 0)
                    {
                        ModelState.AddModelError("LoginPassword","Password does not match"); 
                        return View("Index");
                    }
                return Redirect($"Account/{userInDb.UserId}");
                }  
            }
            else{
                return View("Index");
            }   
        }

    


        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
