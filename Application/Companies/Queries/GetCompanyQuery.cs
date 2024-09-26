using MediatR;
using Shared.DataTransferObjects.CompanyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Companies.Queries
{
	public sealed record GetCompanyQuery(Guid id, bool trackChanges) : IRequest<CompanyDto>;
}
