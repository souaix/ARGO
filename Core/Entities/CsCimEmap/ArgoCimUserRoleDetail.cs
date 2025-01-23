namespace Core.Entities.CsCimEmap
{
	public class ArgoCimUserRoleDetail
	{
		public string RoleNo { get; set; } = null!;

		public string Level03No { get; set; } = null!;

		public DateTime? CreateDate { get; set; }

		public string? Creator { get; set; }

		public DateTime? UpdateDate { get; set; }

		public string? Updater { get; set; }
	}
}
