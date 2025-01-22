using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimScadadevicelimit
{
    public string Deviceno { get; set; } = null!;

    public string? Dvparam { get; set; }

    public string? Dvparamname { get; set; }

    public decimal? Dvuppercontrollimit { get; set; }

    public decimal? Dvlowercontrollimit { get; set; }

    public decimal? Dvupperspeclimit { get; set; }

    public decimal? Dvlowerspeclimit { get; set; }

    public DateTime? Createdate { get; set; }

    public string? Creator { get; set; }

    public DateTime? Updatedate { get; set; }
}
