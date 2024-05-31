using AgriEnergyApp.Data;
using AgriEnergyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyApp.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetByFarmerId(int farmerId);
        Product GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public IEnumerable<Product> GetByFarmerId(int farmerId)
        {
            return _context.Products.Where(p => p.FarmerId == farmerId).ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Farmers.Find(id);
            if (product != null)
            {
                _context.Farmers.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
