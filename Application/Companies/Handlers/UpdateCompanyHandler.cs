using Application.Companies.Commands;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Companies.Handlers
{
	internal sealed class UpdateCompanyHandler : IRequestHandler<UpdateCompanyCommand>
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;
        public UpdateCompanyHandler(IRepositoryManager repositoryManager, IMapper mapper)
        {
			_repositoryManager = repositoryManager;
			_mapper = mapper;
        }
        public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
		{
			var companyEntity = await _repositoryManager.Company.GetCompanyAsync(request.Id, request.trackChanges);
			if (companyEntity is null)
				throw new NullReferenceException(request.Id.ToString());

			_mapper.Map(request.Company, companyEntity);

			await _repositoryManager.SaveAsync();
		}
	}
}
