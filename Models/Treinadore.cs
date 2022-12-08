using System;
using System.Collections.Generic;

namespace Gerenciador_CT.Models;

public partial class Treinadore
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public int? Idade { get; set; }

    public virtual ICollection<Aula> Aulas { get; set; } = new List<Aula>();

    public virtual ICollection<TreinadoresModalidade> TreinadoresModalidades { get; set; } = new List<TreinadoresModalidade>();
}
