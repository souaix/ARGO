using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

/// <summary>
/// 客戶_MAP_BU2019_頭檔
/// </summary>
public partial class CuMapBu2019
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
    /// 建立時間
    /// </summary>
    public DateTime? MpCtime { get; set; }

    /// <summary>
    /// 建立人員
    /// </summary>
    public string? MpCuser { get; set; }

    /// <summary>
    /// 異動時間
    /// </summary>
    public DateTime? MpMtime { get; set; }

    /// <summary>
    /// 異動人員
    /// </summary>
    public string? MpMuser { get; set; }

    /// <summary>
    /// 啟/停用
    /// </summary>
    public string? MpIfEnable { get; set; }

    /// <summary>
    /// MAP DEVICE
    /// </summary>
    public string? MpDevice { get; set; }

    /// <summary>
    /// 晶圓批號
    /// </summary>
    public string? MpWfLotNo { get; set; }

    /// <summary>
    /// LOT ID
    /// </summary>
    public string? MpLotId { get; set; }

    /// <summary>
    /// 片數
    /// </summary>
    public decimal? MpWfQty { get; set; }

    /// <summary>
    /// 對照編號
    /// </summary>
    public string? MpBmNo { get; set; }

    /// <summary>
    /// NOTCH
    /// </summary>
    public string? MpNotch { get; set; }

    /// <summary>
    /// 總好品數
    /// </summary>
    public decimal? MpTotGqty { get; set; }

    /// <summary>
    /// 總壞品數
    /// </summary>
    public decimal? MpTotBqty { get; set; }

    /// <summary>
    /// Th SUMMARY檔名
    /// </summary>
    public string? MpThSumFilename { get; set; }

    /// <summary>
    /// Th SUMMARY壓縮檔名
    /// </summary>
    public string? MpThSumZipFilename { get; set; }

    /// <summary>
    /// Th SUMMARY壓縮檔內容
    /// </summary>
    public byte[]? MpThSumZipFiledata { get; set; }
}
