using System;
using System.Collections.Generic;

namespace Gerenciador_CT.Models;

public partial class Aluno
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int Idade { get; set; }

    public string Cpf { get; set; } = null!;

    public virtual ICollection<AlunoAula> AlunoAulas { get; set; } = new List<AlunoAula>();

    public virtual ICollection<ModalidadesAluno> ModalidadesAlunos { get; set; } = new List<ModalidadesAluno>();
}
