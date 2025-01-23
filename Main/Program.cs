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
// �]�w�s�u�r��
var LtCimLtEdcProd = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.151.37)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ltedc)));User Id=ltcim;Password=cimlt2401;";
var LtCimLtEdcTest = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.151.37)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ltedc)));User Id=ltcimtest;Password=testcimlt0318;";
var cimRisProdConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.1.52)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=RIS)));User Id=programmer;Password=theltmanager;";
var cimRisTestConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.1.32)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=RIS)));User Id=programmer;Password=theltmanager;";

// ���U DbContext
builder.Services.AddDbContext<LtCimLtEdcProdDbContext>(options =>
	options.UseOracle(LtCimLtEdcProd));
builder.Services.AddDbContext<LtCimLtEdcTestDbContext>(options =>
	options.UseOracle(LtCimLtEdcTest).LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Trace));

// ���U Factory
builder.Services.AddSingleton<IRepositoryFactory>(
	new RepositoryFactory(cimRisProdConnectionString, cimRisTestConnectionString));


// ���U�R�W�A�Ȥ��
builder.Services.AddSingleton<IEnumerable<KeyValuePair<string, object>>>(sp =>
{
	return new List<KeyValuePair<string, object>>
	{
		new KeyValuePair<string, object>("Prod", new OracleRepository(cimRisProdConnectionString)),
		new KeyValuePair<string, object>("Test", new OracleRepository(cimRisTestConnectionString))
	};
});
// 1) �Q�� Scrutor �۰ʱ��y�P�j�w&& type != typeof(DapperRepositoryFactory)
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
		// �b�o�̱ư� Factory + �|�ݭn��ʶ� string �� Repository
		.Where(type =>
			type != typeof(RepositoryFactory) &&
			type != typeof(OracleRepository) &&
			type != typeof(MySqlRepository)
		)
	)
	.AsImplementedInterfaces()
	.WithScopedLifetime()
);


// 3) ���U Factory�A������i�z�L environment="Normal"/"Test" �ʺA���� DbContext
builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();

// 4) ���U�z���ʺA Repository (�Y���h�� Repository Interface �]�i�ۦ�վ�)
builder.Services.AddScoped(typeof(ILtCimLtEdcRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IUserManageSysService, UserManageSysService>();

// 5) �Y�n�b Repository �̳z�L Session �� Claims ���o�ثe�ϥΪ̩ҿ諸���ҡA�ݭn HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// 6) �p�G����ϥ� Session ���x�s�ϥΪ����ҡA�h�������[�W Session Middleware
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	// �̻ݨD�]�w�A�Ҧp Session Timeout 
	options.IdleTimeout = TimeSpan.FromMinutes(20);
});

// 7) MVC / Razor Pages
//builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
//// 8) Cookie ���ҡG�ϥΪ̵n�J��i�� Environment �b Cookie (�� Session)
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
//�s�W�R�A�ɮצ۩w�q���|

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





// �s�W�R�A�ɮ׹��� wwwroot/lib �����|

app.MapControllerRoute(
	   name: "default",
	   pattern: "{controller=Main}/{action=Index}/{id?}");



app.Run();
