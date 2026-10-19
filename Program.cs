using wt_lab6_6minimalapi_talko.Data;
using wt_lab6_6minimalapi_talko.Models;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Minimal API работает!");

app.MapGet("/api/info", () =>
{
    return new
    {
        Application = "Служба доставки еды",
        Technology = "ASP.NET Core Minimal API",
        Version = "1.0"
    };
});

// GET: Список всех блюд (с поиском по query-параметру ?search=)
app.MapGet("/api/dishes", (string? search) =>
{
    var dishes = DishStore.Dishes.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        dishes = dishes.Where(d => d.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
    return Results.Ok(dishes);
});

// GET: Блюдо по ID
app.MapGet("/api/dishes/{id}", (int id) =>
{
    var dish = DishStore.Dishes.FirstOrDefault(d => d.Id == id);
    if (dish == null) return Results.NotFound();

    return Results.Ok(dish);
});

// POST: Создать новое блюдо
app.MapPost("/api/dishes", (Dish dish) =>
{
    // Автогенерация ID
    dish.Id = DishStore.Dishes.Count == 0 ? 1 : DishStore.Dishes.Max(d => d.Id) + 1;

    DishStore.Dishes.Add(dish);

    // Возвращаем 201 Created и ссылку на созданный объект
    return Results.Created($"/api/dishes/{dish.Id}", dish);
});

// PUT: Обновить блюдо по ID
app.MapPut("/api/dishes/{id}", (int id, Dish updatedDish) =>
{
    var dish = DishStore.Dishes.FirstOrDefault(d => d.Id == id);
    if (dish == null) return Results.NotFound();

    dish.Name = updatedDish.Name;
    dish.Category = updatedDish.Category;
    dish.Price = updatedDish.Price;
    dish.IsAvailable = updatedDish.IsAvailable;

    return Results.Ok(dish);
});

// DELETE: Удалить блюдо по ID
app.MapDelete("/api/dishes/{id}", (int id) =>
{
    var dish = DishStore.Dishes.FirstOrDefault(d => d.Id == id);
    if (dish == null) return Results.NotFound();

    DishStore.Dishes.Remove(dish);

    // 204 No Content (успешно, но отвечать нечем)
    return Results.NoContent();
});

// === API ДЛЯ КУРЬЕРОВ (Couriers) ===

app.MapGet("/api/couriers", () => Results.Ok(CourierStore.Couriers));

app.MapGet("/api/couriers/{id}", (int id) =>
{
    var courier = CourierStore.Couriers.FirstOrDefault(c => c.Id == id);
    return courier == null ? Results.NotFound() : Results.Ok(courier);
});

// === ОБНОВЛЕНИЕ И УДАЛЕНИЕ КУРЬЕРОВ ===

// PUT: Обновить данные курьера
app.MapPut("/api/couriers/{id}", (int id, Courier updatedCourier) =>
{
    var courier = CourierStore.Couriers.FirstOrDefault(c => c.Id == id);
    if (courier == null) return Results.NotFound();

    courier.FullName = updatedCourier.FullName;
    courier.Phone = updatedCourier.Phone;
    courier.Vehicle = updatedCourier.Vehicle;

    return Results.Ok(courier);
});

// DELETE: Удалить курьера
app.MapDelete("/api/couriers/{id}", (int id) =>
{
    var courier = CourierStore.Couriers.FirstOrDefault(c => c.Id == id);
    if (courier == null) return Results.NotFound();

    CourierStore.Couriers.Remove(courier);
    return Results.NoContent();
});

// === API ДЛЯ РЕСТОРАНОВ (Restaurants) ===

app.MapGet("/api/restaurants", () => Results.Ok(RestaurantStore.Restaurants));

app.MapGet("/api/restaurants/{id}", (int id) =>
{
    var restaurant = RestaurantStore.Restaurants.FirstOrDefault(r => r.Id == id);
    return restaurant == null ? Results.NotFound() : Results.Ok(restaurant);
});

// === ОБНОВЛЕНИЕ И УДАЛЕНИЕ РЕСТОРАНОВ ===

// PUT: Обновить данные ресторана
app.MapPut("/api/restaurants/{id}", (int id, Restaurant updatedRestaurant) =>
{
    var restaurant = RestaurantStore.Restaurants.FirstOrDefault(r => r.Id == id);
    if (restaurant == null) return Results.NotFound();

    restaurant.Title = updatedRestaurant.Title;
    restaurant.Address = updatedRestaurant.Address;
    restaurant.Rating = updatedRestaurant.Rating;

    return Results.Ok(restaurant);
});

// DELETE: Удалить ресторан
app.MapDelete("/api/restaurants/{id}", (int id) =>
{
    var restaurant = RestaurantStore.Restaurants.FirstOrDefault(r => r.Id == id);
    if (restaurant == null) return Results.NotFound();

    RestaurantStore.Restaurants.Remove(restaurant);
    return Results.NoContent();
});

// === API ДЛЯ ЗАКАЗОВ (Orders) ===

// 1. Получить все заказы
app.MapGet("/api/orders", () => Results.Ok(OrderStore.Orders));

// 2. Получить заказ по ID
app.MapGet("/api/orders/{id}", (int id) =>
{
    var order = OrderStore.Orders.FirstOrDefault(o => o.Id == id);
    return order == null ? Results.NotFound() : Results.Ok(order);
});

// 3. Создать новый заказ (POST)
app.MapPost("/api/orders", (Order order) =>
{
    // Автогенерация ID
    order.Id = OrderStore.Orders.Count == 0 ? 1 : OrderStore.Orders.Max(o => o.Id) + 1;

    // Автоматический расчет цены (ищем блюдо в базе и берем его цену)
    var dish = DishStore.Dishes.FirstOrDefault(d => d.Id == order.DishId);
    if (dish != null)
    {
        order.TotalPrice = dish.Price;
    }

    OrderStore.Orders.Add(order);
    return Results.Created($"/api/orders/{order.Id}", order);
});

// === ОБНОВЛЕНИЕ И УДАЛЕНИЕ ЗАКАЗОВ ===

// PUT: Обновить заказ (изменить адрес, статус или курьера)
app.MapPut("/api/orders/{id}", (int id, Order updatedOrder) =>
{
    var order = OrderStore.Orders.FirstOrDefault(o => o.Id == id);
    if (order == null) return Results.NotFound();

    order.DishId = updatedOrder.DishId;
    order.CourierId = updatedOrder.CourierId;
    order.Address = updatedOrder.Address;
    order.Status = updatedOrder.Status;

    // Пересчитываем цену, если поменялось блюдо
    var dish = DishStore.Dishes.FirstOrDefault(d => d.Id == order.DishId);
    if (dish != null)
    {
        order.TotalPrice = dish.Price;
    }

    return Results.Ok(order);
});

// DELETE: Удалить заказ
app.MapDelete("/api/orders/{id}", (int id) =>
{
    var order = OrderStore.Orders.FirstOrDefault(o => o.Id == id);
    if (order == null) return Results.NotFound();

    OrderStore.Orders.Remove(order);
    return Results.NoContent();
});


app.Run();