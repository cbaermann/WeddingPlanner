using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult CreateUser(LoginViewModel viewModel)
        {
            Console.WriteLine("############################");

            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u=>u.Email == viewModel.newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                viewModel.newUser.Password = Hasher.HashPassword(viewModel.newUser, viewModel.newUser.Password);

                dbContext.Users.Add(viewModel.newUser);
                dbContext.SaveChanges();

                HttpContext.Session.SetInt32("InSession", viewModel.newUser.UserId);

                return RedirectToAction("Dashboard");


            }
                else
                {
                    Console.WriteLine("********************");
                    return View("Index");
                }
        }

         [HttpPost("login")]
        public IActionResult LoginUser(LoginViewModel viewModel)
        {
             if(ModelState.IsValid)
            {
                var dbUser = dbContext.Users.FirstOrDefault(u=>u.Email == viewModel.newLogin.loginEmail);
                if(dbUser == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email");
                    return View("Index");
                }

                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(viewModel.newLogin, dbUser.Password, viewModel.newLogin.loginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("Password", "Password does not match email");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("InSession", dbUser.UserId);

                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("InSession")!=null)
            {
                DashboardViewModel viewModel = new DashboardViewModel();

                viewModel.currentWeddings = dbContext.Weddings
                    .Include(w=>w.UserWeddings)
                    .ThenInclude(u=>u.User)
                    .ToList();

                viewModel.thisUser = dbContext.Users
                    .FirstOrDefault(u=>u.UserId == HttpContext.Session.GetInt32("InSession"));
                
                return View("Dashboard", viewModel);
            }
            else
            {
            return RedirectToAction("Index");

            }
        }

        [HttpGet("addWedding")]
        public IActionResult addWedding()
        {
            if(HttpContext.Session.GetString("InSession")!=null)
            {
                CreateViewModel viewModel = new CreateViewModel();
                return View("Create", viewModel);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost("CreateWedding")]
        public IActionResult CreateWedding(CreateViewModel viewModel)
        {
            if(HttpContext.Session.GetString("InSession")!= null)
            {
                if(ModelState.IsValid)
                {
                    Wedding newWedding = viewModel.newWedding;
                    newWedding.Host = dbContext.Users
                        .FirstOrDefault(u=> u.UserId == HttpContext.Session.GetInt32("InSession"));
                    dbContext.Add(newWedding);
                    dbContext.SaveChanges();
                    return RedirectToAction("Dashboard");

                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("wedding/{WeddingId}")]
        public IActionResult ViewWedding(int WeddingId)
        {
            if(HttpContext.Session.GetString("InSession")!= null)
            {
                ViewWeddingViewModel viewModel = new ViewWeddingViewModel();

                Wedding thisWedding = dbContext.Weddings
                    .Include(uw=>uw.UserWeddings)
                    .ThenInclude(ug=>ug.User)
                    .FirstOrDefault(p=>p.WeddingId == WeddingId);
                return View("ViewWedding", thisWedding);
            }
            return RedirectToAction("Index");
        }

        [HttpGet("RSVP/{WeddingId}/{UserId}")]
        public IActionResult RSVP(int WeddingId, int UserId)
        {
            UserWedding userWedding = new UserWedding();
            userWedding.UserId = UserId;
            userWedding.WeddingId = WeddingId;
            
            dbContext.UserWeddings.Add(userWedding);
            dbContext.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpGet("remove/{WeddingId}/{UserId}")]
        public IActionResult RemoveRSVP(int WeddingId, int UserId)
        {
            User user = dbContext.Users
                .FirstOrDefault(u=>u.UserId==UserId);

            Wedding wedding = dbContext.Weddings
                .FirstOrDefault(w=> w.WeddingId == WeddingId);
            
            UserWedding userWedding = dbContext.UserWeddings
                .Where(uw=> uw.WeddingId == WeddingId && uw.UserId == UserId)
                .FirstOrDefault();
            dbContext.UserWeddings.Remove(userWedding);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("delete/{WeddingId}")]
        public IActionResult DeleteWedding(int WeddingId)
        {
            Wedding wedding = dbContext.Weddings
                .Include(w=> w.Host)
                .FirstOrDefault(w=>w.WeddingId == WeddingId);
            
            dbContext.Weddings.Remove(wedding);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        
    }
}
