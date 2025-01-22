using System;
using System.Collections.Generic;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimSystemmenulist
{
	public decimal Id { get; set; }

	public string? Belongbu { get; set; }

	public string? Belongdept { get; set; }

	public string? Level01 { get; set; }

	public string? Level01no { get; set; }

	public string? Level02 { get; set; }

	public string? Level02no { get; set; }

	public string? Level03 { get; set; }

	public string Level03no { get; set; } = null!;

	public string? Icon { get; set; }

	public string? Controller { get; set; }

	public string? Action { get; set; }

	public decimal? Sequence { get; set; }

	public string? Imgname { get; set; }

	public string? Keyword { get; set; }

	public string? Enabled { get; set; }

	public string? Remark { get; set; }

	public DateTime? Createdate { get; set; }

	public string? Creator { get; set; }

	public DateTime? Updatedate { get; set; }

	public string? Updater { get; set; }
}
