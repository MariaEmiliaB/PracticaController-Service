using PracticaController_Service.Entities;
using PracticaController_Service.Models.DTOs.Request;
using PracticaController_Service.Models.DTOs.Responses;
using PracticaController_Service.Repositories.Interfaces;
using PracticaController_Service.Services.Interfaces;
namespace PracticaController_Service.Services.Implementatios
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public List<ProductForReadDTO> GetAllProducts()
        {
            var productos = _repository.GetAllProducts();
            var listaDtos = new List<ProductForReadDTO>();

            foreach (var product in productos)
            {
                var dto= new ProductForReadDTO();
                dto.Id= product.Id;
                dto.Name = product.Name;
                dto.Price = product.Price;
                listaDtos.Add(dto);
            }
            return listaDtos;
        }
        public ProductForReadDTO? GetProductById(int id)
        {
            var producto = _repository.GetProductById(id);
            if (producto == null)
            {
                return null;
            }
            var dto = new ProductForReadDTO();
            dto.Id = producto.Id;
            dto.Name = producto.Name;
            dto.Price = producto.Price;

            return dto;
        }
        public ProductForReadDTO CreateProduct(ProductForCreateDto dto)
        {
            var nuevoProducto = new Product();
            nuevoProducto.Name = dto.Name;
            nuevoProducto.Price = dto.Price;
            _repository.AddProduct(nuevoProducto);

            var dtoSalida = new ProductForReadDTO();
            dtoSalida.Id = nuevoProducto.Id;
            dtoSalida.Name = nuevoProducto.Name;
            dtoSalida.Price = nuevoProducto.Price;
            return dtoSalida;
        }
        public void UpdateProduct(int id, ProductForUpdateDto dto)
        {
            var productoExistente = _repository.GetProductById(id);
            if (productoExistente != null)
            {
                var productoUpdate = new Product();
                productoUpdate.Id = id;
                productoUpdate.Name = dto.Name;
                productoUpdate.Price = dto.Price;
                _repository.UpdateProduct(productoUpdate);
            }
        }
        public void DeleteProduct(int id)
        {
            var productoDelete = _repository.GetProductById(id);
            if (productoDelete != null)
            {
                _repository.DeleteProduct(productoDelete);
            }
        }


    }
}
