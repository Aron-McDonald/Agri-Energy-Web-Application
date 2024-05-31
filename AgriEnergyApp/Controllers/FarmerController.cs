using AgriEnergyApp.Data;
using AgriEnergyApp.Models;
using AgriEnergyApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgriEnergyApp.Controllers
{
    [Authorize(Policy = "FarmerRole")]
    public class FarmerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFarmerRepository _farmerRepository;
        private readonly IProductRepository _productRepository;

        public FarmerController(ApplicationDbContext context, IFarmerRepository farmerRepository, IProductRepository productRepository)
        {
            _context = context;
            _farmerRepository = farmerRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var farmerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = _productRepository.GetByFarmerId(int.Parse(farmerId));
            return View(products);
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var farmerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var farmer = _context.Farmers.FirstOrDefault(f => f.Id.ToString() == farmerId);
                product.FarmerId = farmer.Id;
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Add other actions for farmer-specific functionality
    }
}
