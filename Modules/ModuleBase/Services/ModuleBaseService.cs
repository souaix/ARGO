using System.Collections.Generic;
using System.Linq;
using Core.Entities.LtCimLtEdc;
using Microsoft.AspNetCore.Http;
using Core.Entities;
using Core.Interfaces;

namespace Modules.ModuleBase.Services
{
	public class ModuleBaseService : IModuleBaseService
	{
		private readonly IDbContextFactory _dbContextFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly string _environment;

		public ModuleBaseService(IDbContextFactory dbContextFactory, IHttpContextAccessor httpContextAccessor)
		{
			_dbContextFactory = dbContextFactory;
			_httpContextAccessor = httpContextAccessor;
			// 從 HttpContext 取得環境變數，例如：envProduction 或 envTest
			_environment = _httpContextAccessor.HttpContext?.Request.Headers["Environment"].ToString() ?? "envProduction";
		}

		public List<ArgoCimCimSystemmenulist> GetData()
		{


			using (var dbContext = _dbContextFactory.GetDbContext(_environment))
			{				
				return dbContext.Set<ArgoCimCimSystemmenulist>().ToList();
			}
		}
	}
}
