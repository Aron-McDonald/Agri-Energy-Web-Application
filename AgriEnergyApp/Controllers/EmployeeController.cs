using AgriEnergyApp.Data;
using AgriEnergyApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriEnergyApp.Controllers
{
    [Authorize(Policy = "EmployeeRole")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var farmers = _context.Farmers.ToList();
            return View(farmers);
        }

        public IActionResult AddFarmer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddFarmer(Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                _context.Farmers.Add(farmer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(farmer);
        }

        public IActionResult ViewProducts(int farmerId)
        {
            var products = _context.Products.Where(p => p.FarmerId == farmerId).ToList();
            return View(products);
        }

        // Add other actions for employee-specific functionality
    }
}
