using AutoMapper;
using Todo_list.DTO;
using Todo_list.Models;
using Todo_list.DTO;
using Todo_list.Models;

namespace TodoApi.Mappings;

/// <summary>
/// Профиль маппинга для AutoMapper
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Маппинг для TodoList
        CreateMap<TodoList, TodoListDTO>()
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.Items.Count))
            .ForMember(dest => dest.CompletedCount, opt => opt.MapFrom(src => src.Items.Count(i => i.IsCompleted)));

        CreateMap<TodoList, TodoListDetailDTO>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<CreateTodoListDTO, TodoList>();
        CreateMap<UpdateTodoListDTO, TodoList>();

        // Маппинг для TodoItem
        CreateMap<TodoItem, TodoItemDTO>()
            .ForMember(dest => dest.TodoListTitle, opt => opt.MapFrom(src => src.TodoList.Title));

        CreateMap<CreateTodoItemDTO, TodoItem>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(_ => false));

        CreateMap<UpdateTodoItemDTO, TodoItem>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.TodoListId, opt => opt.Ignore());
    }
}