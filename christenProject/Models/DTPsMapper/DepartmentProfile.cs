using christenProject.Model;
using christenProject.Models.DTOs;
using AutoMapper;

namespace christenProject.Models.DTPsMapper
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            // Create a map from Employee to EmployeeDTO
            CreateMap<DepartmentModel, DepartmentDTOPostPut>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Name}"))
                .ForMember(dest => dest.MName, opt => opt.MapFrom(src => src.ManagerName));
        }
    }
}