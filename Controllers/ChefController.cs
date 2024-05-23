using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFinalProject.Models;

namespace MVCFinalProject.Controllers
{
    public class ChefController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHost;

        public ChefController(ModelContext context, IWebHostEnvironment webHost)
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

        // GET: ChefController
        public async Task<ActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("ChefId");
            var chef=await _context.Userinfos.SingleOrDefaultAsync(u=>u.Id==id);
            ViewBag.chef = chef;
            return View(chef);
        }

        // GET: AllRecipes
        public async Task<IActionResult> AllRecipes()
        {
            var id = HttpContext.Session.GetInt32("ChefId");
            var chef = await _context.Userinfos.SingleOrDefaultAsync(u => u.Id == id);
            ViewBag.chef = chef;

            var model = await _context.Recipes.Include(r => r.Category).Include(r => r.User).Include(r => r.Status).
                Where(r => r.StatusId == 2).ToListAsync();
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> AllRecipes( string? name)
        {
            var model = _context.Recipes.Include(r => r.Category).Include(r => r.User).Include(r => r.Status).
                Where(r => r.StatusId == 2).AsQueryable();


            if (!String.IsNullOrEmpty(name))
            {
              model = model.Where(x => x.Name.ToLower().Contains(name.ToLower())).AsQueryable();
                return View(model);
            }

            else
                return View(model);

        }


        // GET: MyRecipes
        public async Task<IActionResult> MyRecipes()
        {
            var id = HttpContext.Session.GetInt32("ChefId");
            var chef = await _context.Userinfos.SingleOrDefaultAsync(u => u.Id == id);
            ViewBag.chef = chef;

            var modelContext = await _context.Recipes.Include(r => r.Category).Include(r => r.User).Where(r=>r.UserId==id).ToListAsync();
            return View( modelContext);
        }

        //// GET: ChefController/CreateRecipe
        //public async Task<ActionResult> CreateRecipe()
        //{
        //    //var id = HttpContext.Session.GetInt32("ChefId");
        //    //var chef = _context.Userinfos.SingleOrDefaultAsync(u => u.Id == id);
        //    //ViewBag.chef = chef;
        //    return View();
        //}

        //// POST: ChefController/CreateRecipe
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public  IActionResult CreateRecipe([Bind("RecipeId,Price,Description,CreationDate,RecipeImg,Name,CategoryId,UserId,UserId")] Recipe recipe)
        //{
        //    var id = HttpContext.Session.GetInt32("ChefId");
        //    var chef = _context.Userinfos.SingleOrDefaultAsync(u => u.Id == id);
        //    ViewBag.chef = chef;
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
        //    if (ModelState.IsValid)
        //    {
        //        var _id = HttpContext.Session.GetInt32("ChefId");
        //        recipe.UserId = _id;
        //        recipe.StatusId = 1;
        //        _context.Add(recipe);
        //         _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(recipe);
        //}

        public IActionResult CreateRecipe()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["UserId"] = new SelectList(_context.Userinfos, "Id", "Id");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecipe([Bind("RecipeId,Price,Description,CreationDate,RecipeImg,Name,CategoryId,UserId,imgFile")] Recipe recipe)
        {
            if (recipe.imgFile != null)
            {
                recipe.RecipeImg = await UploadImg(recipe.imgFile);
            }

            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Userinfos, "Id", "Id", recipe.UserId);
            return View(recipe);
        }


        // GET: Recipes/Edit/5
        public async Task<IActionResult> EditRecipe(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Userinfos, "Id", "Id", recipe.UserId);
            return View(recipe);
        }

        // POST: Recipes/EditRecipe/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecipe(decimal id,  Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.RecipeId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", recipe.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Userinfos, "Id", "Id", recipe.UserId);
            return View(recipe);
        }

        public async Task<IActionResult> AllChefs()
        {
            var modelContext = _context.Logins.Include(l => l.User).Include(l => l.Role).Where(l => l.Role.RoleId == 2) ;
            return View(await modelContext.ToListAsync());
        }

        public async Task<IActionResult> ChefRecipes(decimal? id)
        {
            var modelContext = _context.Recipes.Include(l => l.User).Where(x => x.UserId == id && x.StatusId == 2);
            return View(await modelContext.ToListAsync());
        }


        //GET: Recipes/Delete/5
        public async Task<IActionResult> DeleteRecipe(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("DeleteRecipe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipe(decimal id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'ModelContext.Recipes'  is null.");
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyRecipes));
        }

        private bool RecipeExists(decimal id)
        {
            return (_context.Recipes?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        }

        // GET: ChefController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        //// GET: ChefController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: ChefController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: ChefController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: ChefController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //private bool RecipeExists(decimal id)
        //{
        //    return (_context.Recipes?.Any(e => e.RecipeId == id)).GetValueOrDefault();
        //}
    }
}
