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
			List<Modalidade> modalidades = _context.Modalidades.ToList();
			ViewBag.modalidadesSelectList = new SelectList(modalidades, "Id", "Nome");
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
				if (!string.IsNullOrWhiteSpace(treinadorRepositorio.treinador.Cpf) && !string.IsNullOrWhiteSpace(treinadorRepositorio.treinador.Nome) && !string.IsNullOrWhiteSpace(treinadorRepositorio.treinador.Idade.ToString()) && !string.IsNullOrWhiteSpace(treinadorRepositorio.Modalidade.ToString()))
				{
                  
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
		public async Task<IActionResult> EditarModalidades(int? id)
		{
			if (id == null || _context.Treinadores == null)
			{
				return NotFound();
			}

			var treinador = await _context.Treinadores.FirstOrDefaultAsync(a => a.Id == id);
			TreinadorModalidadeRepositorio reTreinador = new TreinadorModalidadeRepositorio();
			reTreinador.treinador = treinador;
			reTreinador.treinador.TreinadoresModalidades = _context.TreinadoresModalidades.Where(m => m.FkTreinadores == reTreinador.treinador.Id).ToList();



			foreach (TreinadoresModalidade a in reTreinador.treinador.TreinadoresModalidades)
			{
				reTreinador.modalidadesLista.Add(_context.Modalidades.Find(a.FkModalidades));
			}
			if (treinador == null)
			{
				return NotFound();
			}

			return View(reTreinador);
		}

		public async Task<IActionResult> NovaModalidade(int id)
		{
			if (id == null || _context.Treinadores == null)
			{
				return NotFound();
			}

            var treinador =  _context.Treinadores.Include(m => m.TreinadoresModalidades).FirstOrDefault(m => m.Id==id); ;
			if (treinador == null)
			{
				return NotFound();
			}
			
			TreinadorModalidadeRepositorio ReTreinador = new TreinadorModalidadeRepositorio();
			List<Modalidade> modalidades = _context.Modalidades.ToList();
			foreach (Modalidade item in modalidades)
			{
				foreach (var item1 in treinador.TreinadoresModalidades)
				{
					if (item.Id == item1.FkModalidades)
					{
						modalidades.Remove(item);
					}
				}

			}
			ReTreinador.treinador = treinador;
  
            ViewBag.modalidadesSelectList = new SelectList(modalidades, "Id", "Nome");
            return View(ReTreinador);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AdicionarModalidadeTreinador(TreinadorModalidadeRepositorio reTreinador)
		{
			if (reTreinador.treinador == null)
			{
				return NotFound("Problemas para cadastrar");
			}
			Modalidade m = new Modalidade();
            TreinadoresModalidade modalidadesTreinador = new TreinadoresModalidade();
			m = _context.Modalidades.FirstOrDefault(mod => mod.Id == reTreinador.Modalidade.Id);
			if (m == null)
			{
				return BadRequest("A modalidade não existe no banco de dados");
			}
			reTreinador.treinador = await _context.Treinadores.FindAsync(reTreinador.treinador.Id);
			TreinadoresModalidade ma = new TreinadoresModalidade();
            ma.FkTreinadoresNavigation = reTreinador.treinador;
			ma.FkModalidadesNavigation = m;


			_context.TreinadoresModalidades.Add(ma);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> ExcluirModalidades(int id)
		{


			try
			{
				TreinadoresModalidade modalidadesTreinador = new TreinadoresModalidade();
				modalidadesTreinador = _context.TreinadoresModalidades.Find(id);
				_context.TreinadoresModalidades.Remove(modalidadesTreinador);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{

				return BadRequest("Algo deu muito errado");



			}
			return RedirectToAction(nameof(Index));
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
