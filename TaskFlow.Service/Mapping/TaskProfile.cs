using AutoMapper;
using Model.Models;
using TaskFlow.Model.Models;
using TaskFlow.Service.DTOs.Task;
using TaskFlow.Service.DTOs.User;

namespace Service.Mapping
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(x => x.TaskPriorityName, opt => opt.MapFrom(x => x.TaskPriority != null ? x.TaskPriority.Name : ""));
            CreateMap<TaskItemDto, TaskItem>();
            CreateMap<TaskCategory, TaskCategoryDto>().ReverseMap();
            CreateMap<TaskPriority, TaskPriorityDto>().ReverseMap();
            CreateMap<FriendRelation, AppUserDto>()
                .ForMember(x => x.Username, opt => opt.MapFrom(x => x.User2.UserName));
            CreateMap<FriendRelation, ReceivedFriendRequestDto>()
                .ForMember(x => x.Username, opt => opt.MapFrom(x => x.User1.UserName));
            CreateMap<FriendRelation, SentFriendRequestDto>()
                .ForMember(x => x.Username, opt => opt.MapFrom(x => x.User2.UserName));
        }
    }
}
