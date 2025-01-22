using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimScadadevicegroup
{
    public string Devicegroupno { get; set; } = null!;

    public string? Devicegroupname { get; set; }

    public string? Devicegrouptype { get; set; }

    public DateTime? Createdate { get; set; }

    public string? Creator { get; set; }

    public DateTime? Updatedate { get; set; }

    public string? Updater { get; set; }
}
