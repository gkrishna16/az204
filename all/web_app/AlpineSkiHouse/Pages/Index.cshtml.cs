using Microsoft.AspNetCore.Mvc;
using AlpineSkiHouse.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AlpineSkiHouse.Services;

namespace AlpineSkiHouse.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
    public List<Product> Products;
    public void OnGet()
    {
        ProductService productService = new ProductService();
        Products = productService.GetProducts();
    }
}

