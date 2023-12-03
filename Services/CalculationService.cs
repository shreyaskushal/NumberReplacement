using Microsoft.EntityFrameworkCore;
using NumberReplacementApi.Constants;
using NumberReplacementApi.Database;
using NumberReplacementApi.Models;
using System.Diagnostics;

namespace NumberReplacementApi.Services
{
	public class CalculationService : ICalculationService
	{
		private readonly MyDbContext _dbcontext;

		public CalculationService(MyDbContext dbcontext)
		{
			_dbcontext = dbcontext;
		}

		public async Task<Guid> StartCalculation(string firstName, string lastName)
		{
			Guid processId = Guid.NewGuid();
			try
			{
				var status = GetRandomStatus();

				InsertCalculationResult(processId, firstName, lastName, CalculationStatus.Running, DateTime.Now);

				Random random = new Random();
				int delay = random.Next(10000, 60000);
				await Task.Delay(delay);

				if (status == CalculationStatus.Failed)
				{
					throw new ApplicationException("Calculation failed");
				}

				UpdateCalculationResultStatus(processId, CalculationStatus.Completed);

				return processId;
			}
			catch (Exception ex)
			{
				UpdateCalculationResultStatus(processId, CalculationStatus.Failed);
				throw new ApplicationException("An error occurred during calculation", ex);
			}
		}

		public StatusObject GetStatus(Guid processId)
		{
			var calculationResult = _dbcontext.CalculationResults.SingleOrDefault(c => c.ProcessId == processId);

			if (calculationResult == null)
			{
				return new StatusObject { Status = "NotFound", Progress = 0 };
			}

			StatusObject statusObject = new StatusObject();

			statusObject.Status = calculationResult.Status.ToString().ToLower();
			statusObject.Progress = CalculateProgress(calculationResult);

			return statusObject;
		}

		private int CalculateProgress(CalculationResult calculationResult)
		{
			if (calculationResult.Status == CalculationStatus.Running)
			{
				int totalDuration = 60;
				var elapsedDuration = (int)(DateTime.Now - calculationResult.StartTime).Value.TotalMinutes;
				return (int)((double)elapsedDuration / totalDuration * 100);
			}

			return 100;
		}

		private CalculationStatus GetRandomStatus()
		{
			Random random = new Random();
			int randomNumber = random.Next(0, 3);

			switch (randomNumber)
			{
				case 0:
					return CalculationStatus.Running;
				case 1:
					return CalculationStatus.Completed;
				case 2:
					return CalculationStatus.Failed;
				default:
					return CalculationStatus.Running;
			}
		}

		private void InsertCalculationResult(Guid processId, string firstName, string lastName, CalculationStatus status, DateTime startTime)
		{
			_dbcontext.CalculationResults.Add(new CalculationResult
			{
				ProcessId = processId,
				FirstName = firstName,
				LastName = lastName,
				Status = status,
				StartTime = startTime
			});

			_dbcontext.SaveChanges();
		}

		private void UpdateCalculationResultStatus(Guid processId, CalculationStatus status)
		{
			var calculationResult = _dbcontext.CalculationResults.Where(c => c.ProcessId == processId).FirstOrDefault();

			if (calculationResult != null)
			{
				calculationResult.Status = status;
				calculationResult.EndTime = DateTime.Now;

				_dbcontext.SaveChanges();
			}
		}
	}
}
