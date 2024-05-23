using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCFinalProject.Models;

namespace MVCFinalProject.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        private readonly ModelContext _context;
        public LoginAndRegisterController(ModelContext context)
        {
            _context = context;
        }

      

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Userinfo userinfo, string Email, string Username, string Password , decimal? RoleId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userinfo);
                _context.SaveChanges();

                Login login = new Login(); 
                login.Email = Email;
                login.UserName = Username;
                login.Password = Password;
                login.UserId = userinfo.Id;
                login.RoleId = RoleId;
                _context.Add(login);
                _context.SaveChanges();

                return RedirectToAction(nameof(Login));
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            Login userLogin = await _context.Logins.
                Where(x => x.UserName == login.UserName && x.Password == login.Password).SingleOrDefaultAsync();

            if (userLogin != null)
            {
                switch(userLogin.RoleId)
                {
                    case 1:
                        HttpContext.Session.SetInt32("AdminId", (int)userLogin.UserId);
                     return RedirectToAction("Index", "Admin");
                        
                    case 2:
                        HttpContext.Session.SetInt32("ChefId", (int)userLogin.UserId);
                        
                        return RedirectToAction("Index", "Chef");                       
                    case 3:
                        HttpContext.Session.SetInt32("UserId", (int)userLogin.UserId);
                        return RedirectToAction("Index", "Home");                        
                }
               
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View(nameof(Login));
        }
    }

}
