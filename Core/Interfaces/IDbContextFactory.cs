using Microsoft.EntityFrameworkCore;
namespace Core.Interfaces
{
	public interface IDbContextFactory
	{
		DbContext GetDbContext(string environment);
	}
}
