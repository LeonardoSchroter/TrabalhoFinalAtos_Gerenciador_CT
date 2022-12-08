using Gerenciador_CT.Models;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_CT.Repositorio
{
	public class AlunoModalidadesRepositorio
	{
		public Aluno aluno { get; set; }
		public ModalidadesAluno lista { get; set; }
		public Modalidade Modalidade { get; set; }
		public string nomeModalidade { get; set; }
		public List<ModalidadesAluno> modalidadeAlunosLista { get; set; }
		public List<Modalidade> modalidadesLista { get; set; }
		private readonly GerenciadorCtDbContext _context;
		public List<Modalidade> todasModalidades { get; set; }

		public AlunoModalidadesRepositorio()
		{
			aluno= new Aluno();
			lista= new ModalidadesAluno();
			Modalidade= new Modalidade();
			modalidadeAlunosLista = new List<ModalidadesAluno>();
			modalidadesLista = new List<Modalidade>();
			_context = new GerenciadorCtDbContext();
			todasModalidades= _context.Modalidades.ToList();
		}
				
		
	}
}
