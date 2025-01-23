using Core.Entities.LtCimLtEdc;
using Core.Interfaces;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace UserManageSys.Services
{
    public class UserManageSysService : IUserManageSysService
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _environment;

        public UserManageSysService(IDbContextFactory dbContextFactory, IHttpContextAccessor httpContextAccessor)
        {
            _dbContextFactory = dbContextFactory;
            _httpContextAccessor = httpContextAccessor;
            // 從 HttpContext 取得環境變數，例如：envProduction 或 envTest
            _environment = _httpContextAccessor.HttpContext?.Request.Headers["Environment"].ToString() ?? "envProduction";
        }

        public string UserData()
        {

            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                var UserDatas = dbContext.Set<ArgoCimCimUserbasis>().ToList();
                var UserData = UserDatas.Select(g => new
                {
                    g.Userno,
                    g.Username,
                    g.Dndesc,
                    g.Email,
                    g.Hirestatus,
                    g.Comeday,
                    g.Leaveday

                }).Where(m => m.Hirestatus == "3-啟用中").OrderBy(m => m.Userno).ToList();

                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd";
                var UserDataReturn = JsonConvert.SerializeObject(UserData, Newtonsoft.Json.Formatting.Indented, timeFormat);

                return UserDataReturn;
            }
        }

        string IUserManageSysService.UserRoleBasis()
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                var UserRoleBasiss = dbContext.Set<ArgoCimCimUserrolebasis>().ToList();
                var UserRoleBasis = UserRoleBasiss.Select(g => new
                {
                    g.Roleno,
                    g.Rolename,
                    g.Roletype,
                    Roletypetxt = (g.Roletype.ToString() == "0" ? "0-部門預設" : (g.Roletype.ToString() == "1" ? "1-可申請" : (g.Roletype.ToString() == "9" ? "9-部門預設，不開放查詢" : g.Roletype.ToString()))),
                    g.Createdate,
                    g.Creator,
                    g.Updatedate,
                    g.Updater

                }).OrderBy(m => m.Roleno).ToList();

                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm";
                var UserRoleBasisReturn = JsonConvert.SerializeObject(UserRoleBasis, Newtonsoft.Json.Formatting.Indented, timeFormat);

                return UserRoleBasisReturn;
            }
        }

        string IUserManageSysService.UserRole(ArgoCimCimUserbasis user)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                var UserRoles = dbContext.Set<ArgoCimCimUserrole>().ToList();
                var UserRole = UserRoles.Where(m => m.Userno == user.Userno).Select(m => m.Userrole).FirstOrDefault();

                return UserRole;
            }
        }

        string IUserManageSysService.SysMenu()
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                var SysMenus = dbContext.Set<ArgoCimCimSystemmenulist>().ToList();
                var SysrMenu = SysMenus.OrderBy(m => m.Level03no).ToList();

                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm";
                var SysrMenuReturn = JsonConvert.SerializeObject(SysrMenu, Newtonsoft.Json.Formatting.Indented, timeFormat);

                return SysrMenuReturn;
            }
        }

        string[] IUserManageSysService.UserRoleDetailMenu(ArgoCimCimUserroledetail detail)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                var UserRoleDetails = dbContext.Set<ArgoCimCimUserroledetail>().ToList();
                var UserRoleDetail = UserRoleDetails.Where(m => m.Roleno == detail.Roleno).Select(m => m.Level03no).ToArray();

                return UserRoleDetail; //(string[])
            }
        }

        string[] IUserManageSysService.UserRoleDetailRole(ArgoCimCimUserroledetail detail)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                var UserRoleDetails = dbContext.Set<ArgoCimCimUserroledetail>().ToList();
                var UserRoleDetail = UserRoleDetails.Where(m => m.Level03no == detail.Level03no).Select(m => m.Roleno).ToArray();

                return (string[])UserRoleDetail;
            }
        }

        bool IUserManageSysService.UserRoleAdd(ArgoCimCimUserrolebasis dataAdd)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                try
                {
                    dataAdd.Createdate = DateTime.Now;

                    dbContext.Set<ArgoCimCimUserrolebasis>().Add(dataAdd);
                    dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        bool IUserManageSysService.UserRoleUpdate(ArgoCimCimUserrolebasis dataEdit)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                try
                {
                    var dataUpdate = dbContext.Set<ArgoCimCimUserrolebasis>().FirstOrDefault(m => m.Roleno == dataEdit.Roleno);

                    if (dataUpdate != null)
                    {
                        //需異動的欄位如下, SaveChanges()只會判斷有異動的欄位然後更新
                        dataUpdate.Rolename = dataEdit.Rolename;
                        dataUpdate.Roletype = dataEdit.Roletype;
                        dataUpdate.Updatedate = DateTime.Now;
                        dataUpdate.Updater = dataEdit.Updater;

                        dbContext.SaveChanges();

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        bool IUserManageSysService.UserRoleDel(Dictionary<string, string> role)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                try
                {
                    //刪除角色
                    var dataRoles = dbContext.Set<ArgoCimCimUserrolebasis>().FirstOrDefault(m => m.Roleno == role["id"]);
                    if (dataRoles != null)
                    {
                        dbContext.Set<ArgoCimCimUserrolebasis>().Remove(dataRoles);
                    }

                    //刪除角色綁定之選單
                    var dataDetails = dbContext.Set<ArgoCimCimUserroledetail>().Where(m => m.Roleno == role["id"]);
                    if (dataDetails != null)
                    {
                        dbContext.Set<ArgoCimCimUserroledetail>().RemoveRange(dataDetails);
                    }

                    //將使用者綁定之角色取代為空白
                    //先取得實體
                    var dataUserBind = dbContext.Set<ArgoCimCimUserrole>().Where(m => m.Userrole.Contains(role["id"]));

                    //修改實體資料,SaveChanges()會偵測異動的部分再執行SQL UPDATE動作
                    foreach (var row in dataUserBind)
                    {
                        row.Userrole = row.Userrole.Replace(role["id"] + ",", "");
                        row.Userrole = row.Userrole.Replace("," + role["id"], "");
                        row.Updatedate = DateTime.Now;
                        row.Updater = role["updator"];
                    }

                    dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        bool IUserManageSysService.MenuAdd(ArgoCimCimSystemmenulist formData)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                try
                {
                    var dataNew = dbContext.Set<ArgoCimCimSystemmenulist>().FirstOrDefault(m => m.Level03no == formData.Level03no);
                    if (dataNew != null)
                    {
                        return false;  //LEVEL03NO 重複
                    }
                    else
                    {
                        //檔案處理
                        if (formData.File != null && formData.File.Length > 0)
                        {
                            //指定新檔案名稱+上傳之副檔名
                            var newFileName = $"{formData.Level03no}{Path.GetExtension(formData.File.FileName)}";
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "../Modules/UserManageSys/wwwroot/images/UserManageSys", newFileName);
                            //創建路徑
                            using (var stream = System.IO.File.Create(path))
                            {
                                /*await*/
                                formData.File.CopyToAsync(stream); //將檔案丟入路徑
                            }

                            formData.Imgname = newFileName;
                        }

                        //其他資料處理
                        formData.Createdate = DateTime.Now;

                        dbContext.Set<ArgoCimCimSystemmenulist>().Add(formData);
                        dbContext.SaveChanges();

                        return true;
                    }


                }
                catch
                {
                    return false;
                }
            }
        }

        bool IUserManageSysService.MenuUpdate(ArgoCimCimSystemmenulist formData)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                try
                {
                    var dataUpdate = dbContext.Set<ArgoCimCimSystemmenulist>().FirstOrDefault(m => m.Level03no == formData.Level03no);

                    if (dataUpdate != null)
                    {
                        //檔案處理
                        if (formData.File != null && formData.File.Length > 0)
                        {
                            //指定新檔案名稱+上傳之副檔名
                            var newFileName = $"{formData.Level03no}{Path.GetExtension(formData.File.FileName)}";
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "../Modules/UserManageSys/wwwroot/images/UserManageSys", newFileName);
							System.Console.WriteLine(path);
							//創建路徑
							using (var stream = System.IO.File.Create(path))
                            {
                                /*await*/
                                formData.File.CopyToAsync(stream); //將檔案丟入路徑
                            }

                            dataUpdate.Imgname = newFileName;
                        }

                        //需異動的欄位如下, SaveChanges()只會判斷有異動的欄位然後更新
                        dataUpdate.Belongdept = formData.Belongdept;
                        dataUpdate.Level03 = formData.Level03;
                        dataUpdate.Icon = formData.Icon;
                        dataUpdate.Controller = formData.Controller;
                        dataUpdate.Action = formData.Action;
                        dataUpdate.Sequence = formData.Sequence;
                        dataUpdate.Keyword = formData.Keyword;
                        dataUpdate.Enabled = formData.Enabled;
                        dataUpdate.Remark = formData.Remark;
                        dataUpdate.Updatedate = DateTime.Now;
                        dataUpdate.Updater = formData.Updater;

                        dbContext.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }
        bool IUserManageSysService.SysMenuEnabled(Dictionary<string, string> level3)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                try
                {
                    //變更狀態
                    var sysMenus = dbContext.Set<ArgoCimCimSystemmenulist>().FirstOrDefault(m => m.Level03no == level3["id"]);
                    if (sysMenus != null)
                    {
                        sysMenus.Enabled = sysMenus.Enabled == "Y" ? "N" : "Y";
                        sysMenus.Updatedate = DateTime.Now;
                        sysMenus.Updater = level3["updator"];
                    }
                    dbContext.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        bool IUserManageSysService.UserRoleBind(ArgoCimCimUserrole dataBind)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                try
                {
                    var dataUpdate = dbContext.Set<ArgoCimCimUserrole>().FirstOrDefault(m => m.Userno == dataBind.Userno);

                    if (dataUpdate != null)
                    {
                        //需異動的欄位如下, SaveChanges()只會判斷有異動的欄位然後更新
                        dataUpdate.Userrole = dataBind.Userrole;
                        dataUpdate.Updatedate = DateTime.Now;
                        dataUpdate.Updater = dataBind.Updater;
                    }
                    else
                    {
                        //若無該USERNO則新建一筆資料
                        dataBind.Createdate = DateTime.Now;
                        dataBind.Updatedate = null;
                        dataBind.Updater = null;
                        dbContext.Set<ArgoCimCimUserrole>().Add(dataBind);
                    }

                    dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        bool IUserManageSysService.UserRoleDetail(ArgoCimCimUserroledetail dataBind)
        {
            using (var dbContext = _dbContextFactory.GetDbContext(_environment))
            {
                try
                {
                    var dataUpdate = dbContext.Set<ArgoCimCimUserroledetail>().FirstOrDefault(m => m.Roleno == dataBind.Roleno && m.Level03no == dataBind.Level03no);

                    if (dataUpdate != null)
                    {
                        //若有符合的資料代表需刪除
                        dbContext.Set<ArgoCimCimUserroledetail>().Remove(dataUpdate);
                    }
                    else
                    {
                        //若無則新建一筆資料
                        dataBind.Createdate = DateTime.Now;
                        dataBind.Updatedate = null;
                        dataBind.Updater = null;
                        dbContext.Set<ArgoCimCimUserroledetail>().Add(dataBind);
                    }

                    dbContext.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
    }
}
