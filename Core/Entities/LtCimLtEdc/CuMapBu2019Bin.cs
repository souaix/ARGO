using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

/// <summary>
/// 客戶_MAP_BU2019_BIN檔
/// </summary>
public partial class CuMapBu2019Bin
{
    /// <summary>
    /// 公司別
    /// </summary>
    public string CNo { get; set; } = null!;

    /// <summary>
    /// MAP編號
    /// </summary>
    public string MpNo { get; set; } = null!;

    /// <summary>
    /// 項次
    /// </summary>
    public string MpdItem { get; set; } = null!;

    /// <summary>
    /// BIN NO (同欣BIN)
    /// </summary>
    public string MpdBinNo { get; set; } = null!;

    /// <summary>
    /// BIN數量
    /// </summary>
    public decimal? MpdBinQty { get; set; }

    /// <summary>
    /// BIN類別
    /// </summary>
    public string? MpdBinType { get; set; }
}
