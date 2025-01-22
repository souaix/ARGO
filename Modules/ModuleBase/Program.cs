using Microsoft.AspNetCore.Authentication;
using Modules.ModuleBase.Services;
using Core.Interfaces;
using Infrastructure.Data.Factories;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// 加入 MVC 支援
builder.Services.AddControllersWithViews();

// 模擬使用者驗證（獨立運行時，使用假的身份驗證）
builder.Services.AddAuthentication("FakeScheme")
	.AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("FakeScheme", options => { });

// Oracle for EF
var LtCimLtEdcProd = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.151.37)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ltedc)));User Id=ltcim;Password=cimlt2401;";
var LtCimLtEdcTest = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.151.37)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ltedc)));User Id=ltcimtest;Password=testcimlt0318;";

//註冊兩個不同的 DbContext(正式 / 測試)
builder.Services.AddDbContext<LtCimLtEdcProdDbContext>(options =>
{
	options.UseOracle(LtCimLtEdcProd);
	// 實務上換成正式庫連線字串
});

builder.Services.AddDbContext<LtCimLtEdcTestDbContext>(options =>
{
	options.UseOracle(LtCimLtEdcTest)
		   .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Trace);
	// 實務上換成測試庫連線字串
});


// Oracle for Dapper
var cimRisProdConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.1.52)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=RIS)));User Id=programmer;Password=theltmanager;";
var cimRisTestConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.1.32)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=RIS)));User Id=programmer;Password=theltmanager;";

//// 註冊 Dapper Factory
//builder.Services.AddSingleton<IDapperRepositoryFactory>(
//	new DapperRepositoryFactory(cimRisProdConnectionString, cimRisTestConnectionString));


// 註冊 HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// 使用 Singleton 註冊 DbContextFactory，並注入 IServiceScopeFactory
builder.Services.AddSingleton<IDbContextFactory>(serviceProvider =>
{
	var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
	var configuration = serviceProvider.GetRequiredService<IConfiguration>();
	var environment = configuration["Environment"] ?? "envProduction";
	var dbContextFactory = new DbContextFactory(serviceScopeFactory)
	{
		DefaultEnvironment = environment
	};
	Console.WriteLine($"Default Environment: {environment}");
	return dbContextFactory;
});

// 註冊 ModuleAService
builder.Services.AddScoped<IModuleBaseService, ModuleBaseService>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles();
//新增靜態檔案自定義路徑

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "../../Shared/wwwroot")),
	RequestPath = "/shared"
});




app.MapControllerRoute(
	name: "default",
	pattern: "{controller=ModuleBase}/{action=Index}/{id?}");

app.Run();
