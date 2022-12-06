using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Gerenciador_CT.Models;

public partial class ModalidadesAluno
{
	public int Id { get; set; }

	public int FkAlunos { get; set; }

	public int FkModalidades { get; set; }

	public virtual Aluno FkAlunosNavigation { get; set; } = null!;

	public virtual Modalidade FkModalidadesNavigation { get; set; } = null!;


	
	
}
