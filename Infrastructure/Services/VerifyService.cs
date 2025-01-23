using Core.Entities.Main;
using Core.Interfaces;

namespace Infrastructure.Services
{
	public class VerifyService : IVerifyService
	{
		private readonly IVerifyRepository _verifyRepository;

		public VerifyService(IVerifyRepository verifyRepository)
		{
			_verifyRepository = verifyRepository;
		}

		public bool PasswordVerify(VLogin vLogin)
		{
			if (vLogin == null || string.IsNullOrEmpty(vLogin.UserNo) || string.IsNullOrEmpty(vLogin.Password))
				throw new ArgumentException("Invalid login credentials.");

			const string ldapService = "LDAP://LTDC.theil.com"; // LDAP 地址

			return _verifyRepository.ValidateLdapUser(ldapService, vLogin.UserNo, vLogin.Password);
		}

		public string GetRole(string userNo)
		{
			var userRole = _verifyRepository.GetUserRole(userNo);

			if (string.IsNullOrEmpty(userRole))
				return string.Empty;

			// 模擬角色和功能列表組合
			//var roleDetails = userRole.Split(",").Select(role => $"Role_{role}"); // 假設功能代碼加前綴
			//return string.Join(",", roleDetails);

			return userRole;
		}
	}
}