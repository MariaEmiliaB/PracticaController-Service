using PracticaController_Service.Data;
using PracticaController_Service.Entities;
using PracticaController_Service.Repositories.Interfaces;

namespace PracticaController_Service.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product? GetProductById(int id)
        {
            return _context.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existente = _context.Products.Find(product.Id);
            if (existente != null)
            {
                _context.Entry(existente).CurrentValues.SetValues(product);
                _context.SaveChanges();
            }
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public List<Product> SearchProductsByName(string name)
        {
            return _context.Products
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .ToList();
        }
    }
}
