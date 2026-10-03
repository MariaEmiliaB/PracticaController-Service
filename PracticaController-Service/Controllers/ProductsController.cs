using Microsoft.AspNetCore.Mvc;
using PracticaController_Service.Entities;
using PracticaController_Service.Models.DTOs.Request;
using PracticaController_Service.Models.DTOs.Responses;
using PracticaController_Service.Services.Implementatios;

namespace PracticaController_Service.Controllers
{
    
   
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private ProductService _service = new ProductService();

        [HttpGet]
        public ActionResult<List<ProductForReadDTO>> GetAll()
        {
            var productos = _service.GetAllProducts();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductForReadDTO> GetById(int id)
        {
            var producto = _service.GetProductById(id);
            if (producto is null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct(ProductForCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("The name is required.");
            }

            if (dto.Price <= 0)
            {
                return BadRequest("The price must be greater than zero.");
            }

            var productoCreado = _service.CreateProduct(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = productoCreado.Id },
                productoCreado);
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductForUpdateDto dto)
        {
            var productoExistente = _service.GetProductById(id);

            if (productoExistente is null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest("The name is required.");
            }

            if (dto.Price <= 0)
            {
                return BadRequest("The price must be greater than zero.");
            }

            _service.UpdateProduct(id, dto);

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var productoExistente = _service.GetProductById(id);

            if (productoExistente is null)
            {
                return NotFound();
            }

            _service.DeleteProduct(id);

            return NoContent(); 
        }
    }
}

