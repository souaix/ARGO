using Core.Interfaces;
using System.DirectoryServices;
using System.Net;
using System.Security.Principal;
using System.Linq;
using Infrastructure.Data.DbContexts;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Core.Entities.LtCimLtEdc;



namespace Infrastructure.Data.Repositories
{
	public class VerifyRepository : IVerifyRepository
	{
		private readonly IDbContextFactory _dbContextFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;

		// 建構子改成注入 "DbContextFactory" 與 "HttpContextAccessor"
		public VerifyRepository(
			IDbContextFactory dbContextFactory,
			IHttpContextAccessor httpContextAccessor)
		{
			_dbContextFactory = dbContextFactory;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// 依照使用者 Session / Claims 取出當下要用哪個 DbContext (Test/Normal)，
		/// 預設環境 = "Normal" (正式區)
		/// </summary>
		private DbContext CurrentDbContext
		{
			get
			{
				// 若您用 Session，則： 
				//string environment = _httpContextAccessor.HttpContext?.Session?.GetString("DbEnvironment")
				//					 ?? "Normal";

				//若您用 Claims(Environment= Test / Normal)，可改用:
				string environment = _httpContextAccessor.HttpContext?.User
											 ?.FindFirst("Environment")?.Value
									  ?? "envProduction";

				return _dbContextFactory.GetDbContext(environment);
			}
		}


		public bool ValidateLdapUser(string ldapService, string userName, string password)
		{
			try
			{

				using (var entry = new System.DirectoryServices.DirectoryEntry(ldapService, userName, password))
				{
					var nativeObj = entry.NativeObject;
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception: " + ex.Message);
				return false;
			}
		}

		public string GetUserRole(string userNo)
		{
			// 1) 從動態取得的 CurrentDbContext 來存取資料
			string role = "";

			var userRole = CurrentDbContext
				.Set<ArgoCimCimUserrole>()  // 注意: 您的 Entity 類型 (Test/Prod) 要一致
				.Where(u => u.Userno == userNo)
				.Select(u => u.Userrole)
				.FirstOrDefault();

			var menuLists = CurrentDbContext.Set<ArgoCimCimSystemmenulist>().OrderBy(m => m.Sequence).ToList();
			var userRolesDetails = CurrentDbContext.Set<ArgoCimCimUserroledetail>().ToList();

			if (userRole.Length > 0 && userRole[0] != null)
			{
				var roleDetails = from x in userRole.Split(",") /* 將角色字串以','分割為陣列*/
								  join b in userRolesDetails on x equals b.Roleno  /*角色綁定功能列表*/
								  join c in menuLists on b.Level03no equals c.Level03no  /*選單列表*/
								  where c.Enabled == "Y"  /*功能必須為啟用*/
								  orderby (c.Sequence)
								  select new
								  {
									  c.Level03no,
								  };

				if (roleDetails != null)
				{
					role = string.Join(",", roleDetails);
					role = role.Replace("{", "");
					role = role.Replace("}", "");
					role = role.Replace("Level03no = ", "");
					role = role.Replace(" ", "");
				}
			}

			return role ?? string.Empty;

			//return userRole ?? string.Empty;


		}
	}
}