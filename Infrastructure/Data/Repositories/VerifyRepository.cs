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
			var userRole = CurrentDbContext
				.Set<ArgoCimCimUserrole>()  // 注意: 您的 Entity 類型 (Test/Prod) 要一致
				.Where(u => u.Userno == userNo)
				.Select(u => u.Userrole)
				.FirstOrDefault();

			return userRole ?? string.Empty;


		}
	}
}