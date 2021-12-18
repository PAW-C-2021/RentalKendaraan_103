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
    public class PengembaliansController : Controller
    {
        private readonly RentKendaraanContext _context;

        public PengembaliansController(RentKendaraanContext context)
        {
            _context = context;
        }

        // GET: Pengembalians
        public async Task<IActionResult> Index(string idpjm, string searchString, string sortOrder, string currentFilter, int? pageNumber)
        {
            var idpjmList = new List<string>();

            var idpjmQuery = from d in _context.Pengembalian orderby d.IdPeminjaman.ToString() select d.IdPeminjaman.ToString();

            idpjmList.AddRange(idpjmQuery.Distinct());

            ViewBag.idpjm = new SelectList(idpjmList);

            var menu = from m in _context.Pengembalian.Include(k => k.IdKondisiNavigation).Include(k => k.IdPeminjamanNavigation) select m;

            if (!string.IsNullOrEmpty(idpjm))
            {
                menu = menu.Where(x => x.IdPeminjaman.ToString() == idpjm);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.IdKondisiNavigation.NamaKondisi.Contains(searchString) || s.Denda.ToString().Contains(searchString) || s.TglPengembalian.ToString().Contains(searchString));
            }

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DataSortParm"] = sortOrder == "Date" ? "date_dessc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.Denda);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.TglPengembalian);
                    break;
                case "data_desc":
                    menu = menu.OrderByDescending(S => S.TglPengembalian);
                    break;
                default:
                    menu = menu.OrderBy(s => s.Denda);
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

            return View(await PaginatedList<Pengembalian>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Pengembalians/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalian
                .Include(p => p.IdKondisiNavigation)
                .Include(p => p.IdPeminjamanNavigation)
                .FirstOrDefaultAsync(m => m.IdPengembalian == id);
            if (pengembalian == null)
            {
                return NotFound();
            }

            return View(pengembalian);
        }

        // GET: Pengembalians/Create
        public IActionResult Create()
        {
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraan, "IdKondisi", "IdKondisi");
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjaman, "IdPinjaman", "IdPinjaman");
            return View();
        }

        // POST: Pengembalians/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPengembalian,TglPengembalian,IdPeminjaman,IdKondisi,Denda")] Pengembalian pengembalian)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pengembalian);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraan, "IdKondisi", "IdKondisi", pengembalian.IdKondisi);
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjaman, "IdPinjaman", "IdPinjaman", pengembalian.IdPeminjaman);
            return View(pengembalian);
        }

        // GET: Pengembalians/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalian.FindAsync(id);
            if (pengembalian == null)
            {
                return NotFound();
            }
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraan, "IdKondisi", "IdKondisi", pengembalian.IdKondisi);
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjaman, "IdPinjaman", "IdPinjaman", pengembalian.IdPeminjaman);
            return View(pengembalian);
        }

        // POST: Pengembalians/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPengembalian,TglPengembalian,IdPeminjaman,IdKondisi,Denda")] Pengembalian pengembalian)
        {
            if (id != pengembalian.IdPengembalian)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pengembalian);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PengembalianExists(pengembalian.IdPengembalian))
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
            ViewData["IdKondisi"] = new SelectList(_context.KondisiKendaraan, "IdKondisi", "IdKondisi", pengembalian.IdKondisi);
            ViewData["IdPeminjaman"] = new SelectList(_context.Peminjaman, "IdPinjaman", "IdPinjaman", pengembalian.IdPeminjaman);
            return View(pengembalian);
        }

        // GET: Pengembalians/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pengembalian = await _context.Pengembalian
                .Include(p => p.IdKondisiNavigation)
                .Include(p => p.IdPeminjamanNavigation)
                .FirstOrDefaultAsync(m => m.IdPengembalian == id);
            if (pengembalian == null)
            {
                return NotFound();
            }

            return View(pengembalian);
        }

        // POST: Pengembalians/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pengembalian = await _context.Pengembalian.FindAsync(id);
            _context.Pengembalian.Remove(pengembalian);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PengembalianExists(int id)
        {
            return _context.Pengembalian.Any(e => e.IdPengembalian == id);
        }
    }
}
