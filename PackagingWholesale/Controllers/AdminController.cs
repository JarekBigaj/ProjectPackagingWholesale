﻿using Microsoft.AspNetCore.Mvc;
using PackagingWholesale.Models;
using System.Linq;

namespace PackagingWholesale.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index() => View(repository.Products);

        public ViewResult Edit(int productId) => View(repository.Products.FirstOrDefault(p => p.ProductID == productId));

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = $"Zapisano {product.Name}";
                return RedirectToAction("Index");
            }
            else
                return View(product);
        }

        public ViewResult Create() => View("Edit", new Product());

        [HttpPost]
        public IActionResult Delete(int productId)
        {
            Product deletedProduct = repository.DeleteProduct(productId);
            if(deletedProduct != null)
            {
                TempData["message"] = $"Usunięto {deletedProduct.Name}";
            }
            return RedirectToAction("Index");
        }
    }
}
