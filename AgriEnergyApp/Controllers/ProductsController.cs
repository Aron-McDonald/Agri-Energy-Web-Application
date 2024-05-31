using AgriEnergyApp.Models;
using AgriEnergyApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AgriEnergyApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            this.context = context;
            _userManager = userManager;
            this.environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains("farmer"))
            {
                var products = await context.Products.Where(p => p.FarmerId == user.Id).ToListAsync();
                return View(products);
            }
            else
            {
                // Handle the case when the user is not a farmer
                // You can show an appropriate message or redirect to another page
                return View("NotAuthorized");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssff");
            newFileName += Path.GetExtension(productDto.ImageFile.FileName);
            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            var user = await _userManager.GetUserAsync(User);

            // save the new product in the database
            Product product = new Product()
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Category = productDto.Category,
                ImageFileName = newFileName,
                Availability = productDto.Availability,
                FarmerId = user.Id // Set the FarmerId to the current user's ID
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Products");

            

        }

        [Authorize(Roles = "employee")]
        public async Task<IActionResult> ProductsByFarmer(string farmerId, string category = null)
        {
            var farmer = await _userManager.FindByIdAsync(farmerId);
            if (farmer == null || !await _userManager.IsInRoleAsync(farmer, "farmer"))
            {
                return NotFound();
            }

            var productsQuery = context.Products.Where(p => p.FarmerId == farmerId);

            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.Category == category);
            }

            var products = await productsQuery.ToListAsync();

            ViewBag.FarmerName = farmer.UserName;
            ViewBag.Category = category;
            return View(products);
        }

        [Authorize(Roles = "farmer")]
        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            // create productDto from product
            var productDto = new ProductDto()
            {
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                Availability = product.Availability,

            };

            ViewData["ProductId"] = id;
            ViewData["ImageFileName"] = product.ImageFileName;

            return View(productDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageFileName"] = product.ImageFileName;


                return View(productDto);
            }

            // update the image file if we have a new image file
            string newFileName = product.ImageFileName;
            if (productDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssff");
                newFileName += Path.GetExtension(productDto.ImageFile.FileName);
                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }

                // delete the old image file
                string oldImageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update the product in the database
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Category = productDto.Category;
            product.ImageFileName = newFileName;
            product.Availability = productDto.Availability;

            context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        [Authorize(Roles = "farmer")]
        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }
            string imageFullPath = environment.WebRootPath + "/products/" + product.ImageFileName;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            context.SaveChanges(true);

            return RedirectToAction("Index", "Products");
        }
    }
}
