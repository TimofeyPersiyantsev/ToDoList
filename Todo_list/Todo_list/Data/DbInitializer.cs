using Todo_list.Data;
using Todo_list.Models;
using Todo_list.Models;

namespace Todo_list.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        if (context.TodoLists.Any())
        {
            return;
        }

        var lists = new TodoList[]
        {
            new TodoList{Title = "Рабочие задачи"},
            new TodoList{Title = "Личные дела"},
            new TodoList{Title = "Покупки"},
            new TodoList{Title = "Идеи для проектов"},
            new TodoList{Title = "Фильмы к просмотру"},
        };
        context.TodoLists.AddRange(lists);
        context.SaveChanges();

        var items = new TodoItem[]
        {
            // Задачи для списка 1 (Рабочие задачи)
            new TodoItem{TodoListId = 1, Title = "Подготовить отчет", Description = "Квартальный отчет по продажам", DueDate = DateTime.Today.AddDays(2)},
            new TodoItem{TodoListId = 1, Title = "Созвониться с клиентом", IsCompleted = true},
            new TodoItem{TodoListId = 1, Title = "Обновить документацию"},
            new TodoItem{TodoListId = 1, Title = "Планерка в 10:00", DueDate = DateTime.Today},
            new TodoItem{TodoListId = 1, Title = "Заказать новые канцтовары"},

            // Задачи для списка 2 (Личные дела)
            new TodoItem{TodoListId = 2, Title = "Записаться к врачу"},
            new TodoItem{TodoListId = 2, Title = "Позвонить родителям", IsCompleted = true},
            new TodoItem{TodoListId = 2, Title = "Оплатить коммунальные услуги", DueDate = DateTime.Today.AddDays(5)},
            new TodoItem{TodoListId = 2, Title = "Сходить в спортзал"},
            new TodoItem{TodoListId = 2, Title = "Почитать книгу"},

            // Задачи для списка 3 (Покупки)
            new TodoItem{TodoListId = 3, Title = "Купить молоко"},
            new TodoItem{TodoListId = 3, Title = "Хлеб", IsCompleted = true},
            new TodoItem{TodoListId = 3, Title = "Фрукты"},
            new TodoItem{TodoListId = 3, Title = "Крупа"},
            new TodoItem{TodoListId = 3, Title = "Кофе"},

            // Задачи для списка 4 (Идеи для проектов)
            new TodoItem{TodoListId = 4, Title = "Изучить Blazor"},
            new TodoItem{TodoListId = 4, Title = "Написать мобильное приложение", Description = "Идея для трекера привычек"},
            new TodoItem{TodoListId = 4, Title = "Создать личный блог"},
            new TodoItem{TodoListId = 4, Title = "Опробовать новую cloud платформу"},
            new TodoItem{TodoListId = 4, Title = "Участвовать в опенсорс проекте"},

            // Задачи для списка 5 (Фильмы к просмотру)
            new TodoItem{TodoListId = 5, Title = "Интерстеллар"},
            new TodoItem{TodoListId = 5, Title = "Начало", IsCompleted = true},
            new TodoItem{TodoListId = 5, Title = "Зеленая книга"},
            new TodoItem{TodoListId = 5, Title = "Облачный атлас"},
            new TodoItem{TodoListId = 5, Title = "Довод"},
        };

        context.TodoItems.AddRange(items);
        context.SaveChanges();
    }
}
