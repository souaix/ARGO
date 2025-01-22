using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Data.Repositories
{
	public class EfRepository<T> : ILtCimLtEdcRepository<T> where T : class
	{
		private readonly IDbContextFactory _dbContextFactory;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public EfRepository(
			IDbContextFactory dbContextFactory,
			IHttpContextAccessor httpContextAccessor)
		{
			_dbContextFactory = dbContextFactory;
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// 每次取用都會依照使用者的 Environment 動態取得不同 DbContext
		/// </summary>
		private DbContext CurrentDbContext
		{
			get
			{
				// 1) 如果是用 Session
				// string environment = _httpContextAccessor.HttpContext?.Session?.GetString("DbEnvironment") ?? "Normal";

				// 2) 如果是用 Claims
				// 下面只是範例，要自行判斷 null / 例外處理
				string environment = _httpContextAccessor.HttpContext?.User?
											.FindFirst("Environment")?.Value
											?? "envProduction";  // 預設用正式環境

				return _dbContextFactory.GetDbContext(environment);
			}
		}

		public async Task<T?> GetByIdAsync(int id)
		{
			return await CurrentDbContext.Set<T>().FindAsync(id);
		}

		public async Task<IEnumerable<T>> ListAsync()
		{
			return await CurrentDbContext.Set<T>().ToListAsync();
		}

		public async Task<IEnumerable<T>> ListAllAsync()
		{
			return await CurrentDbContext.Set<T>().ToListAsync();
		}

		public async Task<T> AddAsync(T entity)
		{
			CurrentDbContext.Set<T>().Add(entity);
			await CurrentDbContext.SaveChangesAsync();
			return entity;
		}

		public async Task UpdateAsync(T entity)
		{
			CurrentDbContext.Set<T>().Update(entity);
			await CurrentDbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			CurrentDbContext.Set<T>().Remove(entity);
			await CurrentDbContext.SaveChangesAsync();
		}
	}
}
