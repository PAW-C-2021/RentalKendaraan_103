using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalKendaraan.Models;

namespace RentalKendaraan.Controllers
{
    public class CustomersController : Controller
    {
        private readonly RentKendaraanContext _context;

        public CustomersController(RentKendaraanContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string gndr, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var gndrList = new List<string>();

            var gndrQuery = from d in _context.Customer orderby d.NamaCustomer select d.NamaCustomer;

            gndrList.AddRange(gndrQuery.Distinct());

            ViewBag.gndr = new SelectList(gndrList);

            var menu = from m in _context.Customer.Include(k => k.IdGenderNavigation) select m;

            if (!string.IsNullOrEmpty(gndr))
            {
                menu = menu.Where(x => x.NamaCustomer == gndr);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.NoHp.Contains(searchString) || s.Nik.Contains(searchString) || s.Alamat.Contains(searchString) || s.IdGenderNavigation.NamaGender.Contains(searchString));
            }

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DataSortParm"] = sortOrder == "Date" ? "date_dessc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.NamaCustomer);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.Alamat);
                    break;
                case "data_desc":
                    menu = menu.OrderByDescending(S => S.Alamat);
                    break;
                default:
                    menu = menu.OrderBy(s => s.NamaCustomer);
                    break;
            }

            ViewData["CurrentSort"] = sortOrder;

            if(searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            int pageSize = 5;

            return View(await PaginatedList<Customer>.CreateAsync (menu.AsNoTracking(), pageNumber?? 1, pageSize));
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.IdGenderNavigation)
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["IdGender"] = new SelectList(_context.Gender, "IdGender", "IdGender");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCustomer,NamaCustomer,Nik,Alamat,NoHp,IdGender")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdGender"] = new SelectList(_context.Gender, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["IdGender"] = new SelectList(_context.Gender, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCustomer,NamaCustomer,Nik,Alamat,NoHp,IdGender")] Customer customer)
        {
            if (id != customer.IdCustomer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.IdCustomer))
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
            ViewData["IdGender"] = new SelectList(_context.Gender, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.IdGenderNavigation)
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.IdCustomer == id);
        }
    }
}
