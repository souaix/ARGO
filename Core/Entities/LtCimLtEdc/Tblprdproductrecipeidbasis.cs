using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class Tblprdproductrecipeidbasis
{
    public string Id { get; set; } = null!;

    public string Productno { get; set; } = null!;

    public string Customerpartver { get; set; } = null!;

    public string Opno { get; set; } = null!;

    public string Modelno { get; set; } = null!;

    public string? Recipeid { get; set; }

    public string? IpcRecipe { get; set; }

    public string? Inuse { get; set; }

    public string? Creator { get; set; }

    public DateTime? Createdate { get; set; }

    public string? Editor { get; set; }

    public DateTime? Editdate { get; set; }

    public decimal? Motypeno { get; set; }
}
