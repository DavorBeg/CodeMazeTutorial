using AutoMapper;
using Entities;
using Entities.Models;
using Shared.DataTransferObjects.AuthenticationDtos;
using Shared.DataTransferObjects.CompanyDtos;
using Shared.DataTransferObjects.EmployeeDtos;

namespace CodeMazeTutorial
{
    public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForCtorParam(nameof(CompanyDto.FullAddress), opt =>
                {
                    opt.MapFrom(x => string.Join(" ", x.Address, x.Country));
                });

            CreateMap<Employee, EmployeeDto>();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<CompanyForUpdateDto, Company>();

            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>().ReverseMap();

            CreateMap<UserForRegistrationDto, User>();

        }
    }
}
