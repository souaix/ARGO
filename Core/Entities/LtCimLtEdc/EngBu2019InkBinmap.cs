using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

/// <summary>
/// 工程_BU2019_INKLESS_BIN對照_頭檔
/// </summary>
public partial class EngBu2019InkBinmap
{
    /// <summary>
    /// 公司別
    /// </summary>
    public string CNo { get; set; } = null!;

    /// <summary>
    /// 對照編號
    /// </summary>
    public string BmNo { get; set; } = null!;

    /// <summary>
    /// 對照說明
    /// </summary>
    public string? BmDesc { get; set; }

    /// <summary>
    /// 啟用
    /// </summary>
    public string? BmIfEnable { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTime? BmCtime { get; set; }

    /// <summary>
    /// 建立人員
    /// </summary>
    public string? BmCuser { get; set; }

    /// <summary>
    /// 異動時間
    /// </summary>
    public DateTime? BmMtime { get; set; }

    /// <summary>
    /// 異動人員
    /// </summary>
    public string? BmMuser { get; set; }

    /// <summary>
    /// 備註
    /// </summary>
    public string? BmRemark { get; set; }
}
