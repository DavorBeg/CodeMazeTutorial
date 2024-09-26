using MediatR;
using Shared.DataTransferObjects.CompanyDtos;

namespace Application.Companies.Commands
{
	public sealed record UpdateCompanyCommand(Guid Id, CompanyForUpdateDto Company, bool trackChanges) : IRequest;
}
