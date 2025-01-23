using Microsoft.AspNetCore.Authentication;
using UserManageSys.Services;
using Core.Interfaces;
using Infrastructure.Data.Factories;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// �[�J MVC �䴩
builder.Services.AddControllersWithViews();

// �����ϥΪ����ҡ]�W�߹B��ɡA�ϥΰ����������ҡ^
builder.Services.AddAuthentication("FakeScheme")
    .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>("FakeScheme", options => { });

// Oracle for EF
//var LtCimLtEdcProd = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.151.37)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ltedc)));User Id=ltcim;Password=cimlt2401;";
//var LtCimLtEdcTest = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.21.151.37)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ltedc)));User Id=ltcimtest;Password=testcimlt0318;";

//���U��Ӥ��P�� DbContext(���� / ����)
//builder.Services.AddDbContext<LtCimLtEdcProdDbContext>(options =>
//{
//    options.UseOracle(LtCimLtEdcProd);
//    //  ��ȤW���������w�s�u�r��
//});

//builder.Services.AddDbContext<LtCimLtEdcTestDbContext>(options =>
//{
//    options.UseOracle(LtCimLtEdcTest)
//           .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Trace);
//    // ��ȤW�������ծw�s�u�r��
//});

// Oracle for Dapper
var csCimEmapProdConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.20.120)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=emap)));User Id=cscim;Password=cscim2025adm!;";
var csCimEmapTestConnectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.30.40.133)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=emap)));User Id=cscim;Password=cscim2025adm!;";

//// ���U Dapper Factory
builder.Services.AddSingleton<IRepositoryFactory>(
    new RepositoryFactory(csCimEmapProdConnectionString, csCimEmapTestConnectionString));


// ���U HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// �ϥ� Singleton ���U DbContextFactory�A�ê`�J IServiceScopeFactory
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

// ���U ModuleAService
builder.Services.AddScoped<IUserManageSysService, UserManageSysService>();

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles();
//�s�W�R�A�ɮצ۩w�q���|

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "../../Shared/wwwroot")),
    RequestPath = "/shared"
});

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
	RequestPath = "/UserManageSys"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Scripts")),
    RequestPath = new PathString("/userManageSysScripts")
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserManageSys}/{action=UserV2}/{id?}");

app.Run();
