var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapControllerRoute(
	   name: "default",
	   pattern: "{controller=Main}/{action=Index}/{id?}");

app.Run();
