using AutoMapper;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Core.Entities;

namespace N_Tier.Application.MappingProfiles
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<CreateTodoItemModel, TodoItem>();
        }
    }
}
