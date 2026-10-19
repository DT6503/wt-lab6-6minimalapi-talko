using wt_lab6_6minimalapi_talko.Models;

namespace wt_lab6_6minimalapi_talko.Data;

public static class RestaurantStore
{
    public static List<Restaurant> Restaurants { get; } = new()
    {
        new Restaurant { Id = 1, Title = "Золотой Колос", Address = "ул. Ленина 1", Rating = 4.8m },
        new Restaurant { Id = 2, Title = "Sakura", Address = "пр. Мира 15", Rating = 4.6m }
    };
}