using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.ModuleBase.Services;

namespace Modules.ModuleBase.Controllers
{
	[Authorize]
	public class ModuleBaseController : Controller
	{
		private readonly IModuleBaseService _moduleBaseService;

		public ModuleBaseController(IModuleBaseService moduleBaseService)
		{
			_moduleBaseService = moduleBaseService;
		}

		public IActionResult Index()
		{
			var userName = User.Identity.Name;
			var menuLists = _moduleBaseService.GetData();
			return View(menuLists);
			
		}

		[AllowAnonymous]
		public IActionResult PublicPage()
		{
			return View();
		}
	}
}
