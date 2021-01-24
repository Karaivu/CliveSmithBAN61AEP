using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationApp.Models;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore;

namespace PresentationApp.Controllers
{
    public class ProductsController : Controller
    {
        private IProductsService _prodService;
        private ICategoriesService _catService;
        private IWebHostEnvironment _env;
        public ProductsController(IProductsService prodService, ICategoriesService categoryService, IWebHostEnvironment env)
        {
            _prodService = prodService;
            _catService = categoryService;
            _env = env;
        }

        /*public IActionResult Index2()
        {
            try
            {
                var list = _prodService.GetProducts();
                CatalogModel model = new CatalogModel() { Products = list, Categories = _catService.GetCategories() };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Warning"] = "Failed to load the products. please try again later";
                return RedirectToAction("Error", "Home");
            }
        }
        */

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

        public IActionResult Details(Guid Id)
        {
            return View(_prodService.GetProduct(Id));
        }

        //Search Function

        [HttpPost]
        public IActionResult Search(int SelectedCategory)
        {
            //1. perform search therefore return list products by category
            //2. catalogmodel

            var list = _prodService.GetProducts(SelectedCategory);
            CatalogModel model = new CatalogModel() { Products = list, Categories = _catService.GetCategories() };

            return View("Index", model);
        }

        //-------------------- ADD -----------------------------

        [HttpGet] //this will be called before loading the Create page
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            CreateModel model = new CreateModel();

            var list = _catService.GetCategories();
            model.Categories = list.ToList();

            return View(model);
        }

        [HttpPost] //2nd method will  be triggered when the user clicks on the submit button!
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateModel data) 
        {
            //validation
            try
            {
                if (data.File != null)
                {
                    if (data.File.Length > 0)
                    {
                        string newFilename = @"/Images/" + Guid.NewGuid() + System.IO.Path.GetExtension(data.File.FileName);
                        string absolutePath = _env.WebRootPath;

                        using (var stream = System.IO.File.Create(absolutePath + newFilename))
                        {
                            data.File.CopyTo(stream);
                        }

                        data.Product.ImageUrl = newFilename;
                    }
                }

                _prodService.AddProduct(data.Product);

                TempData["feedback"] = "Product added successfully";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                TempData["warning"] = "Product was not added successfully. Check your inputs";
            }

            var list = _catService.GetCategories();
            data.Categories = list.ToList();

            return View(data);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            _prodService.DeleteProduct(id);

            TempData["feedback"] = "product deleted successfully";  //ViewData should be changed to TempData

            return RedirectToAction("Index");
        }

        /*[Authorize(Roles = "Admin")]
        public IActionResult Hide(Guid id)
        {
            _prodService.HideProduct(id);

            TempData["feedback"] = "product hidden successfully"; 

            return RedirectToAction("Index");
        }

        */

    }
}