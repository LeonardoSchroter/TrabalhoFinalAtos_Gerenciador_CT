using Gerenciador_CT.Models;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_CT.Repositorio
{
	public class AulaRepositorio
	{
		public Aula aula { get; set; }
		public Aluno aluno { get; set; }
		public AlunoAula alunoAula { get; set; }
		public Horario horario { get; set; }
		public Treinadore treinador { get; set; }
		public List<Aula> aulaLista { get; set; }
		public List <AlunoAula> alunoAulaLista { get; set; }
		public List<Aluno> alunoLista { get; set; }
		public List<Aluno> todosAlunos { get; set; }
		public readonly GerenciadorCtDbContext _context;

		public AulaRepositorio()
		{
			_context= new GerenciadorCtDbContext();
			aula = new Aula();
			aluno = new Aluno();
			alunoAula = new AlunoAula();
			horario = new Horario();
			treinador = new Treinadore();
			aulaLista = new List<Aula>();
			alunoAulaLista = new List<AlunoAula>();
			alunoLista = new List<Aluno>();
			todosAlunos = _context.Alunos.ToList();
		}
	}
}
