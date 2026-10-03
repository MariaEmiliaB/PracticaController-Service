using Microsoft.AspNetCore.Mvc;
using PracticaController_Service.Entities;

namespace PracticaController_Service.Models.DTOs.Responses
{
    public class ProductForReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
