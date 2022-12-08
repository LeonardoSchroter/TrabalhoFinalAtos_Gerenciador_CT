using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gerenciador_CT.Models;
using Gerenciador_CT.Repositorio;

namespace Gerenciador_CT.Controllers
{
    public class TreinadoresController : Controller
    {
        private readonly GerenciadorCtDbContext _context;

        public TreinadoresController(GerenciadorCtDbContext context)
        {
            _context = context;
        }

        // GET: Treinadores
        public async Task<IActionResult> Index()
        {
            List <Treinadore> treinadores = new List<Treinadore>();
            treinadores = (from Treinadore a in _context.Treinadores select a).Include(a => a.TreinadoresModalidades).ToList<Treinadore>();
			return View(treinadores);
        }

        // GET: Treinadores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Treinadores == null)
            {
                return NotFound();
            }

            var treinadore = await _context.Treinadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treinadore == null)
            {
                return NotFound();
            }

            return View(treinadore);
        }

        // GET: Treinadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Treinadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TreinadorModalidadeRepositorio treinadorRepositorio)
        {
			try
			{
				if (!string.IsNullOrWhiteSpace(treinadorRepositorio.treinador.Cpf) && !string.IsNullOrWhiteSpace(treinadorRepositorio.treinador.Nome) && !string.IsNullOrWhiteSpace(treinadorRepositorio.treinador.Idade.ToString()) && !string.IsNullOrWhiteSpace(treinadorRepositorio.nomeModalidade))
				{
                    treinadorRepositorio.Modalidade = _context.Modalidades.FirstOrDefault(m => m.Nome.ToUpper() == treinadorRepositorio.nomeModalidade.ToUpper());
                    treinadorRepositorio.treinadorModalidade.FkTreinadores = treinadorRepositorio.treinador.Id;
                    treinadorRepositorio.treinadorModalidade.FkModalidades = treinadorRepositorio.Modalidade.Id;
                    treinadorRepositorio.treinador.TreinadoresModalidades.Add(treinadorRepositorio.treinadorModalidade);
					_context.Treinadores.Add(treinadorRepositorio.treinador);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				throw new System.Exception("Houve um erro ao adicionar o treinador ");
			}
			catch (Exception)
			{
				throw new System.Exception("Houve um erro ao adicionar o treinador");
			}
		}

        // GET: Treinadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Treinadores == null)
            {
                return NotFound();
            }

            var treinadore = await _context.Treinadores.FindAsync(id);
            if (treinadore == null)
            {
                return NotFound();
            }
            return View(treinadore);
        }

        // POST: Treinadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cpf,Idade")] Treinadore treinadore)
        {
            if (id != treinadore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treinadore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreinadoreExists(treinadore.Id))
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
            return View(treinadore);
        }

        // GET: Treinadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Treinadores == null)
            {
                return NotFound();
            }

            var treinadore = await _context.Treinadores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treinadore == null)
            {
                return NotFound();
            }

            return View(treinadore);
        }

        // POST: Treinadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Treinadores == null)
            {
                return Problem("Entity set 'GerenciadorCtDbContext.Treinadores'  is null.");
            }
            var treinadore = await _context.Treinadores.FindAsync(id);
            if (treinadore != null)
            {
                _context.Treinadores.Remove(treinadore);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreinadoreExists(int id)
        {
          return _context.Treinadores.Any(e => e.Id == id);
        }
    }
}
