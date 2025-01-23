using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimUserroledetail
{
	public string Roleno { get; set; } = null!;

	public string Level03no { get; set; } = null!;

	public DateTime? Createdate { get; set; }

	public string? Creator { get; set; }

	public DateTime? Updatedate { get; set; }

	public string? Updater { get; set; }
}
