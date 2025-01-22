using Core.Entities;
using Core.Entities.Main;
namespace Core.Interfaces
{
    public interface IVerifyRepository
    {
        bool ValidateLdapUser(string ldapService, string userName, string password);
        string GetUserRole(string userNo);
    }
}