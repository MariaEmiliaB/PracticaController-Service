using PracticaController_Service.Models.DTOs.Request;
using PracticaController_Service.Models.DTOs.Responses;

namespace PracticaController_Service.Services.Interfaces
{
    public interface IProductService
    {
        List<ProductForReadDTO> GetAllProducts();
        ProductForReadDTO? GetProductById(int id);
        ProductForReadDTO CreateProduct(ProductForCreateDto dto);
        void UpdateProduct(int id, ProductForUpdateDto dto);
        void DeleteProduct(int id);

    }
}
