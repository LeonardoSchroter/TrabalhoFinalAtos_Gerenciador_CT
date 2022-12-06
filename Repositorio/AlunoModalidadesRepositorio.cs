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

		public AlunoModalidadesRepositorio()
		{
			aluno= new Aluno();
			lista= new ModalidadesAluno();
			Modalidade= new Modalidade();
		}
				
		
	}
}
