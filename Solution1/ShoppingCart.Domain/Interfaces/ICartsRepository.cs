using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICartsRepository
    {
        Product GetProduct(Guid id);
        IQueryable<Product> GetProducts();

        Guid AddProduct(Product p);

        void DeleteProduct(Guid id);

        //void HideProduct(Guid id);
    }
}
