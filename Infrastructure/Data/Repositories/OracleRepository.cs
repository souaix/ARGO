using System.Data;
using System.Data.OracleClient;
using Dapper;
using Core.Interfaces;

namespace Infrastructure.Data.Repositories;
public class OracleRepository : IRepository
{
	private readonly string _connectionString;

	public OracleRepository(string connectionString)
	{
		_connectionString = connectionString;
	}

	private IDbConnection CreateConnection()
	{
		return new OracleConnection(_connectionString);
	}

	public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
	{
		using (var connection = CreateConnection())
		{
			return await connection.QueryAsync<T>(sql, parameters);
		}
	}

	public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
	{
		using (var connection = CreateConnection())
		{
			return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
		}
	}

	public async Task<int> ExecuteAsync(string sql, object? parameters = null)
	{
		using (var connection = CreateConnection())
		{
        // 打印參數
        if (parameters != null)
        {
            Console.WriteLine("Parameters:");
            foreach (var property in parameters.GetType().GetProperties())
            {
                var name = property.Name;
                var value = property.GetValue(parameters);
                Console.WriteLine($"    {name}: {value}");
            }
        }
        else
        {
            Console.WriteLine("Parameters: None");
        }		
			return await connection.ExecuteAsync(sql, parameters);
		}
	}

	// Insert 操作
	public async Task<int> InsertAsync<T>(string sql, T entity)
	{
		using (var connection = CreateConnection())
		{
			// 打印插入的實體資料
			Console.WriteLine("Inserting Entity:");
			foreach (var property in entity.GetType().GetProperties())
			{
				var name = property.Name;
				var value = property.GetValue(entity);
				Console.WriteLine($"    {name}: {value}");
			}

			return await connection.ExecuteAsync(sql, entity);
		}
	}

	// Update 操作
	public async Task<int> UpdateAsync<T>(string sql, T entity)
	{
		using (var connection = CreateConnection())
		{
			// 打印更新的實體資料
			Console.WriteLine("Updating Entity:");
			foreach (var property in entity.GetType().GetProperties())
			{
				var name = property.Name;
				var value = property.GetValue(entity);
				Console.WriteLine($"    {name}: {value}");
			}

			return await connection.ExecuteAsync(sql, entity);
		}
	}

}
