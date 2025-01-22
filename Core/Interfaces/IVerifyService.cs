
using Core.Entities.Main;
namespace Core.Interfaces
{
	public interface IVerifyService
	{
		bool PasswordVerify(VLogin vLogin);
		string GetRole(string userNo);
	}
}