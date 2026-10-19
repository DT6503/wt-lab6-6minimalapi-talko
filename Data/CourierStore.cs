using wt_lab6_6minimalapi_talko.Models;

namespace wt_lab6_6minimalapi_talko.Data;

public static class CourierStore
{
    public static List<Courier> Couriers { get; } = new()
    {
        new Courier { Id = 1, FullName = "Иванов Иван", Phone = "+375291234567", Vehicle = "Скутер" },
        new Courier { Id = 2, FullName = "Петрова Анна", Phone = "+375297654321", Vehicle = "Велосипед" },
        new Courier { Id = 3, FullName = "Сидоров Петр", Phone = "+375293334455", Vehicle = "Автомобиль" }
    };
}