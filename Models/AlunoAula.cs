using System;
using System.Collections.Generic;

namespace Gerenciador_CT.Models;

public partial class AlunoAula
{
    public int Id { get; set; }

    public int FkAluno { get; set; }

    public int FkAula { get; set; }

    public virtual Aluno FkAlunoNavigation { get; set; } = null!;

    public virtual Aula FkAulaNavigation { get; set; } = null!;
}
