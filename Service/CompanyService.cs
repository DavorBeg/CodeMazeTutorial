﻿using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Entities.Responses;
using Entities.Responses.Abstract;
using Service.Contracts;
using Shared.DataTransferObjects.CompanyDtos;
using System.Formats.Asn1;

namespace Service
{
	internal sealed class CompanyService : ICompanyService
	{
		private readonly IRepositoryManager _repository;
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;

		public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}

		private async Task<Company> GetCompanyAndCheckIfItExist(Guid id, bool trackChanges)
		{
			var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
			return company;
		}

		public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
		{
			if (companyCollection is null)
				throw new CompanyCollectionBadRequest();

			var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
			foreach ( var company in companyEntities)
			{
				_repository.Company.CreateCompany(company);
			}

			await _repository.SaveAsync();

			var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
			var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

			return (companies:  companyCollectionToReturn, ids);
		}

		public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
		{
			if (ids == null)
				throw new IdParametersBadRequestException();

			var companyEntities = await _repository.Company.GetByIdsAsync(ids, trackChanges);
			if(ids.Count() != companyEntities.Count())
				throw new CollectionByIdsBadRequestException();

			var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
			return companiesToReturn;
		}


		public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company)
		{
			var companyEntity = _mapper.Map<Company>(company);

			_repository.Company.CreateCompany(companyEntity);
			await _repository.SaveAsync();

			var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
			return companyToReturn;
		}

		public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
		{
				var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);
				var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
				return companiesDto;
		}

		public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
		{
			var company = await this.GetCompanyAndCheckIfItExist(companyId, trackChanges);

			var companyDto = _mapper.Map<CompanyDto>(company);
			return companyDto;
		}

		public async Task DeleteCompanyAsync(Guid companyId, bool trackChanges)
		{
			var company = await this.GetCompanyAndCheckIfItExist(companyId, trackChanges);

			_repository.Company.DeleteCompany(company);
			await _repository.SaveAsync();
		}

		public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdateDto companyForUpdate, bool trackChanges)
		{
			var company = await this.GetCompanyAndCheckIfItExist(companyId, trackChanges);

			_mapper.Map(companyForUpdate, company);
			await _repository.SaveAsync();
		}

		public async Task<ApiBaseResponse> GetAllCompanies(bool trackChanges)
		{
			var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);
			var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

			return new ApiOkResponse<IEnumerable<CompanyDto>>(companiesDto);
		}

		public async Task<ApiBaseResponse> GetCompany(Guid companyId, bool trackChanges)
		{
			var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
			if (company is null)
				return new CompanyNotFoundException(companyId);

			var companyDto = _mapper.Map<CompanyDto>(company);
			return new ApiOkResponse<CompanyDto>(companyDto);
		}

	}
}
