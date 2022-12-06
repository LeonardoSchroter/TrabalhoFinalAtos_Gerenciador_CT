using System;
using System.Collections.Generic;

namespace Gerenciador_CT.Models;

public partial class Aula
{
    public int Id { get; set; }

    public int FkHorario { get; set; }

    public int FkTreinador { get; set; }

    public virtual ICollection<AlunoAula> AlunoAulas { get; } = new List<AlunoAula>();

    public virtual Horario FkHorarioNavigation { get; set; } = null!;

    public virtual Treinadore FkTreinadorNavigation { get; set; } = null!;
}
