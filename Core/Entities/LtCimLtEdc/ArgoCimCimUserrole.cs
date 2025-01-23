using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimUserrole
{
	public string Userno { get; set; } = null!;

	public string? Userrole { get; set; }

	public DateTime? Createdate { get; set; }

	public string? Creator { get; set; }

	public DateTime? Updatedate { get; set; }

	public string? Updater { get; set; }
}
