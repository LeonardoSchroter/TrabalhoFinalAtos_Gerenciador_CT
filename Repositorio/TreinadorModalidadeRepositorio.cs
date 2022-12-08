using Gerenciador_CT.Models;

namespace Gerenciador_CT.Repositorio
{
	public class TreinadorModalidadeRepositorio
	{
		public Treinadore treinador { get; set; }
		public TreinadoresModalidade treinadorModalidade { get; set; }
		public Modalidade Modalidade { get; set; }
		public string nomeModalidade { get; set; }
		public List<TreinadoresModalidade> modalidadeTreinadoresLista { get; set; }
		public List<Modalidade> modalidadesLista { get; set; }
		private readonly GerenciadorCtDbContext _context;
		public List<Modalidade> todasModalidades { get; set; }

		public TreinadorModalidadeRepositorio()
		{
			treinador = new Treinadore();
			treinadorModalidade = new TreinadoresModalidade();
			Modalidade = new Modalidade();
			modalidadeTreinadoresLista = new List<TreinadoresModalidade>();
			modalidadesLista = new List<Modalidade>();
			_context = new GerenciadorCtDbContext();
			todasModalidades = _context.Modalidades.ToList();
		}
	}
}
