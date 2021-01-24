using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class CartService
    {
        private ICartsRepository _cartsRepo;
        public CartService(ICartsRepository categoriesRepo)
        {
            _cartsRepo = categoriesRepo;
        }

        public IQueryable<CartViewModel> GetCategories()
        {
            var list = from c in _cartsRepo.GetProducts()
                       select new CartViewModel()
                       {
                           Id = c.Id,
                           Name = c.Name
                       };

            return list;
        }
    }
}
