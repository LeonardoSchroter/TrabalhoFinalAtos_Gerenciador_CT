using System;
using System.Collections.Generic;

namespace Gerenciador_CT.Models;

public partial class Modalidade
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Descricao { get; set; } = null!;

    public virtual ICollection<ModalidadesAluno> ModalidadesAlunos { get; set; } = new List<ModalidadesAluno>();

    public virtual ICollection<TreinadoresModalidade> TreinadoresModalidades { get; set; } = new List<TreinadoresModalidade>();
}
