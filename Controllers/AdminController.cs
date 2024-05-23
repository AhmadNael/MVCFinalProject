using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFinalProject.Models;

namespace MVCFinalProject.Controllers
{
    public class AdminController : Controller
    {

        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHost;

        public AdminController(ModelContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        public async Task<string> UploadImg(IFormFile img)
        {
            string wwwrootPath = _webHost.WebRootPath;
            string fileName = Guid.NewGuid().ToString() + "_" + img.FileName;
            string fullPath = Path.Combine(wwwrootPath, "Images", fileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }

            return fileName;
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == id).SingleOrDefault();
            ViewBag.Users = _context.Logins.Where(x => x.RoleId == 3).Count();
            ViewBag.Chefs = _context.Logins.Where(x => x.RoleId == 2).Count();
            ViewBag.Recipes = _context.Recipes.Where(x => x.StatusId == 1).Count();
            return View(user);

        }

        public async Task<IActionResult> AllCategories ()
        {
            var id = HttpContext.Session.GetInt32("AdminId");
            var user =  _context.Userinfos.Where(x => x.Id == id).SingleOrDefault();

            var model = await _context.Categories.ToListAsync();
            return View(model);
        }

        // GET: AdminController/Add Categorie
        public async Task<ActionResult> AddCategorie()
        {
            var id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == id).SingleOrDefault();          
            return View();
        }

        // POST: AdminController/Add Categorie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategorie(Category category)
        {
            var id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == id).SingleOrDefault();

            if (category.imgFile !=null) {

                category.CategoryImg = await UploadImg(category.imgFile);
            }
            
            if (ModelState.IsValid)
            {                 
                _context.Add(category);
             await  _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllCategories));
            }

            return View(category);
        }

        public async Task<IActionResult> EditCategory(decimal? id)
        {
            var setion_id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == setion_id).SingleOrDefault();
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(decimal id, [Bind("CategoryId,CategoryName")] Category category)
        {
            var setion_id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == setion_id).SingleOrDefault();
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        private bool CategoryExists(decimal id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> DeleteCategory(decimal? id)
        {
            var setion_id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == setion_id).SingleOrDefault();
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(decimal id)
        {
            var setion_id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == setion_id).SingleOrDefault();
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ModelContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsCategory(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        
        public async Task<IActionResult> AcceptRecipe(decimal? id)
        {


            if (id == null ||  _context.Recipes == null)
            {
                return NotFound();
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                recipe.StatusId = 2;
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("AllRecipes");
        }


        public async Task<IActionResult> RejectRecipe(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                recipe.StatusId = 3;
                _context.Recipes.Update(recipe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AllRecipes");

        }

        public async Task<IActionResult> AllRecipes()
        {           
            var model = await _context.Recipes.Include(r => r.Category).Include(r => r.User).ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AllRecipes(DateTime? startDate, DateTime? endDate , string? name)
        {
           var model = await _context.Recipes.Include(r => r.Category).Include(r => r.User).Include(r => r.Status).ToListAsync();

            


            if (startDate == null && endDate == null)
            {
                if (!String.IsNullOrEmpty(name)) model =  model.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                return View(model);
            }
            else if (startDate != null && endDate == null)
            {
                model = model.Where(x => x.CreationDate >= startDate).ToList();
                if (!String.IsNullOrEmpty(name)) model = model.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                return View(model);
            }
            else if (startDate == null && endDate != null)
            {
                model = model.Where(x => x.CreationDate <= endDate).ToList();
                if (!String.IsNullOrEmpty(name)) model = model.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                return View(model);

            }
            else
            {
                model = model.Where(x => x.CreationDate >= startDate && x.CreationDate <= endDate).ToList();
                if (!String.IsNullOrEmpty(name)) model = model.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                return View(model);
            }

        }

        public async Task<IActionResult> AllUsers()
        {
            var modelContext = _context.Logins.Include(l => l.Role).Include(l => l.User);
            return View(await modelContext.ToListAsync());
        }


        // GET: Logins/Details
        public async Task<IActionResult> DetailsUsers(string id)
        {
            if (id == null || _context.Logins == null)
            {
                return NotFound();
            }

            var login = await _context.Logins
                .Include(l => l.Role)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Email == id);
            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }


        public async Task<IActionResult> AcceptTestimonial(decimal? id)
        {

            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                testimonial.StatusId = 2;
                _context.Testimonials.Update(testimonial);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("AllTestimonials");
        }


        public async Task<IActionResult> RejectTestimonial(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                testimonial.StatusId = 3;
                _context.Testimonials.Update(testimonial);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("AllTestimonials");

        }

        public async Task<IActionResult> AllTestimonials()
        {
            var model = await _context.Testimonials.Include(u => u.User).Include(u => u.Status).ToListAsync();
            return View(model);
        }

        public string? GetEmail()
        {
            var id = HttpContext.Session.GetInt32("AdminId");
            var adminEmail = _context.Logins.Include(l => l.User).Include(l => l.Role).Where(l => l.UserId == id && l.Role.RoleId == 1).FirstOrDefault();
            return (adminEmail.Email); 
        }

        public async Task<IActionResult> ProfileDetails(string? id = null)
        {
            // If id is not provided, call GetEmail to fetch the admin email
            if (id == null)
            {
                id = GetEmail();
            }
            
            var login = await _context.Logins
                .Include(l => l.Role)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Email == GetEmail());

            if (login == null)
            {
                return NotFound();
            }

            return View(login);
        }



        public async Task<IActionResult> UpdateProfile(string id)
        {

            if (id == null)
            {
                id = GetEmail();
            }

            var login = await _context.Logins.FindAsync(id);
            if (login == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", login.RoleId);
            ViewData["UserId"] = new SelectList(_context.Userinfos, "Id", "Id", login.UserId);
            return View(login);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string id, [Bind("Email,Password,UserName,UserId,RoleId")] Login login
            ,string firstName , string lastName, DateTime birthDate, string img)
        {
            var setion_id = HttpContext.Session.GetInt32("AdminId");
            if (id != login.Email)
            {
                return NotFound();
            }

           
                try
                {
                    var model = await _context.Userinfos.Where(x => x.Id == setion_id).SingleOrDefaultAsync();
                    model.FirstName = firstName;
                    model.LastName = lastName;
                    model.BirthDate = birthDate;
                    model.ImgPath = img;
                    _context.Update(model);
                    await _context.SaveChangesAsync();


                    login.UserId = setion_id;
                    login.RoleId = 1;
                    _context.Update(login);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoginExists(login.Email))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
          
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", login.RoleId);
            ViewData["UserId"] = new SelectList(_context.Userinfos, "Id", "Id", login.UserId);
            return View(login);
        }

        private bool LoginExists(string id)
        {
            return (_context.Logins?.Any(e => e.Email == id)).GetValueOrDefault();
        }

      
    }
}
