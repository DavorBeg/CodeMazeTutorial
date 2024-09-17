using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
	public class Employee
	{
		[Column("EmployeeId")]
		public Guid Id { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		[MaxLength(30, ErrorMessage = "Maximum lenght of name is 30 characters.")]
		public string? Name { get; set; }

		[Required(ErrorMessage = "Age is required")]
		public int Age { get; set; }

		[Required(ErrorMessage = "Position is required.")]
		[MaxLength(20, ErrorMessage = "Maximum lenght for the Position is 20")]
		public string? Position { get; set; }

		[ForeignKey(nameof(Company))]
		public Guid CompanyId { get; set; }
		public Company? Company { get; set; }

	}
}
