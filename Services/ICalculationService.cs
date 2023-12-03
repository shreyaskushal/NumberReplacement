using NumberReplacementApi.Models;

namespace NumberReplacementApi.Services
{
	public interface ICalculationService
	{
		Task<Guid> StartCalculation(string firstName, string lastName);
		StatusObject GetStatus(Guid processId);
	}
}
