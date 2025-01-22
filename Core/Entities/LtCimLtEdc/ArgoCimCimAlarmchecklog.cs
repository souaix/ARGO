using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimAlarmchecklog
{
    public string Deviceno { get; set; } = null!;

    public string Colname { get; set; } = null!;

    public string? Colcname { get; set; }

    public DateTime Createdate { get; set; }

    public DateTime? Importdate { get; set; }

    public string? Value { get; set; }

    public decimal? Checkstatus { get; set; }

    public decimal? Checkcount { get; set; }
}
