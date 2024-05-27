using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCFinalProject.Models;

namespace MVCFinalProject.Controllers
{
    public class VisaCheckersController : Controller
    {
        private readonly ModelContext _context;

        public VisaCheckersController(ModelContext context)
        {
            _context = context;
        }

        // GET: VisaCheckers
        public async Task<IActionResult> Index()
        {
              return _context.VisaCheckers != null ? 
                          View(await _context.VisaCheckers.ToListAsync()) :
                          Problem("Entity set 'ModelContext.VisaCheckers'  is null.");
        }

        // GET: VisaCheckers/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.VisaCheckers == null)
            {
                return NotFound();
            }

            var visaChecker = await _context.VisaCheckers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visaChecker == null)
            {
                return NotFound();
            }

            return View(visaChecker);
        }

        // GET: VisaCheckers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VisaCheckers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CardHolderName,Cvc,CardNumber,Balance,Id")] VisaChecker visaChecker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visaChecker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(visaChecker);
        }

        // GET: VisaCheckers/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.VisaCheckers == null)
            {
                return NotFound();
            }

            var visaChecker = await _context.VisaCheckers.FindAsync(id);
            if (visaChecker == null)
            {
                return NotFound();
            }
            return View(visaChecker);
        }

        // POST: VisaCheckers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("CardHolderName,Cvc,CardNumber,Balance,Id")] VisaChecker visaChecker)
        {
            if (id != visaChecker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visaChecker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisaCheckerExists(visaChecker.Id))
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
            return View(visaChecker);
        }

        // GET: VisaCheckers/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.VisaCheckers == null)
            {
                return NotFound();
            }

            var visaChecker = await _context.VisaCheckers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (visaChecker == null)
            {
                return NotFound();
            }

            return View(visaChecker);
        }

        // POST: VisaCheckers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.VisaCheckers == null)
            {
                return Problem("Entity set 'ModelContext.VisaCheckers'  is null.");
            }
            var visaChecker = await _context.VisaCheckers.FindAsync(id);
            if (visaChecker != null)
            {
                _context.VisaCheckers.Remove(visaChecker);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisaCheckerExists(decimal id)
        {
          return (_context.VisaCheckers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
