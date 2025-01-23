using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.LtCimLtEdc;

public partial class ArgoCimCimUserrolebasis
{
	/// <summary>
	/// 處級代碼(3碼)+部門/模組代碼(3碼)+課級代碼(3碼)+流水號(4碼)
	/// </summary>
	[Display(Name = "角色代碼")]
	public string Roleno { get; set; } = null!;

	[Display(Name = "名稱說明")]
	public string? Rolename { get; set; }

	/// <summary>
	/// 0-依部門預設,1-依模組,9-系統人員/主任以上
	/// </summary>
	[Display(Name = "類型")]
	public decimal? Roletype { get; set; }

	[NotMapped]
	[Display(Name = "類型")]
	public string? Roletypetxt { get; set; }

	[Display(Name = "建立時間")]
	public DateTime? Createdate { get; set; }

	[Display(Name = "建立人")]
	public string? Creator { get; set; }

	[Display(Name = "修改時間")]
	public DateTime? Updatedate { get; set; }

	[Display(Name = "修改人")]
	public string? Updater { get; set; }
}
