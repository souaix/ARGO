using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Entities.LtCimLtEdc;
using UserManageSys.Services;

namespace UserManageSys.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserManageSys_APIController : ControllerBase
    {
        private IUserManageSysService _service;

        public UserManageSys_APIController(IUserManageSysService usermanagesys)
        {
            _service = usermanagesys;

        }

        /// <summary>
        /// 取得使用者主檔
        /// </summary>
        [HttpPost]
        [Route("UserData")]
        public IActionResult UserData()
        {
            return Ok(_service.UserData());
        }

        /// <summary>
        /// 取得角色主檔
        /// </summary>
        [HttpPost]
        [Route("UserRoleBasisData")]
        public IActionResult UserRoleBasis()
        {
            return Ok(_service.UserRoleBasis());
        }

        /// <summary>
        /// 取得指定工號角色
        /// </summary>
        [HttpPost]
        [Route("UserRoleData")]
        public IActionResult UserRole(ArgoCimCimUserbasis user)
        {
            return Ok(_service.UserRole(user));
        }

        /// <summary>
        /// 取得選單主檔
        /// </summary>
        [HttpPost]
        [Route("SysMenuData")]
        public IActionResult SysMenu()
        {
            return Ok(_service.SysMenu());
        }

        /// <summary>
        /// 取得角色與選單關聯
        /// </summary>
        [HttpPost]
        [Route("UserRoleDetailMenu")]
        public IActionResult UserRoleDetailMenu(ArgoCimCimUserroledetail detail)
        {
            return Ok(_service.UserRoleDetailMenu(detail));
        }

        /// <summary>
        /// 取得選單與角色關聯
        /// </summary>
        [HttpPost]
        [Route("UserRoleDetailRole")]
        public IActionResult UserRoleDetailRole(ArgoCimCimUserroledetail detail)
        {
            return Ok(_service.UserRoleDetailRole(detail));
        }




        /// <summary>
        /// 新增角色
        /// </summary>
        [HttpPost]
        [Route("ArgoCimCimUserrolebasis")]
        public IActionResult UserRoleAdd([FromForm] ArgoCimCimUserrolebasis dataAdd)
        {

            if (_service.UserRoleAdd(dataAdd) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("角色代碼重複，請重新作業!");
            }
        }

        /// <summary>
        /// 編輯角色
        /// </summary>
        [HttpPost]
        [Route("UserRoleEdit")]
        public IActionResult UserRoleUpdate([FromForm] ArgoCimCimUserrolebasis dataEdit)
        {

            if (_service.UserRoleUpdate(dataEdit) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("更新失敗，請重新作業!");
            }
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        [HttpPost]
        [Route("UserRoleDel")]
        public IActionResult UserRoleDel(Dictionary<string, string> role)
        {
            if (_service.UserRoleDel(role) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("刪除失敗，請重新作業!");
            }
        }

        /// <summary>
        /// 新增選單
        /// </summary>
        [HttpPost]
        [Route("ArgoCimCimSystemmenulist")]
        public async Task<IActionResult> MenuAdd([FromForm] ArgoCimCimSystemmenulist formData)
        {
            if (_service.MenuAdd(formData) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("新增失敗，請重新作業!");
            }
        }

        /// <summary>
        /// 編輯選單
        /// </summary>
        [HttpPost]
        [Route("MenuEdit")]
        public IActionResult MenuUpdate([FromForm] ArgoCimCimSystemmenulist dataEdit)
        {

            if (_service.MenuUpdate(dataEdit) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("更新失敗，請重新作業!");
            }
        }

        /// <summary>
        /// 變更選單狀態(啟用/停用)
        /// </summary>
        [HttpPost]
        [Route("SysMenuEnabled")]
        public IActionResult SysMenuEnabled(Dictionary<string, string> level3)
        {
            if (_service.SysMenuEnabled(level3) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("變更失敗，請重新作業!");
            }
        }

        /// <summary>
        /// 綁定使用者角色
        /// </summary>
        [HttpPost]
        [Route("ArgoCimCimUserrole")]
        public IActionResult UserRoleBind(ArgoCimCimUserrole dataBind)
        {
            if (_service.UserRoleBind(dataBind) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("綁定失敗，請重新作業!");
            }
        }

        /// <summary>
        /// 角色綁定選單/選單綁定角色
        /// </summary>
        [HttpPost]
        [Route("ArgoCimCimUserroledetail")]
        public IActionResult UserRoleDetail(ArgoCimCimUserroledetail dataBind)
        {
            if (_service.UserRoleDetail(dataBind) == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("綁定失敗，請重新作業!");
            }
        }


    }

    public class UserManageSysController : Controller
    {
        [Authorize(Roles = "CIMSYSUSR0004")]
        public IActionResult UserV2()
        {
			var userName = User.Identity.Name;
			return View();
        }

        [Authorize(Roles = "CIMSYSUSR0005")]
        public IActionResult RoleV2()
        {
			var userName = User.Identity.Name;
			return View();
        }

        [Authorize(Roles = "CIMSYSUSR0006")]
        public IActionResult MenuV2()
        {
			var userName = User.Identity.Name;
			return View();
        }
    }
}
