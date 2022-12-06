using System;
using System.Collections.Generic;

namespace Gerenciador_CT.Models;

public partial class TreinadoresModalidade
{
    public int Id { get; set; }

    public int FkTreinadores { get; set; }

    public int FkModalidades { get; set; }

    public virtual Modalidade FkModalidadesNavigation { get; set; } = null!;

    public virtual Treinadore FkTreinadoresNavigation { get; set; } = null!;
}
