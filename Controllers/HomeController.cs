using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using commonroom.Models;
using System.Linq;
using commonroom.ViewModels;
using Microsoft.AspNetCore.Identity;


using System.Data;

using System.Data.SqlClient;

namespace commonroom.Controllers
{
    public class HomeController : Controller
    {
        private DataContext _context;
        public HomeController(DataContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        //---------------------------------------------------------- C R E A T E   A N   A C C O U N T ------------------------------------------
        [HttpGet]
        [Route("createanaccount")]
        public IActionResult CreateAnAccount()
        {
            return View("New");
        }
        //------------------------------------------------------------- R E G I S T E R ----------------------
        [HttpPost]
        [Route("register")]
        public IActionResult Registration(RegistrationViewModel reg)
        {
            if(!ModelState.IsValid)
            {
                return View("Index");
            }
            User UserInDB = _context.Users.FirstOrDefault(u => u.Email == reg.Email);
            if(UserInDB != null)
            {
                ModelState.AddModelError("Email", "User already exists");
                return View("Index");
            }
            PasswordHasher<RegistrationViewModel> hasher = new PasswordHasher<RegistrationViewModel>();
            string hashedPW = hasher.HashPassword(reg, reg.Password);
            User newUser = new User
            {
                FirstName = reg.FirstName,
                LastName = reg.LastName,
                Email = reg.Email,
                Password = hashedPW
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();

            User loggedIn = _context.Users
                .FirstOrDefault (u => u.Email == reg.Email);

            HttpContext.Session.SetInt32 ("userId", loggedIn.UserID);
            // HttpContext.Session.SetString ("userName", loggedIn.FirstName);

            return RedirectToAction("Home"); 
        }
        //---------------------------------------------------------------- L O G I N -----------------------
        [HttpPost ("login")]
        public IActionResult Login(LoginViewModel LoginUser) 
        {
            if(!ModelState.IsValid)
            {
                return View("Index");
            }
            User userinDB = _context.Users.FirstOrDefault(u=>u.Email == LoginUser.LoginEmail);
            
            if(userinDB is null)
            {
                ModelState.AddModelError("Email","Invalid Email");
                return View("Index");
            }
            PasswordHasher<LoginViewModel> hasher = new PasswordHasher<LoginViewModel> ();
            var result = hasher.VerifyHashedPassword (LoginUser, userinDB.Password, LoginUser.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError ("LoginEmail", "Invalid Login");
                return View ("Index");
            }
            User loggedIn = _context.Users.FirstOrDefault (u => u.Email == LoginUser.LoginEmail);
            HttpContext.Session.SetInt32 ("ID", loggedIn.UserID);
            HttpContext.Session.SetString("UserName", loggedIn.FirstName);
            HttpContext.Session.SetString("Login", "true");
            return RedirectToAction("Home");
        }
        //-------------------------------------------------- H O M E -----------------------------------
        [HttpGet]
        [Route("home")]
        public IActionResult Home()
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if(userID == null)
            {
                return RedirectToAction("Index");
            }
            // List<User> users=_context.Users.ToList();
            // ViewBag.Users = users;



            List<Country> allCountries = _context.Countries.ToList();
            ViewBag.allCountries = allCountries;

            return View("Home");
        }
        //-------------------------------------------- A D D   C O U N T R Y [POST] --------------------------------

        [HttpPost]
        [Route("addcountry")]
        public IActionResult AddCountry(Country country)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if(userID == null)
            {
                return RedirectToAction("Index");
            }
           
            User User = _context.Users
            .FirstOrDefault (u => u.UserID == userID);

            Country newCountry = new Country
            {
                CountryName = country.CountryName,
                UserID = (int) userID,
                User = country.User
            };
            _context.Countries.Add(newCountry);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }


        //----------------------------------------------------- L O G O U T -------------------------------
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        //------------------------------------------ E D I T   A C C O U N T ------------------------------------
        // [HttpGet]
        // [Route("editaccount/{user_id}")]
        // public IActionResult EditAccount(int user_id)
        // {
        //     int? userID = HttpContext.Session.GetInt32("ID");
        //     if(userID == null)
        //     {
        //         return RedirectToAction("Index");
        //     }
        //     User retrieveduser = _context.Users.SingleOrDefault(u=>u.UserID == user_id);
        //     ViewBag.RetrievedUser = retrieveduser;

