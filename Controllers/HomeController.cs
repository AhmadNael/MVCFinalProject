using Microsoft.AspNetCore.Mvc;
using MVCFinalProject.Models;
using System.Diagnostics;

namespace MVCFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        public HomeController(ILogger<HomeController> logger , ModelContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var user = _context.Userinfos.Where(x => x.Id == id).SingleOrDefault();
            return View(user);         
        }
        public IActionResult About()
        {
            return View();
        }


        public IActionResult Service()
        {
            return View();
        }
        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Booking()
        {
            return View();

        }

        public IActionResult OurTeam()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Testimonial()
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
