using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PresentationApp.Models;
using ShoppingCart.Application.Interfaces;

namespace PresentationApp.Controllers
{
    public class CartController : Controller
    {
        private IProductsService _prodService;
        private ICategoriesService _catService;
        private IWebHostEnvironment _env;
        public CartController(IProductsService prodService, ICategoriesService categoryService, IWebHostEnvironment env)
        {
            _prodService = prodService;
            _catService = categoryService;
            _env = env;
        }

        [Authorize]
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                int ExcludeRecords = (pageNumber * pageSize) - pageSize;
                var list = _prodService.GetProducts().Skip(ExcludeRecords).Take(pageSize);
                CatalogModel model = new CatalogModel() { Products = list, Categories = _catService.GetCategories() };

                /*var answer = new PagedResult<ProductViewModel>
                {
                    Data = list.AsNoTracking().ToList(),
                    TotalItems = _prodService.GetProducts().Count(),
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
                */

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Warning"] = "Failed to load the products. please try again later";
                return RedirectToAction("Error", "Home");
            }
        }

 

        [HttpPost][Authorize]
        public IActionResult AddtoCart(Guid productId)
        {
            string user = User.Identity.Name;

            //code to add to cart

            return RedirectToAction("Index");
        }

        [HttpGet] //this will be called before loading the Details page
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            CreateModel model = new CreateModel();

            var list = _catService.GetCategories();
            model.Categories = list.ToList();

            return View(model);
        }
    }
}
