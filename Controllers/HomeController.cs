using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFinalProject.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using MVCFinalProject.Email;
using MVCFinalProject.PDF;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MVCFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHost = webHostEnvironment;
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

        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var modelContext = _context.Testimonials.Include(t => t.Status).Include(t => t.User).
                 Where(t => t.Status.Id == 2);
            return View(await modelContext.ToListAsync());

        }

        public async Task<IActionResult> AllRecipes()
        {
            var model = await _context.Recipes.Include(r => r.Category).Include(r => r.User).ToListAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AllRecipes(string? name)
        {
            var model = _context.Recipes.Include(r => r.Category).Include(r => r.User).Include(r => r.Status).AsQueryable();


            if (!String.IsNullOrEmpty(name))
            {
                model = model.Where(x => x.Name.ToLower().Contains(name.ToLower())).AsQueryable();
                return View(model);
            }

            else
            {

                return View(model);
            }

        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> RecipeDetails(decimal? id)
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

        public async Task<IActionResult> AllCategories()
        {
            var model = await _context.Categories.ToListAsync();
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> AllChefs()
        {
            var modelContext = _context.Logins.Include(l => l.User).Include(l => l.Role).Where(l => l.Role.RoleId == 2);
            return View(await modelContext.ToListAsync());
        }




        public async Task<IActionResult> ChefRecipes(decimal? id)
        {
            var modelContext = _context.Recipes.Include(l => l.User).Where(x => x.UserId == id && x.StatusId == 2);
            return View(await modelContext.ToListAsync());
        }


        public async Task<IActionResult> CategoryRecipes(decimal? id)
        {
            var modelContext = _context.Recipes.Include(r => r.Category).Where(r => r.CategoryId == id);
            return View(await modelContext.ToListAsync());
        }

        public async Task<IActionResult> Testimonials()
        {
            var modelContext = _context.Testimonials.Include(t => t.Status).Include(t => t.User).
                Where(t => t.Status.Id == 2);
            return View(await modelContext.ToListAsync());
        }




        // GET: Testimonials/Create
        public IActionResult CreateTestimonial()
        {
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Userinfos, "Id", "Id");
            return View();
        }

        // POST: Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTestimonial([Bind("TestId,CreationDate,Content,UserId,StatusId")] Testimonial testimonial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Testimonials));
            }
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", testimonial.StatusId);
            ViewData["UserId"] = new SelectList(_context.Userinfos, "Id", "Id", testimonial.UserId);
            return View(testimonial);
        }

        private bool TestimonialExists(decimal id)
        {
            return (_context.Testimonials?.Any(e => e.TestId == id)).GetValueOrDefault();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contactus()
        {
            return View();
        }

        // POST: Contactus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contactus([Bind("Id,UserName,Email,Subject,Message")] Contactu contactu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactu);
        }



        public string? GetEmail()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var UserEmail = _context.Logins.Include(l => l.User).Include(l => l.Role).Where(l => l.UserId == id && l.Role.RoleId == 3).FirstOrDefault();
            return UserEmail?.Email;
        }

        public async Task<IActionResult> ProfileDetails(string? id = null)
        {
            // If id is not provided, call GetEmail to fetch the admin email
            if (id == null)
            {
                id = GetEmail();
                if (id == null)
                {
                    return NotFound(); // Or handle the case where email is null
                }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string id, [Bind("Email,Password,UserName,UserId,RoleId")] Login login
            , string firstName, string lastName, DateTime birthDate, string img)
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
                login.RoleId = 3;
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

        // GET: paymentform

        public async Task<IActionResult> PaymentForm(decimal? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentForm(decimal id, string nameCard, string cardNumber, DateTime expirationDate, int cvc)
        {
            var session_id = HttpContext.Session.GetInt32("UserId");
            var visa = await _context.VisaCheckers.SingleOrDefaultAsync(x => x.CardHolderName == nameCard && x.CardNumber == cardNumber);
            var recipe = await _context.Recipes.SingleOrDefaultAsync(x => x.RecipeId == id);

            if (visa == null)
            {
                ModelState.AddModelError("", "Visa details are incorrect.");
                return View();
            }

            if (visa.CardNumber != cardNumber)
            {
                ModelState.AddModelError("", "The Card number is wrong");
                return View();
            }

            if (visa.Cvc != cvc)
            {
                ModelState.AddModelError("", "The CVC is wrong");
                return View();
            }

            if (visa.CardHolderName != nameCard)
            {
                ModelState.AddModelError("", "The Card name is wrong");
                return View();
            }

            decimal totalPrice = recipe.Price + (0.10M * recipe.Price);
            if (visa.Balance < totalPrice)
            {
                ModelState.AddModelError("", "You don't have enough money");
                return View();
            }

            // Create a new request
            Request request = new Request
            {
                RequsetDate = DateTime.Now,
                UserId = session_id,
                RecipeId = id,
                RequestTax = 0.10M * recipe.Price
            };
            _context.Add(request);
            await _context.SaveChangesAsync();

            // Deduct the amount from visa balance
            visa.Balance -= totalPrice;
            await _context.SaveChangesAsync();

            return View("Success"); // Assuming you have a Success view
        }


        public IActionResult RecipeBuying(int id)
        {
            var recipe = _context.Recipes
                                 .Include(r => r.Category)
                                 .Include(r => r.User)
                                 .Include(r => r.Status)
                                 .FirstOrDefault(r => r.RecipeId == id);

            if (recipe == null)
            {
                return NotFound();
            }

            var pdfUpdater = new CreatePDF(_context, _webHost);
            string filePath = pdfUpdater.UpdateRecipePdf(recipe);

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return BadRequest("User not logged in.");
            }

            var login = _context.Logins.Include(x => x.User).SingleOrDefault(x => x.UserId == userId);

            if (login == null)
            {
                return BadRequest("User not found.");
            }

            string customerEmail = login.Email;
            string subject = "Your Recipe Purchase";
            string body = "Thank you for purchasing the recipe. Please find the recipe attached as a PDF.";

            CreateEmail emailGenerator = new CreateEmail();
            bool emailSent = emailGenerator.SendEmailWithPDF(customerEmail, subject, body, filePath);

            if (!emailSent)
            {
                // Log the error or show a message to the user
                return StatusCode(500, "Error sending email.");
            }

            return RedirectToAction(nameof(Index));
        }


    }
}

