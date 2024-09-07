using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneZero.Application.Interfaces;
using OneZero.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OneZero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(
        IMenuService menuService
        ) : ControllerBase
    {
        private readonly IMenuService _menuService = menuService;
        [HttpGet]
        public async Task<MenuItem> Get(int Id, DateTime Date) => await _menuService.GetProductById(Id, Date);
    }
}
