using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

/// <summary>
/// 客戶_MAP_BU2019_尾檔
/// </summary>
public partial class CuMapBu2019Dt
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
    /// WF ID
    /// </summary>
    public string? MpdWfId { get; set; }

    /// <summary>
    /// WF NUMBER
    /// </summary>
    public string? MpdWfNum { get; set; }

    /// <summary>
    /// PASS QTY
    /// </summary>
    public decimal? MpdWfGqty { get; set; }

    /// <summary>
    /// FAIL QTY
    /// </summary>
    public decimal? MpdWfBqty { get; set; }

    /// <summary>
    /// TEST QTY
    /// </summary>
    public decimal? MpdTestQty { get; set; }

    /// <summary>
    /// MAP版本
    /// </summary>
    public string? MpdVer { get; set; }

    /// <summary>
    /// Yield
    /// </summary>
    public decimal? MpdYield { get; set; }

    /// <summary>
    /// 客戶MAP檔名
    /// </summary>
    public string? MpdFilename { get; set; }

    /// <summary>
    /// 客戶MAP壓縮檔名
    /// </summary>
    public string? MpdZipFilename { get; set; }

    /// <summary>
    /// 客戶MAP壓縮檔內容
    /// </summary>
    public byte[]? MpdZipFiledata { get; set; }

    /// <summary>
    /// Th MAP檔名
    /// </summary>
    public string? MpdThFilename { get; set; }

    /// <summary>
    /// Th MAP壓縮檔名
    /// </summary>
    public string? MpdThZipFilename { get; set; }

    /// <summary>
    /// Th MAP壓縮檔內容
    /// </summary>
    public byte[]? MpdThZipFiledata { get; set; }
}
