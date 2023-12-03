using Microsoft.AspNetCore.Mvc;
using NumberReplacementApi.Database;
using NumberReplacementApi.Services;

namespace NumberReplacementApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CalculationController : ControllerBase
	{
		private readonly ICalculationService _calculationService;
		public CalculationController(ICalculationService calculationService)
		{
			_calculationService = calculationService;
		}

		[HttpPost]
		public async Task<ActionResult<Guid>> StartCalculation(string firstName, string lastName)
		{
			try
			{
				return await _calculationService.StartCalculation(firstName, lastName);
			}
			catch (ApplicationException ex)
			{
				return StatusCode(500, "Calculation failed" + ex.Message);
			}
			
		}

		[HttpGet]
		[Route("{processId}")]
		public ActionResult<object> GetStatus(Guid processId)
		{
			try
			{
				return _calculationService.GetStatus(processId);
			}
			catch (Exception ex)
			{
				// Log the exception (consider using a logging framework)
				return StatusCode(500, "An error occurred");
			}
		}
	}
}