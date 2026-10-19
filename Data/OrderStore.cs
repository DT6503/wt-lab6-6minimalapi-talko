using wt_lab6_6minimalapi_talko.Models;

namespace wt_lab6_6minimalapi_talko.Data;

public static class OrderStore
{
    public static List<Order> Orders { get; } = new()
    {
        new Order { Id = 1, DishId = 1, CourierId = 1, Address = "ул. Ленина 10", Status = "В пути", TotalPrice = 450m },
        new Order { Id = 2, DishId = 2, CourierId = 2, Address = "пр. Мира 5", Status = "Доставлен", TotalPrice = 599m },
        new Order { Id = 3, DishId = 4, CourierId = 1, Address = "ул. Гагарина 3", Status = "Готовится", TotalPrice = 700m }
    };
}