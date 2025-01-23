using Core.Entities.LtCimLtEdc;
using Microsoft.AspNetCore.Mvc;

namespace UserManageSys.Services
{
    public interface IUserManageSysService
    {
        string UserData();  //取得使用者主檔 OK
        string UserRoleBasis();  //取得角色主檔 OK
        string UserRole(ArgoCimCimUserbasis user);  //取得指定工號角色 OK
        string SysMenu();  //取得選單主檔 OK
        string[] UserRoleDetailMenu(ArgoCimCimUserroledetail detail); //取得角色與選單關聯 OK
        string[] UserRoleDetailRole(ArgoCimCimUserroledetail detail);  //取得選單與角色關聯 OK
        bool UserRoleAdd([FromForm] ArgoCimCimUserrolebasis dataAdd);  //新增角色 OK
        bool UserRoleUpdate([FromForm] ArgoCimCimUserrolebasis dataEdit);  //編輯角色 OK
        bool UserRoleDel(Dictionary<string, string> role);  //刪除角色 OK
        bool MenuAdd([FromForm] ArgoCimCimSystemmenulist formData);  //新增選單 改為同步處理檔案(註解掉await) OK
        bool MenuUpdate([FromForm] ArgoCimCimSystemmenulist formData);  //編輯選單 同步處理檔案 OK
        bool SysMenuEnabled(Dictionary<string, string> level3);  //變更選單狀態(啟用/停用) OK
        bool UserRoleBind(ArgoCimCimUserrole dataBind);  //綁定使用者角色 OK
        bool UserRoleDetail(ArgoCimCimUserroledetail dataBind);  //角色綁定選單/選單綁定角色 OK
    }
}
