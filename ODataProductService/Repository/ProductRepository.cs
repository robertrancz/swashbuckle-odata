using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ODataProductService.Models;

namespace ODataProductService.Repository
{
    public class ProductRepository : IDisposable, IProductRepository
    {
        private ProductsContext db = new ProductsContext();

        public IEnumerable<Product> GetAll()
        {
            return db.Products;
        }

        public Product GetById(int id)
        {
            return db.Products.FirstOrDefault(p => p.Id == id);
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return db.Products.FindAsync(id);
        }

        public void Add(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
        }

        public Task<int> AddAsync(Product product)
        {
            db.Products.Add(product);
            return db.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(Product product)
        {
            db.Products.Remove(product);
            return db.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync()
        {
            return db.SaveChangesAsync();
        }

        //public bool IsModified(Product product)
        //{
        //    db.Entry(product).State = EntityState.Modified;
        //}

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                db?.Dispose();
                db = null;
            }
        }
    }
}