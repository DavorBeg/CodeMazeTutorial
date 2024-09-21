using Shared.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace Entities.LinkModels
{
	public sealed record LinkParameters(EmployeeParameters EmployeeParameters, HttpContext Context);
}
