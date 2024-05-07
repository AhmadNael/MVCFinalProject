using Microsoft.AspNetCore.Mvc;
using MVCFinalProject.Models;

namespace MVCFinalProject.Controllers
{
    public class AdminController : Controller
    {
        
        private readonly ModelContext _context;
        public AdminController(ModelContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == id).SingleOrDefault();
            return View(user);
        }
    }
}
