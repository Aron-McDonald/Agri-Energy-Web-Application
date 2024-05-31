using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AgriEnergyApp.Models;
using AgriEnergyApp.Repositories;

namespace AgriEnergyApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IFarmerRepository _farmerRepository;
        private readonly IProductRepository _productRepository;

        public ProductController(IFarmerRepository farmerRepository, IProductRepository productRepository)
        {
            _farmerRepository = farmerRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var farmerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = _productRepository.GetByFarmerId(int.Parse(farmerId));
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var farmerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                product.FarmerId = int.Parse(farmerId);
                _productRepository.Add(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}