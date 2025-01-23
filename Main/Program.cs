using Microsoft.EntityFrameworkCore;

using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.FileProviders;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Core.Interfaces;
using Infrastructure.Data.DbContexts;
using Infrastructure.Data.Factories;
using Infrastructure.Data.Repositories;
using System;
using Microsoft.Extensions.Options;
using Infrastructure.Services;
using Microsoft.AspNetCore.Server.IISIntegration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using UserManageSys.Services;




var builder = WebApplication.CreateBuilder(args);
// 設定連線字串
var LtCimLtEdcProd = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.151.37)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ltedc)));User Id=ltcim;Password=cimlt2401;";
var LtCimLtEdcTest = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.151.37)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ltedc)));User Id=ltcimtest;Password=testcimlt0318;";
var cimRisProdConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.1.52)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=RIS)));User Id=programmer;Password=theltmanager;";
var cimRisTestConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.1.32)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=RIS)));User Id=programmer;Password=theltmanager;";

// 註冊 DbContext
builder.Services.AddDbContext<LtCimLtEdcProdDbContext>(options =>
	options.UseOracle(LtCimLtEdcProd));
builder.Services.AddDbContext<LtCimLtEdcTestDbContext>(options =>
	options.UseOracle(LtCimLtEdcTest).LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Trace));

// 註冊 Factory
builder.Services.AddSingleton<IRepositoryFactory>(
	new RepositoryFactory(cimRisProdConnectionString, cimRisTestConnectionString));


// 註冊命名服務支持
builder.Services.AddSingleton<IEnumerable<KeyValuePair<string, object>>>(sp =>
{
	return new List<KeyValuePair<string, object>>
	{
		new KeyValuePair<string, object>("Prod", new OracleRepository(cimRisProdConnectionString)),
		new KeyValuePair<string, object>("Test", new OracleRepository(cimRisTestConnectionString))
	};
});
// 1) 利用 Scrutor 自動掃描與綁定&& type != typeof(DapperRepositoryFactory)
builder.Services.Scan(scan => scan
	.FromAssemblies(
		Assembly.Load("Infrastructure"),
		Assembly.Load("ModuleBase")
	)
	.AddClasses(classes =>
		classes.InNamespaces(
			"Infrastructure.Services",
			"Infrastructure.Data.Repositories",
			"Modules"
		)
		// 在這裡排除 Factory + 會需要手動傳 string 的 Repository
		.Where(type =>
			type != typeof(RepositoryFactory) &&
			type != typeof(OracleRepository) &&
			type != typeof(MySqlRepository)
		)
	)
	.AsImplementedInterfaces()
	.WithScopedLifetime()
);


// 3) 註冊 Factory，讓後續可透過 environment="Normal"/"Test" 動態取用 DbContext
builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();

// 4) 註冊您的動態 Repository (若有多個 Repository Interface 也可自行調整)
builder.Services.AddScoped(typeof(ILtCimLtEdcRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IUserManageSysService, UserManageSysService>();

// 5) 若要在 Repository 裡透過 Session 或 Claims 取得目前使用者所選的環境，需要 HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// 6) 如果打算使用 Session 來儲存使用者環境，則必須先加上 Session Middleware
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	// 依需求設定，例如 Session Timeout 
	options.IdleTimeout = TimeSpan.FromMinutes(20);
});

// 7) MVC / Razor Pages
//builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
//// 8) Cookie 驗證：使用者登入後可放 Environment 在 Cookie (或 Session)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.Cookie.HttpOnly = true;
		options.LoginPath = new PathString("/Main/Login");
		options.AccessDeniedPath = new PathString("/Main/Login");

	});



builder.Services
	.AddControllersWithViews()
	.ConfigureApplicationPartManager(manager =>
	{
		Console.WriteLine("=== Application Parts ===");
		foreach (var part in manager.ApplicationParts)
		{
			Console.WriteLine($" - {part.Name}");
		}
	});


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles();
//新增靜態檔案自定義路徑

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "../Shared/wwwroot")),
	RequestPath = "/shared"
});

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "../Modules/UserManageSys/wwwroot")),
	RequestPath = "/UserManageSys"
});

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "../Modules/UserManageSys/Scripts")),
	RequestPath = "/userManageSysScripts"
});





// 新增靜態檔案對應 wwwroot/lib 的路徑

app.MapControllerRoute(
	   name: "default",
	   pattern: "{controller=Main}/{action=Index}/{id?}");



app.Run();
