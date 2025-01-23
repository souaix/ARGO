using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.cscimEmap
{
	public class ArgoCimSystemMenuList
	{
		public decimal Id { get; set; }

		[Display(Name = "功能代碼")]
		public string Level03No { get; set; } = null!;

		[Display(Name = "功能名稱")]
		public string? Level03 { get; set; }

		[Display(Name = "歸屬BU")]
		public string? BelongBu { get; set; }

		[Display(Name = "歸屬部門")]
		public string? BelongDept { get; set; }

		[Display(Name = "系統代碼")]
		public string? Level01No { get; set; }

		[Display(Name = "系統名稱")]
		public string? Level01 { get; set; }

		[Display(Name = "模組代碼")]
		public string? Level02No { get; set; }

		[Display(Name = "模組名稱")]
		public string? Level02 { get; set; }

		[Display(Name = "ICON")]
		public string? Icon { get; set; }

		[Display(Name = "控制器")]
		public string? Controller { get; set; }

		[Display(Name = "ACTION")]
		public string? Action { get; set; }

		[Display(Name = "排序")]
		public decimal? Sequence { get; set; }

		[Display(Name = "圖片名稱")]
		public string? ImgName { get; set; }

		[Display(Name = "關鍵字")]
		public string? Keyword { get; set; }

		[Display(Name = "備註")]
		public string? Remark { get; set; }

		[Display(Name = "建立時間")]
		public DateTime? CreateDate { get; set; }

		[Display(Name = "建立人")]
		public string? Creator { get; set; }

		[Display(Name = "修改時間")]
		public DateTime? UpdateDate { get; set; }

		[Display(Name = "修改人")]
		public string? Updater { get; set; }

		[Display(Name = "啟用")]
		public string? Enabled { get; set; }

		[NotMapped] //不想實體類中的某個或者某些屬性，不要映射成數據庫中的列的时候使用
		[Display(Name = "圖片上傳")]
		public IFormFile? File { get; set; } //用來接收上傳的檔案, 加?代表檔案可以為空
	}

}
