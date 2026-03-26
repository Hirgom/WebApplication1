using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MovimientosInventarioController : Controller
    {
        private readonly SampleDbContext _context;

        public MovimientosInventarioController(SampleDbContext context)
        {
            _context = context;
        }

        // GET: /MovimientosInventario
        public async Task<IActionResult> Index()
        {
            var data = _context.MovimientosInventario.Include(m => m.Producto);
            return View("~/Views/MovimientoInventarios/Index.cshtml", await data.ToListAsync());
        }

        // GET: /MovimientosInventario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var movimiento = await _context.MovimientosInventario
                .Include(m => m.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movimiento == null) return NotFound();

            return View("~/Views/MovimientoInventarios/Details.cshtml", movimiento);
        }

        // GET: /MovimientosInventario/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Products, "ProductId", "Name");
            return View("~/Views/MovimientoInventarios/Create.cshtml");
        }

        // POST: /MovimientosInventario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductoId,TipoMovimiento,Cantidad,PrecioUnitario,FechaMovimiento,Observacion")] MovimientoInventario movimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movimiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Products, "ProductId", "Name", movimiento.ProductoId);
            return View("~/Views/MovimientoInventarios/Create.cshtml", movimiento);
        }

        // GET: /MovimientosInventario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movimiento = await _context.MovimientosInventario.FindAsync(id);
            if (movimiento == null) return NotFound();
            ViewData["ProductoId"] = new SelectList(_context.Products, "ProductId", "Name", movimiento.ProductoId);
            return View("~/Views/MovimientoInventarios/Edit.cshtml", movimiento);
        }

        // POST: /MovimientosInventario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductoId,TipoMovimiento,Cantidad,PrecioUnitario,FechaMovimiento,Observacion")] MovimientoInventario movimiento)
        {
            if (id != movimiento.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.MovimientosInventario.AnyAsync(e => e.Id == movimiento.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Products, "ProductId", "Name", movimiento.ProductoId);
            return View("~/Views/MovimientoInventarios/Edit.cshtml", movimiento);
        }

        // GET: /MovimientosInventario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movimiento = await _context.MovimientosInventario
                .Include(m => m.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movimiento == null) return NotFound();

            return View("~/Views/MovimientoInventarios/Delete.cshtml", movimiento);
        }

        // POST: /MovimientosInventario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimiento = await _context.MovimientosInventario.FindAsync(id);
            if (movimiento != null)
            {
                _context.MovimientosInventario.Remove(movimiento);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
