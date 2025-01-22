using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Entities.LtCimLtEdc;
using System.Collections.Generic;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Main.Components
{

    public class UserMenuViewComponent : ViewComponent
    {
        public List<ArgoCimCimSystemmenulist> MenuLists = new List<ArgoCimCimSystemmenulist>();
        public List<ArgoCimCimUserrole> UserRoles = new List<ArgoCimCimUserrole>();
        public List<ArgoCimCimUserroledetail> UserRolesDetails = new List<ArgoCimCimUserroledetail>();

		// 改為注入 DbContextFactory + HttpContextAccessor
		private readonly IDbContextFactory _dbContextFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UserMenuViewComponent(
			IDbContextFactory dbContextFactory,
			IHttpContextAccessor httpContextAccessor)
		{
			_dbContextFactory = dbContextFactory;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// 動態取得當前環境 (Normal / Test)，並返回對應的 DbContext
		/// </summary>
		private DbContext CurrentDbContext
		{
			get
			{
				// 如果用 Claims
				string environment = _httpContextAccessor.HttpContext?.User
					?.FindFirst("Environment")?.Value ?? "envProduction";

				// 如果用 Session，請改成下面這行：
				// string environment = _httpContextAccessor.HttpContext?.Session?.GetString("DbEnvironment") ?? "Normal";
				Console.WriteLine($"Environment: {environment}");
				return _dbContextFactory.GetDbContext(environment);
			}
		}

		public IViewComponentResult Invoke(string id) 
        {

            try
            {
                //取得表單資料
                var class1_item = HttpContext.Request.Form["class1_item"];
                var class2_item = HttpContext.Request.Form["class2_item"];

                ViewBag.class1_item = class1_item;
                ViewBag.class2_item = class2_item;
            }
            catch
            {
                //若無預設空白
                ViewBag.class1_item = "";
                ViewBag.class2_item = "";
            }

            var UserId = id;


			MenuLists = CurrentDbContext?.Set<ArgoCimCimSystemmenulist>()
							   ?.OrderBy(m => m.Sequence)
							   ?.ToList();

			UserRoles = CurrentDbContext.Set<ArgoCimCimUserrole>()
				.Where(m => m.Userno == UserId)
				.ToList();

			UserRolesDetails = CurrentDbContext.Set<ArgoCimCimUserroledetail>()
				.ToList();

			var UserRole = UserRoles.Select(x => x.Userrole).ToArray();

            if (UserRole.Length > 0 && UserRole[0] != null)
            {
                var RoleStr = UserRole[0]; /*取得登入者角色*/
                var Menu = from x in RoleStr.Split(",") /* 將角色字串以','分割為陣列*/
                           join b in UserRolesDetails on x equals b.Roleno  /*角色綁定功能列表*/
                           join c in MenuLists on b.Level03no equals c.Level03no  /*選單列表*/
						   where c.Enabled == "Y"  /*功能必須為啟用*/
                           orderby(c.Sequence)
                           select new
                           {
                               LEVEL01NO = c.Level01no,
                               LEVEL02NO = c.Level02no,
                               LEVEL03NO = c.Level03no,
                               LEVEL01 = c.Level01,
                               LEVEL02 = c.Level02,
                               LEVEL03 = c.Level03,
                               ACTION = c.Action,
                               CONTROLLER = c.Controller,
                               ICON = c.Icon,
                           };
                var UserMenu = Menu.ToArray();
                ViewBag.UserMenu = UserMenu;
            }
            else
            {
             
                ViewBag.UserMenu = "";
            }

            return View();
        }
    }
}
