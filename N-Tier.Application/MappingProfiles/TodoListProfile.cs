using AutoMapper;
using N_Tier.Application.Models.TodoList;
using N_Tier.Core.Entities;

namespace N_Tier.Application.MappingProfiles
{
    public class TodoListProfile : Profile
    {
        public TodoListProfile()
        {
            CreateMap<CreateTodoListModel, TodoList>();
        }
    }
}
