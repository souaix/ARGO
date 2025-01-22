using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Main;

public class VMenuList
{
	public int ID { get; set; }

	[Display(Name = "系統名稱")]
	[Required(ErrorMessage = "請輸入系統名稱")]
	public string? CLASS01 { get; set; }


	[Display(Name = "模組名稱")]
	[Required(ErrorMessage = "請輸入模組名稱")]
	public string? CLASS02 { get; set; }

	[Display(Name = "功能名稱")]
	[Required(ErrorMessage = "請輸入功能名稱")]
	public string? CLASS03 { get; set; }

	public string? ICON { get; set; }

	[Display(Name = "控制器")]
	public string? CONTROLLER { get; set; }

	public string? ACTION { get; set; }

	[Display(Name = "顯示順序")]
	public int? SEQUENCE { get; set; }
}