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
            var user = _context.Userinfos.SingleOrDefault(x => x.Id == id);
            ViewBag.Users = _context.Logins.Count(x => x.RoleId == 3);
            ViewBag.Chefs = _context.Logins.Count(x => x.RoleId == 2);
            ViewBag.Recipes = _context.Recipes.Count(x => x.StatusId == 2);
            var requests = _context.Requests.ToList();
            ViewBag.SumOfTaxes = requests.Sum(r => r.RequestTax ?? 0);

            ViewBag.SummOfPending = _context.Recipes.Where(x => x.StatusId == 1).Count();
            ViewBag.SummOfAccepting = _context.Recipes.Where(x => x.StatusId == 2).Count();
            ViewBag.SummOfRejecting = _context.Recipes.Where(x => x.StatusId == 3).Count();

            ViewBag.SummOfTestPending = _context.Testimonials.Where(x => x.StatusId == 1).Count();
            ViewBag.SummOfTestAccepting = _context.Testimonials.Where(x => x.StatusId == 2).Count();
            ViewBag.SummOfTestRejecting = _context.Testimonials.Where(x => x.StatusId == 3).Count();

            ViewBag.NumberOfOrders = _context.Requests.Count(x => x.RequestTax >= 0);
            ViewBag.NumberOfCategories = _context.Categories.Count( x => x.CategoryId >= 1 );
            ViewBag.NumberOfTestmonials = _context.Testimonials.Count(x => x.StatusId == 1 || x.StatusId == 2);

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

        [HttpGet]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(decimal id, [Bind("CategoryId,CategoryName")] Category category, IFormFile imgFile)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            var setion_id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == setion_id).SingleOrDefault();

            if (ModelState.IsValid)
            {
                if (imgFile != null)
                {
                    // Upload the new image and update the category image path
                    string uploadedImagePath = await UploadImg(imgFile);
                    category.CategoryImg = uploadedImagePath;
                }

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
        [HttpGet]
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
        [HttpPost, ActionName("DeleteCategoryConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(decimal id)
        {
            var setion_id = HttpContext.Session.GetInt32("AdminId");
            var user = _context.Userinfos.Where(x => x.Id == setion_id).SingleOrDefault();
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ModelContext.Categories' is null.");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

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

        // GET: Logins/Delete/5
        public async Task<IActionResult> DeleteUser(string id)
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

        // POST: Logins/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Logins == null)
            {
                return Problem("Entity set 'ModelContext.Logins'  is null.");
            }
            var login = await _context.Logins.FindAsync(id);
            if (login != null)
            {
                _context.Logins.Remove(login);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        // GET: Testimonials/Delete/5
        public async Task<IActionResult> DeleteTestimonial(decimal? id)
        {
            if (id == null || _context.Testimonials == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .Include(t => t.Status)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: Testimonials/Delete/5
        [HttpPost, ActionName("DeleteTestimonial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Testimonials == null)
            {
                return Problem("Entity set 'ModelContext.Testimonials'  is null.");
            }
            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllTestimonials));
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


        // GET: Aboutus/Edit/5
        public async Task<IActionResult> EditAboutUs(decimal? id)
        {
            if (id == null || _context.Aboutus == null)
            {
                return NotFound();
            }

            var aboutu = await _context.Aboutus.FindAsync(id);
            if (aboutu == null)
            {
                return NotFound();
            }
            return View(aboutu);
        }

        // POST: Aboutus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAboutUs(decimal id, [Bind("Id,ImgPath,Content")] Aboutu aboutu)
        {
            if (id != aboutu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aboutu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutuExists(aboutu.Id))
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
            return View(aboutu);
        }

        private bool AboutuExists(decimal id)
        {
            return (_context.Aboutus?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var modelContext = _context.Requests.Include(r => r.Recipe).Include(r => r.User);
            return View(await modelContext.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Orders(DateTime? startDate, DateTime? endDate, string name)
        {
            var modelContext = _context.Requests.Include(r => r.Recipe).Include(r => r.User).AsQueryable();

            if (startDate.HasValue)
            {
                modelContext = modelContext.Where(x => x.RequsetDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                modelContext = modelContext.Where(x => x.RequsetDate <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(name))
            {
                modelContext = modelContext.Where(x => x.User!.FirstName!.Contains(name) || x.User!.LastName!.Contains(name));
            }

            var filteredModel = await modelContext.ToListAsync();
            return View(filteredModel);
        }


    }
}
