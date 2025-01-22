using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimUserbasis
{
    public string Userno { get; set; } = null!;

    public string? Username { get; set; }

    public DateTime? Comeday { get; set; }

    public string? Email { get; set; }

    /// <summary>
    /// Y/N
    /// </summary>
    public string? Nowinjob { get; set; }

    public DateTime? Leaveday { get; set; }

    /// <summary>
    /// 0-已離職,1-未啟用(留職停薪),2-退休人員,3-啟用中
    /// </summary>
    public string? Hirestatus { get; set; }

    public string? Dnno { get; set; }

    public string? Dndesc { get; set; }

    public string? Birthday { get; set; }

    public string? Emailoutside { get; set; }
}
