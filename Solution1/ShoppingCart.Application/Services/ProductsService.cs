using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class ProductsService : IProductsService
    {
        private IProductsRepository _productsRepo;
        private IMapper _autoMapper;
        public ProductsService(IProductsRepository productsRepo, IMapper autoMapper)
        {
            _productsRepo = productsRepo;
            _autoMapper = autoMapper;
        }

        public void AddProduct(ProductViewModel model)
        {
            _productsRepo.AddProduct(_autoMapper.Map<Product>(model));
        }

        public void DeleteProduct(Guid id)
        {
            _productsRepo.DeleteProduct(id);
        }

        /*public void HideProduct(Guid id)
        {
            _productsRepo.HideProduct(id);
        }
        */

        public ProductViewModel GetProduct(Guid id)
        {
            var p = _productsRepo.GetProduct(id);
            if (p == null) return null;
            else
            {
                var result = _autoMapper.Map<ProductViewModel>(p);
                return result;
            }

          
         
        }

        public IQueryable<ProductViewModel> GetProducts()
        {
            return _productsRepo.GetProducts().ProjectTo<ProductViewModel>(_autoMapper.ConfigurationProvider);
        }


        public IQueryable<ProductViewModel> GetProducts(string keyword)
        {
            
            return _productsRepo.GetProducts().Where(p => p.Description.Contains(keyword)  
                                                     || p.Name.Contains(keyword))          
                .ProjectTo<ProductViewModel>(_autoMapper.ConfigurationProvider);

        }


    }
}
