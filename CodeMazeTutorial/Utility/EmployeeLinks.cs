using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects.EmployeeDtos;
using System.Net.Http.Headers;

namespace CodeMazeTutorial.Utility
{
	public class EmployeeLinks : IEmployeeLinks
	{
		private readonly LinkGenerator _linkGenerator;
		private readonly IDataShaper<EmployeeDto> _dataShaper;

        public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
			_dataShaper = dataShaper;
        }
  //      public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext)
		//{
		//	var shapedEmployees = ShapeData(employeesDto, fields);
		//	if (ShouldGenerateLinks(httpContext))

		//}


		private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeesDto, string fields)
			=>
			 _dataShaper.ShapeData(employeesDto, fields)
			 .Select(e => e.Entity)
			 .ToList();		//private bool ShouldGenerateLinks(HttpContext httpContext)
		//{
		//	var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
			
			
		//}
	}
}
