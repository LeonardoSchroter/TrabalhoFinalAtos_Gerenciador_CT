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
	public class AlunosController : Controller
	{
		private readonly GerenciadorCtDbContext _context;

		public AlunosController(GerenciadorCtDbContext context)
		{
			_context = context;
		}

		// GET: Alunos
		public async Task<IActionResult> Index()
		{

			List<Aluno> alunos = new List<Aluno>();
			alunos = (from Aluno a in _context.Alunos select a).Include(a => a.ModalidadesAlunos).ToList<Aluno>();
			return View(alunos);
		}

		// GET: Alunos/Details/5
		public async Task<IActionResult> EditarModalidades(int? id)
		{
			if (id == null || _context.Alunos == null)
			{
				return NotFound();
			}

			var aluno = await _context.Alunos.FirstOrDefaultAsync(a => a.Id == id);
			AlunoModalidadesRepositorio reAluno = new AlunoModalidadesRepositorio();
			reAluno.aluno = aluno;
			reAluno.aluno.ModalidadesAlunos = _context.ModalidadesAlunos.Where(m => m.FkAlunos == aluno.Id).ToList();



			foreach (ModalidadesAluno a in reAluno.aluno.ModalidadesAlunos)
			{
				reAluno.modalidadesLista.Add(_context.Modalidades.Find(a.FkModalidades));
			}
			if (aluno == null)
			{
				return NotFound();
			}

			return View(reAluno);
		}

		public async Task<IActionResult> NovaModalidade(int id)
		{
			if (id == null || _context.Alunos == null)
			{
				return NotFound();
			}

			var aluno = await _context.Alunos.FindAsync(id);
			if (aluno == null)
			{
				return NotFound();
			}
			AlunoModalidadesRepositorio ReAluno = new AlunoModalidadesRepositorio();
			ReAluno.aluno = aluno;
			return View(ReAluno);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AdicionarModalidadeAluno(AlunoModalidadesRepositorio reAluno)
		{
			if (reAluno.aluno == null)
			{
				return NotFound("Problemas para cadastrar");
			}
			Modalidade m = new Modalidade();
			m = _context.Modalidades.FirstOrDefault(mod => mod.Nome.ToUpper() == reAluno.nomeModalidade.ToUpper());
			if (m == null)
			{
				return BadRequest("A modalidade não existe no banco de dados");
			}
			reAluno.aluno = await _context.Alunos.FindAsync(reAluno.aluno.Id);
			ModalidadesAluno ma = new ModalidadesAluno();
			ma.FkAlunosNavigation = reAluno.aluno;
			ma.FkModalidadesNavigation = m;
			
			
			_context.ModalidadesAlunos.Add(ma);
			_context.SaveChanges();

			return RedirectToAction(nameof(Index));
		}






		public async Task<IActionResult> ExcluirModalidades(int id)
		{


			try
			{
				ModalidadesAluno modalidadesAlunos = new ModalidadesAluno();
				modalidadesAlunos = _context.ModalidadesAlunos.Find(id);
				_context.ModalidadesAlunos.Remove(modalidadesAlunos);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{

				return BadRequest("Algo deu muito errado");



			}
			return RedirectToAction(nameof(Index));
		}



		// GET: Alunos/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Alunos/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AlunoModalidadesRepositorio alunoRe)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(alunoRe.aluno.Cpf) && !string.IsNullOrWhiteSpace(alunoRe.aluno.Nome) && !string.IsNullOrWhiteSpace(alunoRe.aluno.Idade.ToString()) && !string.IsNullOrWhiteSpace(alunoRe.nomeModalidade))
				{
					alunoRe.Modalidade = _context.Modalidades.FirstOrDefault(m => m.Nome == alunoRe.nomeModalidade);
					alunoRe.lista.FkAlunos = alunoRe.aluno.Id;
					alunoRe.lista.FkModalidades = alunoRe.Modalidade.Id;
					alunoRe.aluno.ModalidadesAlunos.Add(alunoRe.lista);
					_context.Alunos.Add(alunoRe.aluno);
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				return View(alunoRe);
			}
			catch (Exception)
			{
				throw new System.Exception("Houve um erro ao adicionar o contato");
			}

		}

		// GET: Alunos/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Alunos == null)
			{
				return NotFound();
			}

			var aluno = await _context.Alunos.FindAsync(id);
			if (aluno == null)
			{
				return NotFound();
			}
			return View(aluno);
		}

		// POST: Alunos/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Idade,Cpf")] Aluno aluno)
		{
			if (id != aluno.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(aluno);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AlunoExists(aluno.Id))
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
			return View(aluno);
		}

		// GET: Alunos/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Alunos == null)
			{
				return NotFound();
			}

			var aluno = await _context.Alunos
				.FirstOrDefaultAsync(m => m.Id == id);
			if (aluno == null)
			{
				return NotFound();
			}

			return View(aluno);
		}

		// POST: Alunos/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Alunos == null)
			{
				return Problem("Entity set 'GerenciadorCtDbContext.Alunos'  is null.");
			}
			var aluno = await _context.Alunos.FindAsync(id);
			if (aluno != null)
			{
				_context.Alunos.Remove(aluno);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool AlunoExists(int id)
		{
			return _context.Alunos.Any(e => e.Id == id);
		}
	}
}
