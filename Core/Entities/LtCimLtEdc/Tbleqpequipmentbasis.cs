using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class Tbleqpequipmentbasis
{
    public string Equipmentno { get; set; } = null!;

    public string? Equipmenttype { get; set; }

    public int? Capacity { get; set; }

    public string? Vendorno { get; set; }

    public string? Modelno { get; set; }

    public string? Description { get; set; }

    public string? Creator { get; set; }

    public DateTime? Createdate { get; set; }

    public bool? Issuestate { get; set; }

    public string? Engineergroupno { get; set; }

    public string? Assetno { get; set; }

    public string? Equipmentclass { get; set; }

    public byte? Loadport { get; set; }

    public bool? Autoflag { get; set; }

    public string? Eacontroller { get; set; }

    public bool? Eqprecipe { get; set; }

    public string? Qclistno { get; set; }

    public decimal? Maxtime { get; set; }

    public decimal? Fixeqptime { get; set; }

    public decimal? Vareqptime { get; set; }

    public int? Countopunitqty { get; set; }

    public int? Counteqpunitqty { get; set; }

    public short? Counter { get; set; }

    public string? Equipmentname { get; set; }

    public string? Erpno { get; set; }

    public string? Queueserver { get; set; }

    public string? Requestmsgqueue { get; set; }

    public string? Responsemsgqueue { get; set; }

    public bool? Chamberflag { get; set; }

    public string? Belongequipmentno { get; set; }

    public string? ProberTester { get; set; }

    public string? Equipmentip { get; set; }

    public string? Equipmentfn { get; set; }

    public byte? Equipmentclassno { get; set; }
}
