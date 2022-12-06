using System;
using System.Collections.Generic;

namespace Gerenciador_CT.Models;

public partial class Horario
{
    public int Id { get; set; }

    public string Dia { get; set; } = null!;

    public string Hora { get; set; } = null!;

    public virtual ICollection<Aula> Aulas { get; } = new List<Aula>();
}
