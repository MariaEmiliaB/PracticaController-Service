using Microsoft.AspNetCore.Mvc;
using PracticaController_Service.Exceptions;
using PracticaController_Service.Models.DTOs.Requests;
using PracticaController_Service.Models.DTOs.Responses;
using PracticaController_Service.Services.Interfaces;

namespace PracticaController_Service.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ProductForReadDto>> GetAll()
        {
            var productos = _service.GetAllProducts();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductForReadDto> GetById(int id)
        {
            var producto = _service.GetProductById(id);
            if (producto is null)
            {
                return NotFound();
            }

            return Ok(producto);
        }

        // GET /api/products/search?name=teclado
        [HttpGet("search")]
        public ActionResult<List<ProductForReadDto>> Search([FromQuery] string name)
        {
            // Sin coincidencias devuelve 200 con lista vacía: la búsqueda funcionó, no hubo resultados.
            var productos = _service.SearchProductsByName(name);
            return Ok(productos);
        }

        // GET /api/products/stats
        [HttpGet("stats")]
        public ActionResult<ProductStatsDto> GetStats()
        {
            return Ok(_service.GetStats());
        }

        [HttpPost]
        public ActionResult<ProductForReadDto> Create(ProductForCreateDto dto)
        {
            // Las validaciones de entrada (nombre y precio) las hace [ApiController]
            // con las Data Annotations del DTO: si fallan, devuelve 400 sin entrar acá.
            try
            {
                var productoCreado = _service.CreateProduct(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = productoCreado.Id },
                    productoCreado);
            }
            catch (DuplicateProductNameException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductForUpdateDto dto)
        {
            var productoExistente = _service.GetProductById(id);
            if (productoExistente is null)
            {
                return NotFound();
            }

            try
            {
                _service.UpdateProduct(id, dto);
                return NoContent();
            }
            catch (DuplicateProductNameException ex)
            {
                return Conflict(ex.Message);
            }
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
