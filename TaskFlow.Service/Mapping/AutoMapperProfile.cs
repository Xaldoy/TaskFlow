using AutoMapper;
using Model.Models;
using Service.DTOs;

namespace Service.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(x => x.TaskPriorityName, opt => opt.MapFrom(x => x.TaskPriority != null ? x.TaskPriority.Name : ""));
            CreateMap<TaskItemDto, TaskItem>();
            CreateMap<TaskCategory, TaskCategoryDto>().ReverseMap();
            CreateMap<TaskPriority, TaskPriorityDto>().ReverseMap();
        }
    }
}
