using Core.Entities.UserManageSys;// 使用實體類
using Core.Entities.Main;// 使用實體類
using Core.Interfaces; // 使用業務邏輯介面
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Shared.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class Shared_APIController : ControllerBase
	{
		/// <summary>
		/// 取得DisplayName
		/// </summary>
		[HttpPost]
		[Route("GetDisplayName")]
		public string[] GetDisplayName(ArgoCimCimSystemgetdisplayname data)
		{
			Type typeName;
			var name = "";
			var nameArr = new string[0];
			string strClass = "Core.Entities.LtCimLtEdc." + data.Typename + ",Core";

			//Type.GetType 預設只在目前的程式集, 若類型位於其他程式集, 需要在 Type.GetType 中提供程式集名稱, 如下為 "...,Core"
			//typeName = Type.GetType("Core.Entities.LtCimLtEdc.ArgoCimCimUserbasis,Core");
			//typeName = typeof(Core.Entities.LtCimLtEdc.ArgoCimCimUserbasis);

			typeName = Type.GetType(strClass);

			foreach (var column in data.Columnname)
			{
				MemberInfo property = typeName.GetProperty(column);
				var DsiplayName = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

				if (DsiplayName != null)
				{
					name = DsiplayName.Name;
				}
				else
				{
					name = column;
				}

				nameArr = nameArr.Append(name).ToArray();
			}

			return nameArr;
		}
	}


	public class SharedController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
