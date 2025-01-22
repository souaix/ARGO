using Core.Interfaces;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Factories;

public class DbContextFactory : IDbContextFactory
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	public string DefaultEnvironment { get; set; } = "envProduction";

	// 修改構造函數，接收 IServiceScopeFactory
	public DbContextFactory(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}


	public DbContext GetDbContext(string environment)
	{
		// 檢查 environment 是否為 null 或空字串，若是則使用 DefaultEnvironment
		if (string.IsNullOrWhiteSpace(environment))
		{
			environment = DefaultEnvironment;
		}

		// 建立作用域，確保 DbContext 在 Scoped 範圍內
		var scope = _serviceScopeFactory.CreateScope();

		DbContext dbContext = environment switch
		{
			"envProduction" => scope.ServiceProvider.GetRequiredService<LtCimLtEdcProdDbContext>(),
			"envTest" => scope.ServiceProvider.GetRequiredService<LtCimLtEdcTestDbContext>(),
			_ => throw new Exception($"Unsupported environment: {environment}")
		};

		Console.WriteLine($"Returning DbContext: {dbContext.GetType().Name}");
		return dbContext;
	}

}