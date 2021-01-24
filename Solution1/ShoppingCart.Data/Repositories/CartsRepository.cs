using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class CartsRepository : ICartsRepository
    {
        ShoppingCartDbContext _context;
        public CartsRepository(ShoppingCartDbContext context)
        {

            _context = context;
        }

        public Guid AddProduct(Product p)
        {

            _context.Products.Add(p);
            _context.SaveChanges();

            return p.Id;
        }

        public void DeleteProduct(Guid id)
        {
            Product p = GetProduct(id);
            _context.Products.Remove(p);
            _context.SaveChanges();
        }

        public Product GetProduct(Guid id)
        {
            return _context.Products.Include(x => x.Category).SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<Product> GetProducts()
        {
            return _context.Products.Include(x => x.Category);
        }
    }
}
