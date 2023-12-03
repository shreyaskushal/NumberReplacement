using NumberReplacementApi.Constants;

namespace NumberReplacementApi.Models
{
	public class CalculationResult
	{
		public int Id { get; set; }
		public Guid ProcessId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public CalculationStatus Status { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
	}
}