        //     List<User> users=_context.Users.ToList();
        //     ViewBag.Users = users;
        //     //session
        //     //pull db info
        //     return View("Edit");
        // }
        //------------------------------------------ U P D A T E    A C C O U N T -------------------------------------
        // [HttpPost]
        // [Route("updateaccount")]
        // public IActionResult UpdateAccount()
        // {
        //     int? userID = HttpContext.Session.GetInt32("ID");
        //     if(userID == null)
        //     {
        //         return RedirectToAction("Index");
        //     }
        //     //session (????)
        //     //edit pulled info
        //     return View("Edit");
        // }
        //-------------------------------------------------   C H O O S E   C O U T R Y   ---------------------------------------------

        [HttpGet]
        [Route("choosecountry")]
        public IActionResult ChooseCountry()
        {
            List<Country> allCountries = _context.Countries.ToList();
            ViewBag.allCountries = allCountries;
            return View("Inbetween");
        }

        //------------------------------------------------ A D D   T R I P -------------------------------------------\
        [HttpGet]
        [Route("additinerary/{country_id}")]
        public IActionResult AddItinerary(int country_id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if(userID == null)
            {
                return RedirectToAction("Index");
            }

            List<Country> allCountries = _context.Countries.ToList();
            ViewBag.allCountries = allCountries;

            //-------------------------------------------------
            var thisCountry = (int) country_id;
            ViewBag.ThisCountry = thisCountry;
            return View("AddItinerary");
        }

        //------------------------------- A D D   T R I P   P O S T -------------------------------------------


        [HttpPost]
        [Route("/{country_id}/SubmitTrip")]
        public IActionResult SubmitTrip(Trip trip, int country_id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if(userID == null)
            {
                return RedirectToAction("Index");
            }
           
            User User = _context.Users.FirstOrDefault (u => u.UserID == userID);
            ViewBag.User = User;
            
            Country Country = _context.Countries.FirstOrDefault(c=> c.CountryID == country_id);
            var thisCountry = (int) country_id;
            ViewBag.ThisCountry = thisCountry;

            Trip newTrip = new Trip
            {
                TripName = trip.TripName,
                Start = trip.Start,
                Middle = trip.Middle,
                End = trip.End,
                Length = trip.Length,
                About = trip.About,
                UserID = (int) userID,
                User = trip.User,
                CountryID = thisCountry,
                Country = trip.Country
            };
            _context.Trips.Add(newTrip);
            _context.SaveChanges();



            return RedirectToAction("MyTrips");
        }
        //--------------------------------------------   M Y   T R I P S  --------------------------------------------------
        [HttpGet]
        [Route("mytrips")]
        public IActionResult MyTrips(SavedTrip savedTrip, int trip_id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");

            if(userID == null)
            {
                return RedirectToAction("Index");
            }
            
            
            List<Trip> myTrips = _context.Trips.Where(u=>u.UserID == userID).ToList();
            ViewBag.myTrips = myTrips;
            
            List<Comment> tripComments = _context.Comments.Where(a=>a.TripID == trip_id).ToList();
            ViewBag.tripComments = tripComments;

            List<SavedTrip> savedTrips = _context.SavedTrips.ToList();
            
            
            // Trip thisTrip = _context.Trips.Include(c=>c.CountryID == country_id).ThenInclude(u=>u.UserID == user_id).ToList();
            // ViewBag
            // ViewBag.User = _context.Users.Where (u => u.UserID == user_id).FirstOrDefault ();
            
            // Trip myTrip = _context.Trips.Where(c=>c.CountryID == country_id).Include(u=>u.UserID == user_id).FirstOrDefault();

            return View("MyTrips");
        }
        //--------------------------------------------- A L L   T R I P S ------------------------------------------------------
        [HttpGet]
        [Route("alltrips")]
        public IActionResult AllTrips(int country_id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if(userID == null)
            {
                return RedirectToAction("Index");
            }
            List<Trip> allTrips = _context.Trips.ToList();
            ViewBag.allTrips = allTrips;

            Country thisCountry = _context.Countries.FirstOrDefault(c=>c.CountryID == country_id);
            ViewBag.Country = thisCountry;
            return View("AllTrips");
        }
        //------------------------------------ D E L E T E   T R I P ---------------------------
        [HttpGet]
        [Route("delete/{trip_id}")]
        public IActionResult Delete(int trip_id)

        //doesnt work yet. I thing id is not passing
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if(userID == null)
            {
                return RedirectToAction("Index");
            }

            Trip thisTrip = _context.Trips.SingleOrDefault(a=>a.TripID == trip_id);
            _context.Remove(thisTrip);
            _context.SaveChanges();
            
