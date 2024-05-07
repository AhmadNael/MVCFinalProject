using Microsoft.AspNetCore.Mvc;
using MVCFinalProject.Models;

namespace MVCFinalProject.Controllers
{
    public class ChefController : Controller
    {
        private readonly ModelContext _context;
       public ChefController(ModelContext context) { 
        
        _context = context;
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("ChefId");
            var user = _context.Userinfos.Where(x => x.Id == id).SingleOrDefault();
            return View(user);
            
        }
    }
}
