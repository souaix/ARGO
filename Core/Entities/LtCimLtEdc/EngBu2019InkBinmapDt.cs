using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

/// <summary>
/// 工程_BU2019_INKLESS_BIN對照_尾檔
/// </summary>
public partial class EngBu2019InkBinmapDt
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
    /// TH BIN NO
    /// </summary>
    public string? ThCode { get; set; }

    /// <summary>
    /// BIN類別
    /// </summary>
    public string? BinType { get; set; }

    /// <summary>
    /// CU BIN NO
    /// </summary>
    public string CuBinCode { get; set; } = null!;
}
