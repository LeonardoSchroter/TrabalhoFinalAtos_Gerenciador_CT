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
	public class AulasController : Controller
	{
		private readonly GerenciadorCtDbContext _context;

		public AulasController(GerenciadorCtDbContext context)
		{
			_context = context;
		}

		// GET: Aulas
		public async Task<IActionResult> Index()
		{
			var gerenciadorCtDbContext = _context.Aulas.Include(a => a.FkHorarioNavigation).Include(a => a.FkTreinadorNavigation).Include(a => a.AlunoAulas).ToList();
			AulaRepositorio aulaRe = new AulaRepositorio();
			aulaRe.aulaLista = gerenciadorCtDbContext;
			foreach (var item in aulaRe.aulaLista)
			{
				foreach (var item2 in item.AlunoAulas)
				{
					aulaRe.alunoLista = _context.Alunos.Include(a => a.AlunoAulas).ToList();
				}
			}
			

			return View(aulaRe);
		}

		// GET: Aulas/Details/5
		public async Task<IActionResult> EditarAlunos(int? id)
		{
			if (id == null || _context.Aulas == null)
			{
				return NotFound();
			}
			AulaRepositorio aula = new AulaRepositorio();
			aula.aula = await _context.Aulas
			   .Include(a => a.FkHorarioNavigation)
			   .Include(a => a.FkTreinadorNavigation)
			   .Include(a => a.AlunoAulas)
			   .FirstOrDefaultAsync(m => m.Id == id);
			foreach (var item in aula.aula.AlunoAulas)
			{
				aula.alunoLista.Add(_context.Alunos.Find(item.FkAluno));
			}
			if (aula == null)
			{
				return NotFound();
			}

			return View(aula);
		}
		
		public async Task<IActionResult> DeleteAluno( int? id)
		{
			if (id == null || _context.Aulas == null)
			{
				return NotFound();
			}
			try
			{
				AlunoAula alunoAula = _context.AlunoAulas.FirstOrDefault(a => a.Id==id);
				_context.Remove(alunoAula);
				_context.SaveChanges();
			}


			catch (Exception)
			{
				return BadRequest("Erro ao remover aluno "+ id);
			}



			return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> NovoAluno(int id)
		{
			if (id == null || _context.Aulas == null)
			{
				return NotFound();
			}

			AulaRepositorio aulaRe = new AulaRepositorio();
			aulaRe.aula =  _context.Aulas.Include(a => a.AlunoAulas).FirstOrDefault(a => a.Id == id); 
			if (aulaRe.aula == null)
			{
				return NotFound();
			}
			List<Aluno> alunos = aulaRe.todosAlunos;
			foreach(Aluno item in aulaRe.todosAlunos.ToList())
			{
				foreach(var item1 in aulaRe.aula.AlunoAulas)
				{
					if(item.Id == item1.FkAluno)
					{
						alunos.Remove(item);
					}
				}
				
			}


			ViewBag.alunosSelectList = new SelectList(alunos,"Id", "Nome");  //aulaRe.todosAlunos;
			
			return View(aulaRe);
		}
		


			

			
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AdicionarAlunoAula(AulaRepositorio reAula)
		{
			if (reAula.aula == null)
			{
				return NotFound("Problemas para cadastrar");
			}
			Aluno aluno = new Aluno();
			AlunoAula alunoAula = new AlunoAula();
			aluno = _context.Alunos.FirstOrDefault(mod => mod.Id == reAula.aluno.Id);
			if (alunoAula == null)
			{
				return BadRequest(" Algo está errado");
			}
			reAula.aula = await _context.Aulas.FindAsync(reAula.aula.Id);
			 
			alunoAula.FkAulaNavigation = reAula.aula;
			alunoAula.FkAlunoNavigation = aluno;


			_context.AlunoAulas.Add(alunoAula);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

		// GET: Aulas/Create
		public IActionResult Create()
		{

			return View();
		}

		// POST: Aulas/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AulaRepositorio reAula)
		{
			if (reAula.horario.Dia != null || reAula.horario.Hora != null || reAula.treinador.Nome != null)
			{

				reAula.aula.FkHorarioNavigation = reAula.horario;
				reAula.aula.FkTreinadorNavigation = _context.Treinadores.FirstOrDefault(t => t.Nome.ToUpper() == reAula.treinador.Nome.ToUpper());
				_context.Aulas.Add(reAula.aula);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(reAula);
		}

		// GET: Aulas/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Aulas == null)
			{
				return NotFound();
			}

			AulaRepositorio aula = new AulaRepositorio();
			aula.aula = _context.Aulas.Include(a => a.FkTreinadorNavigation).Include(a => a.FkHorarioNavigation).FirstOrDefault(a => a.Id == id);
			if (aula == null)
			{
				return NotFound();
			}

			return View(aula);
		}

		// POST: Aulas/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, AulaRepositorio aulaRe)
		{
			if (id != aulaRe.aula.Id)
			{
				return NotFound();
			}

			if (aulaRe.aula.FkTreinadorNavigation.Nome != null || aulaRe.aula.FkHorarioNavigation.Hora != null || aulaRe.treinador.Nome != null)
			{
				try
				{

					Horario horario = _context.Horarios.FirstOrDefault(h => h.Hora == aulaRe.aula.FkHorarioNavigation.Hora && h.Dia == aulaRe.aula.FkHorarioNavigation.Dia);
					if (horario != null)
					{
						aulaRe.aula.FkHorarioNavigation = horario;
					}

					aulaRe.aula.FkTreinadorNavigation = _context.Treinadores.FirstOrDefault(t => t.Nome.ToUpper() == aulaRe.aula.FkTreinadorNavigation.Nome.ToUpper());
					_context.Update(aulaRe.aula);
					await _context.SaveChangesAsync();
				}



				catch (DbUpdateConcurrencyException)
				{
					if (!AulaExists(aulaRe.aula.Id))
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

			return View(aulaRe);
		}

		// GET: Aulas/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Aulas == null)
			{
				return NotFound();
			}

			var aula = await _context.Aulas
				.Include(a => a.FkHorarioNavigation)
				.Include(a => a.FkTreinadorNavigation)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (aula == null)
			{
				return NotFound();
			}

			return View(aula);
		}

		// POST: Aulas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Aulas == null)
			{
				return Problem("Entity set 'GerenciadorCtDbContext.Aulas'  is null.");
			}
			var aula = await _context.Aulas.FindAsync(id);
			if (aula != null)
			{
				_context.Aulas.Remove(aula);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool AulaExists(int id)
		{
			return _context.Aulas.Any(e => e.Id == id);
		}
	}
}
