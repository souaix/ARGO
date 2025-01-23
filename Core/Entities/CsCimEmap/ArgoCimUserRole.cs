namespace Core.Entities.CsCimEmap
{
	public class ArgoCimUserRole
	{
		public string UserNo { get; set; } = null!;

		public string? UserRole { get; set; }

		public DateTime? CreateDate { get; set; }

		public string? Creator { get; set; }

		public DateTime? UpdateDate { get; set; }

		public string? Updater { get; set; }
	}
}
