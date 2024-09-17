using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
	public class Company
	{
		[Column("CompanyId")]
		public Guid Id { get; set; }

		[Required(ErrorMessage = "Company name is a required field.")]
		[MaxLength(100, ErrorMessage = "Maximum Lenght is 100 characters.")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Adress name is a required field.")]
		[MaxLength(100, ErrorMessage = "Maximum Lenght is 100 characters.")]
		public string? Adress { get; set; }

		public string? Country { get; set; }

		public ICollection<Employee>? Employees { get; set; }

	}
}
