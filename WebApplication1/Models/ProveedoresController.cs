using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly SampleDbContext _context;

        public ProveedoresController(SampleDbContext context)
        {
            _context = context;
        }

        // GET: /Proveedores
        public async Task<IActionResult> Index()
        {
            var list = await _context.Proveedores.ToListAsync();
            return View("~/Views/Proveedors/Index.cshtml", list);
        }

        // GET: /Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null) return NotFound();

            return View("~/Views/Proveedors/Details.cshtml", proveedor);
        }

        // GET: /Proveedores/Create
        public IActionResult Create()
        {
            return View("~/Views/Proveedors/Create.cshtml");
        }

        // POST: /Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,RUC,Telefono,Email,Direccion,FechaRegistro")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Proveedors/Create.cshtml", proveedor);
        }

        // GET: /Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return NotFound();
            return View("~/Views/Proveedors/Edit.cshtml", proveedor);
        }

        // POST: /Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,RUC,Telefono,Email,Direccion,FechaRegistro")] Proveedor proveedor)
        {
            if (id != proveedor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Proveedores.AnyAsync(e => e.Id == proveedor.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Proveedors/Edit.cshtml", proveedor);
        }

        // GET: /Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(m => m.Id == id);
            if (proveedor == null) return NotFound();

            return View("~/Views/Proveedors/Delete.cshtml", proveedor);
        }

        // POST: /Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
