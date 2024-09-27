using Application.Companies.Commands;
using Application.Companies.Notifications;
using Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Companies.Handlers
{
	internal sealed class DeleteCompanyHandler : INotificationHandler<CompanyDeletedNotification>
	{
		private readonly IRepositoryManager _repository;
		public DeleteCompanyHandler(IRepositoryManager repository) => _repository = repository; 

		public async Task Handle(CompanyDeletedNotification request, CancellationToken cancellationToken)
		{
			var company = await _repository.Company.GetCompanyAsync(request.Id, request.TrackChanges);
			if (company is null)
				throw new NullReferenceException("Returned company is null value.");

			_repository.Company.DeleteCompany(company);
			await _repository.SaveAsync();
		}
	}
}
