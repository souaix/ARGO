using Core.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure.Data.Factories;

public class RepositoryFactory : IRepositoryFactory
{
	private readonly string _cimRisProdConnectionString;
	private readonly string _cimRisTestConnectionString;


	public RepositoryFactory(
		string cimRisProdConnectionString,
		string cimRisTestConnectionString
		)
	{
		_cimRisProdConnectionString = cimRisProdConnectionString;
		_cimRisTestConnectionString = cimRisTestConnectionString;

	}

	public IRepository CreateRepository(string environment)
	{
		return environment switch
		{
			"cimRisProd" => new OracleRepository(_cimRisProdConnectionString),
			"cimRisTest" => new OracleRepository(_cimRisTestConnectionString),
			_ => throw new ArgumentException($"Invalid environment: {environment}")
		};
	}
}