            return RedirectToAction("MyTrips");
        }
        //--------------------------------------   V I E W   T R I P   ---------------------------------
        [HttpGet]
        [Route("{trip_id}")]
        public IActionResult ViewTrip(int trip_id, Comment comment, int comment_id, int user_id, int savedtrip_id,SavedTrip savedtrip, int country_id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            ViewBag.userID = userID;
            if(userID == null)
            {
                return RedirectToAction("Index");
            }
            Trip thisTrip = _context.Trips.SingleOrDefault(a=>a.TripID == trip_id);
            ViewBag.thisTrip = thisTrip;

            List<Trip> myTrips = _context.Trips.Where(u=>u.UserID == userID).ToList();
            ViewBag.myTrips = myTrips;
            
            List<Comment> tripComments = _context.Comments.Where(a=>a.TripID == trip_id).ToList();
            ViewBag.tripComments = tripComments;

            Comment thisComment = _context.Comments.SingleOrDefault(c=>c.CommentID == comment_id);
            ViewBag.thisComment = thisComment;

            List<SavedTrip> savedTrips = _context.SavedTrips.Where(u=>u.UserID == user_id).ToList();
            ViewBag.savedTrips = savedTrips;

            List<SavedTrip> savedTrips2 = _context.SavedTrips.ToList();
            ViewBag.savedTrips2 = savedTrips2;
            

            
            Country Country = _context.Countries.FirstOrDefault(c=> c.CountryID == country_id);
            var thisCountry = (int) country_id;
            ViewBag.ThisCountry = thisCountry;
            //-----------------------------------

            User User = _context.Users
            .FirstOrDefault (u => u.UserID == userID);
            
            
            // Trip thisTrip = _context.Trips.SingleOrDefault(a=>a.TripID == trip_id);
            // ViewBag.thisTrip = thisTrip;

            SavedTrip thisSavedTrip = _context.SavedTrips.SingleOrDefault(s=>s.SavedTripID == savedtrip_id);
            ViewBag.thisSavedTrip = thisSavedTrip;

            SavedTrip newSavedTrip = new SavedTrip{
                SavedTripName = savedtrip.SavedTripName,
                UserID = (int) userID,
                User = savedtrip.User,
                TripID = (int) trip_id,
                Trip = thisTrip
            };
            _context.Add(newSavedTrip);
            _context.SaveChanges();

            return View("ViewTrip");
        }
        //-----------------------------------------------   P O S T   C O M M E N T   -----------------------------------
        [HttpPost]
        [Route("{trip_id}/postcomment")]
        public IActionResult PostComment(Comment comment, int trip_id, int comment_id)
        {

            int? userID = HttpContext.Session.GetInt32("ID");
            if(userID == null)
            {
                return RedirectToAction("Index");
            }
            Trip thisTrip = _context.Trips.SingleOrDefault(a=>a.TripID == trip_id);
            ViewBag.thisTrip = thisTrip;
            
            Comment thisComment = _context.Comments.SingleOrDefault(c=>c.CommentID == comment_id);
            ViewBag.thisComment = thisComment;

            Comment newComment = new Comment
            {
                CommentTheme = comment.CommentTheme,
                CommentText = comment.CommentText,
                UserID = (int) userID,
                User = comment.User,
                TripID = (int) trip_id,
                Trip = thisTrip 
            };
            _context.Comments.Add(newComment);
            _context.SaveChanges();
            return RedirectToAction("ViewTrip");
        }
        //---------------------- S A V E D   T R I P S -----------------
        [HttpPost]
        [Route("{trip_id}/savetrip")]
        public IActionResult SaveTrip(SavedTrip savedtrip, int trip_id, int savedtrip_id)
        {
            int? userID = HttpContext.Session.GetInt32("ID");
            if(userID == null)
            {
                return RedirectToAction("Index");
            }
            
            Trip thisTrip = _context.Trips.SingleOrDefault(a=>a.TripID == trip_id);
            ViewBag.thisTrip = thisTrip;

            SavedTrip thisSavedTrip = _context.SavedTrips.SingleOrDefault(s=>s.SavedTripID == savedtrip_id);
            ViewBag.thisSavedTrip = thisSavedTrip;

            SavedTrip newSavedTrip = new SavedTrip{
                SavedTripName = savedtrip.SavedTripName,
                UserID = (int) userID,
                User = savedtrip.User,
                TripID = (int) trip_id,
                Trip = thisTrip
            };
            _context.Add(newSavedTrip);
            _context.SaveChanges();
        
            return RedirectToAction("MyTrips");
        }
    }
}

