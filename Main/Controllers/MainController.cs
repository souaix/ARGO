using Core.Entities.Main;// 使用實體類
using Core.Interfaces; // 使用業務邏輯介面
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Core.Entities.LtCimLtEdc;
using Infrastructure.Data.DbContexts;
using System.Security.Principal;

namespace Main.Controllers
{
	
	public class MainController : Controller
	{
		private readonly IVerifyService _verifyService; // 使用業務服務
		private readonly ILogger<MainController> _logger;
		private readonly IDbContextFactory _dbContextFactory;
		

		public MainController(
			IVerifyService verifyService,
			ILogger<MainController> logger,     		
			IDbContextFactory dbContextFactory

			)
		{
			_verifyService = verifyService;
			_logger = logger;			
			_dbContextFactory = dbContextFactory;
		}


		[AllowAnonymous]
		public IActionResult TestLayout()
		{
			return View("TestFromRcl");
		}


		[AllowAnonymous]
		public IActionResult Index()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Main");
			}
			
			return View();
		}


		[AllowAnonymous]
		public IActionResult Login()
		{
			try
			{
				// 取得目前的Windows使用者名稱
				string userName = WindowsIdentity.GetCurrent().Name;
				// 使用LDAP驗證取得密碼（假設您有LDAP服務器設定）
				string password = ""; // 可根據需求從LDAP提取密碼或預設值
				userName = userName.Split('\\')[1];
				// 將取得的使用者名稱與密碼傳遞到View
				ViewBag.UserNo = userName;
				ViewBag.Password = password;

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "取得AD帳號失敗");
				ViewBag.UserNo = string.Empty;
				ViewBag.Password = string.Empty;
			}
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(VLogin vLogin)
		{
			Console.WriteLine("DEF");
			TempData["alert"] = string.Empty;

			try
			{
				if (vLogin == null)
				{
					return BadRequest();
				}

				// 呼叫服務驗證密碼
				var isValidUser = _verifyService.PasswordVerify(vLogin);

				//Console.WriteLine("isValidUser: " + isValidUser);
				if (isValidUser)
				{
					Console.WriteLine(vLogin.Environment);

					// 取得使用者角色
					var role = _verifyService.GetRole(vLogin.UserNo);
					var roles = role.Split(',');

					// 建立身份宣告
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, vLogin.UserNo),
						new Claim("Environment", vLogin.Environment)
					};
					//多角色指派
					foreach (var r in roles)
					{
						claims.Add(new Claim(ClaimTypes.Role, r));
					}

					// 建立身份驗證資訊
					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var authProperties = new AuthenticationProperties { IsPersistent = true };

					// 登入使用者
					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity),
						authProperties);
					
					return RedirectToAction("Index");
				}

				TempData["alert"] = "您輸入的帳號或密碼不正確。";
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "登入失敗");
				TempData["alert"] = "登入過程中發生錯誤，請稍後再試。";
			}

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			// 清除驗證 Cookie
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

		
			return RedirectToAction("Index", "Main");
		}

	}

	public static class LoggingEvents
	{
		public const int CreateAction = 1000;
		public const int EditAction = 1001;
		public const int GetAction = 1002;
		public const int DeleteAction = 1003;
	}
}
