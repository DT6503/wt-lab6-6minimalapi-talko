using wt_lab6_6minimalapi_talko.Models;

namespace wt_lab6_6minimalapi_talko.Data;

public static class DishStore
{
    public static List<Dish> Dishes { get; } = new()
    {
        new Dish { Id = 1, Name = "Пицца Маргарита", Category = "Пицца", Price = 450m, IsAvailable = true },
        new Dish { Id = 2, Name = "Суши Филадельфия", Category = "Суши", Price = 599m, IsAvailable = true },
        new Dish { Id = 3, Name = "Борщ украинский", Category = "Супы", Price = 299m, IsAvailable = false },
        new Dish { Id = 4, Name = "Паста Карбонара", Category = "Паста", Price = 700m, IsAvailable = true },
        new Dish { Id = 5, Name = "Цезарь с курицей", Category = "Салаты", Price = 380m, IsAvailable = true }
    };
}