using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects.EmployeeDtos;
using Microsoft.Net.Http.Headers;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Metrics;
using System.Dynamic;
using Microsoft.AspNetCore.Http;

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



		public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext)
		{
			var shapedEmployees = ShapeData(employeesDto, fields);

			if (ShouldGenerateLinks(httpContext))
				return ReturnLinkdedEmployees(employeesDto, fields, companyId, httpContext, shapedEmployees);
			return ReturnShapedEmployees(shapedEmployees);
		}



		/// <summary>
		///In this method, we iterate through each employee and create links for it
		///by calling the CreateLinksForEmployee method. Then, we just add it to
		///the shapedEmployees collection. After that, we wrap the collection and
		///create links that are important for the entire collection by calling the
		///CreateLinksForEmployees method.
		/// </summary>
		/// <param name="employeesDto"></param>
		/// <param name="fields"></param>
		/// <param name="companyId"></param>
		/// <param name="httpContext"></param>
		/// <param name="shapedEmployees"></param>
		/// <returns></returns>
		private LinkResponse ReturnLinkdedEmployees(IEnumerable<EmployeeDto> employeesDto, string fields, Guid companyId, HttpContext httpContext, List<Entity> shapedEmployees)
		{
			var employeeDtoList = employeesDto.ToList();
			for (var index = 0; index < employeeDtoList.Count(); index++)
			{
				var employeeLinks = CreateLinksForEmployee(httpContext, companyId, employeeDtoList[index].Id, fields);
				shapedEmployees[index].Add("Links", employeeLinks);
			}
			var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
			var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);
			return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
		}



		private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid companyId, Guid id, string fields = "")
		{
			var links = new List<Link>
			{
				 new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany", values: new { companyId, id, fields }) ?? throw new NullReferenceException("GetUriByAction is null"), "self", "GET"),
				 new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteEmployeeForCompany", values: new { companyId, id }) ?? throw new NullReferenceException("GetUriByAction is null"), "delete_employee", "DELETE"),
				 new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateEmployeeForCompany", values: new { companyId, id }) ?? throw new NullReferenceException("GetUriByAction is null"), "update_employee", "PUT"),
				 new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateEmployeeForCompany", values: new { companyId, id }) ?? throw new NullReferenceException("GetUriByAction is null"), "partially_update_employee", "PATCH"),
			 };
			return links;
		}



		private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext, LinkCollectionWrapper<Entity> employeesWrapper)
		{
			employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeesForCompany", values: new { }) ?? throw new NullReferenceException("GetUriByAction is null"), "self", "GET"));
			return employeesWrapper;
		}



		private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeesDto, string fields)
			=>
			 _dataShaper.ShapeData(employeesDto, fields)
			 .Select(e => e.Entity)
			 .ToList();


		/// <summary>
		/// We extract the media type from
		/// the httpContext.If that media type ends with HATEOAS, the method
		/// returns true; otherwise, it returns false. The ReturnShapedEmployees
		/// method just returns a new LinkResponse with the ShapedEntities
		/// property populated.By default, the HasLinks property is false.
		/// </summary>
		/// <param name="httpContext"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>        private bool ShouldGenerateLinks(HttpContext httpContext)
		{
			var mediaType = (MediaTypeHeaderValue?)httpContext.Items["AcceptHeaderMediaType"];
			if (mediaType == null) throw new ArgumentNullException(nameof(MediaTypeHeaderValue));
			return mediaType.SubTypeWithoutSuffix.EndsWith("hateaos", StringComparison.InvariantCultureIgnoreCase);
		}		private LinkResponse ReturnShapedEmployees(List<Entity> shapedEmployees) => 
			new LinkResponse { ShapedEntities = shapedEmployees };
	}
}
