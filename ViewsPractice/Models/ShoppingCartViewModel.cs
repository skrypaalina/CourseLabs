using Microsoft.AspNetCore.Mvc.Rendering;

namespace ViewsPractice.Models;

public class ShoppingCartViewModel
{
    public string CustomerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Supermarket { get; set; } = string.Empty;
    public string DeliveryDate { get; set; } = string.Empty;
    public List<string> SelectedProducts { get; set; } = [];
    public List<SelectListItem> Supermarkets { get; set; } = [];
    public List<SelectListItem> Products { get; set; } = [];
    public List<string> DeliveryDates { get; set; } = [];
}
