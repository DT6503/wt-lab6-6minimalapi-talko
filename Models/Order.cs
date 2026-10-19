namespace wt_lab6_6minimalapi_talko.Models;

public class Order
{
    public int Id { get; set; }
    public int DishId { get; set; }
    public int CourierId { get; set; }
    public string Address { get; set; } = "";
    public string Status { get; set; } = "Новый";
    public decimal TotalPrice { get; set; }
}