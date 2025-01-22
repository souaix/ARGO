using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimUserrolebasis
{
    /// <summary>
    /// 處級代碼(3碼)+部門/模組代碼(3碼)+課級代碼(3碼)+流水號(4碼)
    /// </summary>
    public string Roleno { get; set; } = null!;

    public string? Rolename { get; set; }

    /// <summary>
    /// 0-依部門預設,1-依模組,9-系統人員/主任以上
    /// </summary>
    public decimal? Roletype { get; set; }

    public DateTime? Createdate { get; set; }

    public string? Creator { get; set; }

    public DateTime? Updatedate { get; set; }

    public string? Updater { get; set; }
}
