using PracticaController_Service.Entities;
using PracticaController_Service.Exceptions;
using PracticaController_Service.Models.DTOs.Requests;
using PracticaController_Service.Models.DTOs.Responses;
using PracticaController_Service.Repositories.Interfaces;
using PracticaController_Service.Services.Interfaces;

namespace PracticaController_Service.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public List<ProductForReadDto> GetAllProducts()
        {
            var productos = _repository.GetAllProducts();
            var listaDtos = new List<ProductForReadDto>();

            foreach (var product in productos)
            {
                listaDtos.Add(MapToReadDto(product));
            }
            return listaDtos;
        }

        public ProductForReadDto? GetProductById(int id)
        {
            var producto = _repository.GetProductById(id);
            if (producto == null)
            {
                return null;
            }
            return MapToReadDto(producto);
        }

        public ProductForReadDto CreateProduct(ProductForCreateDto dto)
        {
            // Regla de negocio: no puede haber dos productos con el mismo nombre.
            if (NameIsTaken(dto.Name, excludeId: null))
            {
                throw new DuplicateProductNameException(dto.Name);
            }

            var nuevoProducto = new Product();
            nuevoProducto.Name = dto.Name;
            nuevoProducto.Price = dto.Price;
            _repository.AddProduct(nuevoProducto);

            return MapToReadDto(nuevoProducto);
        }

        public void UpdateProduct(int id, ProductForUpdateDto dto)
        {
            var productoExistente = _repository.GetProductById(id);
            if (productoExistente == null)
            {
                return;
            }

            // Al editar, el propio producto puede conservar su nombre.
            if (NameIsTaken(dto.Name, excludeId: id))
            {
                throw new DuplicateProductNameException(dto.Name);
            }

            var productoUpdate = new Product();
            productoUpdate.Id = id;
            productoUpdate.Name = dto.Name;
            productoUpdate.Price = dto.Price;
            _repository.UpdateProduct(productoUpdate);
        }

        public void DeleteProduct(int id)
        {
            var productoDelete = _repository.GetProductById(id);
            if (productoDelete != null)
            {
                _repository.DeleteProduct(productoDelete);
            }
        }

        public List<ProductForReadDto> SearchProductsByName(string name)
        {
            var productos = _repository.SearchProductsByName(name);
            var listaDtos = new List<ProductForReadDto>();

            foreach (var product in productos)
            {
                listaDtos.Add(MapToReadDto(product));
            }
            return listaDtos;
        }

        public ProductStatsDto GetStats()
        {
            var productos = _repository.GetAllProducts();
            var stats = new ProductStatsDto();

            // Average() sobre una lista vacía lanza una excepción: devolvemos todo en cero.
            if (!productos.Any())
            {
                return stats;
            }

            stats.Total = productos.Count();
            stats.AveragePrice = Math.Round(productos.Average(p => p.Price), 2);
            stats.MostExpensiveName = productos.OrderByDescending(p => p.Price).First().Name;
            return stats;
        }

        private bool NameIsTaken(string name, int? excludeId)
        {
            return _repository.GetAllProducts().Any(p =>
                p.Id != excludeId &&
                string.Equals(p.Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private static ProductForReadDto MapToReadDto(Product product)
        {
            var dto = new ProductForReadDto();
            dto.Id = product.Id;
            dto.Name = product.Name;
            dto.Price = product.Price;
            return dto;
        }
    }
}
