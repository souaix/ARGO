using System.ComponentModel.DataAnnotations;

namespace Core.Entities.CsCimEmap
{
	public class ArgoCimUserBasis
	{
		[Display(Name = "工號")]
		public string UserNo { get; set; } = null!;

		[Display(Name = "姓名")]
		public string? UserName { get; set; }

		[Display(Name = "部門")]
		public string? DnDesc { get; set; }

		[Display(Name = "E-MAIL")]
		public string? Email { get; set; }

		/// <summary>
		/// 0-已離職,1-未啟用(留職停薪),2-退休人員,3-啟用中
		/// </summary>
		[Display(Name = "雇用狀態")]
		public string? HireStatus { get; set; }

		[Display(Name = "到職日")]
		public DateTime? ComeDay { get; set; }

		[Display(Name = "離職日")]
		public DateTime? LeaveDay { get; set; }

		/// <summary>
		/// Y/N
		/// </summary>
		[Display(Name = "是否在職")]
		public string? NowInJob { get; set; }

		[Display(Name = "部門代碼")]
		public string? DnNo { get; set; }

		[Display(Name = "生日")]
		public string? Birthday { get; set; }

		[Display(Name = "外部信箱")]

		public string? EmailOutSide { get; set; }
	}
}
