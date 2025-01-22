using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimUsrUserbasis
{
    public string Userno { get; set; } = null!;

    public string? Username { get; set; }

    public DateTime? Comeday { get; set; }

    public string? Email { get; set; }

    public string? Nowinjob { get; set; }

    public DateTime? Leaveday { get; set; }

    public string? Hirestatus { get; set; }

    public string? Dnno { get; set; }

    public string? Dndesc { get; set; }
}
