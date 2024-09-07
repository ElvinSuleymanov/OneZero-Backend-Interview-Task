using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneZero.Application.Interfaces;
using OneZero.Domain.Entities;

namespace OneZero.API.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class MenuController(
        IMenuService menuService
        ) : ControllerBase
    {
        private readonly IMenuService _menuService = menuService;

        [HttpGet]
        public async Task<DataWrapper> GetMenuAsync(DateTime date) => await menuService.GetAllMenu(date);
    }
}
