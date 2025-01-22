using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimScadadevicebasis
{
    public string? Serialno { get; set; }

    public string Deviceno { get; set; } = null!;

    public string? Devicename { get; set; }

    public string? Ip { get; set; }

    public string? Mac { get; set; }

    /// <summary>
    /// COM port or RTU Address
    /// </summary>
    public string? Com { get; set; }

    /// <summary>
    /// MQTT,MODBUS,WEBSOCKET
    /// </summary>
    public string? Devicetype { get; set; }

    public string? Devicegroupid { get; set; }

    public string? Location { get; set; }

    public string? Alarm { get; set; }

    public string? Remark { get; set; }

    public decimal? Offset { get; set; }

    /// <summary>
    /// -1(OverSpec)、-2(Alarm)、0(Idle)、1(Run)、9(UserHold)
    /// </summary>
    public decimal? Status { get; set; }

    public DateTime? Createdate { get; set; }

    public string? Creator { get; set; }

    public DateTime? Updatedate { get; set; }

    /// <summary>
    /// 1:Enable
    /// </summary>
    public decimal? Enable { get; set; }
}
