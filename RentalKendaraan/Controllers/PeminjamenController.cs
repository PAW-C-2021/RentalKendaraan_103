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
    public class PeminjamenController : Controller
    {
        private readonly RentKendaraanContext _context;

        public PeminjamenController(RentKendaraanContext context)
        {
            _context = context;
        }

        // GET: Peminjamen
        public async Task<IActionResult> Index(string idcs, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var idcsList = new List<string>();

            var idcsQuery = from d in _context.Peminjaman orderby d.IdCustomerNavigation.NamaCustomer.ToString() select d.IdCustomerNavigation.NamaCustomer.ToString();

            idcsList.AddRange(idcsQuery.Distinct());

            ViewBag.idcs = new SelectList(idcsList);

            var menu = from m in _context.Peminjaman.Include(k => k.IdCustomerNavigation).Include(k => k.IdJaminanNavigation).Include(k => k.IdKendaraanNavigation) select m;

            if (!string.IsNullOrEmpty(idcs))
            {
                menu = menu.Where(x => x.IdCustomerNavigation.NamaCustomer.ToString() == idcs);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.IdKendaraanNavigation.NamaKendaraan.Contains(searchString) || s.IdJaminanNavigation.NamaJaminan.Contains(searchString) || s.Biaya.ToString().Contains(searchString));
            }

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DataSortParm"] = sortOrder == "Date" ? "date_dessc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.IdCustomerNavigation.NamaCustomer);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.TglPeminjaman);
                    break;
                case "data_desc":
                    menu = menu.OrderByDescending(S => S.TglPeminjaman);
                    break;
                default:
                    menu = menu.OrderBy(s => s.IdCustomerNavigation.NamaCustomer);
                    break;
            }

            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            int pageSize = 5;

            return View(await PaginatedList<Peminjaman>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Peminjamen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjaman
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdJaminanNavigation)
                .Include(p => p.IdKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdPinjaman == id);
            if (peminjaman == null)
            {
                return NotFound();
            }

            return View(peminjaman);
        }

        // GET: Peminjamen/Create
        public IActionResult Create()
        {
            ViewData["IdCustomer"] = new SelectList(_context.Customer, "IdCustomer", "IdCustomer");
            ViewData["IdJaminan"] = new SelectList(_context.Jaminan, "IdJaminan", "IdJaminan");
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraan, "IdKendaraan", "IdKendaraan");
            return View();
        }

        // POST: Peminjamen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPinjaman,TglPeminjaman,IdKendaraan,IdCustomer,IdJaminan,Biaya")] Peminjaman peminjaman)
        {
            if (ModelState.IsValid)
            {
                _context.Add(peminjaman);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customer, "IdCustomer", "IdCustomer", peminjaman.IdCustomer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminan, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraan, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        // GET: Peminjamen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjaman.FindAsync(id);
            if (peminjaman == null)
            {
                return NotFound();
            }
            ViewData["IdCustomer"] = new SelectList(_context.Customer, "IdCustomer", "IdCustomer", peminjaman.IdCustomer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminan, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraan, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        // POST: Peminjamen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPinjaman,TglPeminjaman,IdKendaraan,IdCustomer,IdJaminan,Biaya")] Peminjaman peminjaman)
        {
            if (id != peminjaman.IdPinjaman)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peminjaman);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeminjamanExists(peminjaman.IdPinjaman))
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
            ViewData["IdCustomer"] = new SelectList(_context.Customer, "IdCustomer", "IdCustomer", peminjaman.IdCustomer);
            ViewData["IdJaminan"] = new SelectList(_context.Jaminan, "IdJaminan", "IdJaminan", peminjaman.IdJaminan);
            ViewData["IdKendaraan"] = new SelectList(_context.Kendaraan, "IdKendaraan", "IdKendaraan", peminjaman.IdKendaraan);
            return View(peminjaman);
        }

        // GET: Peminjamen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var peminjaman = await _context.Peminjaman
                .Include(p => p.IdCustomerNavigation)
                .Include(p => p.IdJaminanNavigation)
                .Include(p => p.IdKendaraanNavigation)
                .FirstOrDefaultAsync(m => m.IdPinjaman == id);
            if (peminjaman == null)
            {
                return NotFound();
            }

            return View(peminjaman);
        }

        // POST: Peminjamen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var peminjaman = await _context.Peminjaman.FindAsync(id);
            _context.Peminjaman.Remove(peminjaman);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeminjamanExists(int id)
        {
            return _context.Peminjaman.Any(e => e.IdPinjaman == id);
        }
    }
}
