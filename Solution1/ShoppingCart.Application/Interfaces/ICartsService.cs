using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface ICartsService
    {
        IQueryable<CartViewModel> GetProducts();
        IQueryable<CartViewModel> GetProducts(int selectedCategory);
        IQueryable<CartViewModel> GetProducts(string keyword);

        CartViewModel GetProduct(Guid id);

        void AddProduct(CartViewModel model);


        void DeleteProduct(Guid id);
    }
}
