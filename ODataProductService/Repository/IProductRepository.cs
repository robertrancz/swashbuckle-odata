using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ODataProductService.Models;

namespace ODataProductService.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();

        Product GetById(int id);

        Task<Product> GetByIdAsync(int id);

        void Add(Product product);

        Task<int> AddAsync(Product product);

        Task<int> SaveChangesAsync();

        Task<int> DeleteAsync(Product product);

        void Dispose();
    }
}