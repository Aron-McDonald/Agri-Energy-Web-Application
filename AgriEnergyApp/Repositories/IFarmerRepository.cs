using AgriEnergyApp.Data;
using AgriEnergyApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyApp.Repositories
{
    public interface IFarmerRepository
    {
        IEnumerable<Farmer> GetAll();
        Farmer GetById(int id);
        void Add(Farmer farmer);
        void Update(Farmer farmer);
        void Delete(int id);
    }


    public class FarmerRepository : IFarmerRepository
    {
        private readonly ApplicationDbContext _context;

        public FarmerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Farmer> GetAll()
        {
            return _context.Farmers.ToList();
        }

        public Farmer GetById(int id)
        {
            return _context.Farmers.Find(id);
        }

        public void Add(Farmer farmer)
        {
            _context.Farmers.Add(farmer);
            _context.SaveChanges();
        }

        public void Update(Farmer farmer)
        {
            _context.Entry(farmer).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var farmer = _context.Farmers.Find(id);
            if (farmer != null)
            {
                _context.Farmers.Remove(farmer);
                _context.SaveChanges();
            }
        }
    }
}
