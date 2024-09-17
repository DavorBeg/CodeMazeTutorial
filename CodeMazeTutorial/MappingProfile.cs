using AutoMapper;
using Entities;
using Shared.DataTransferObjects;

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
        }
    }
}
