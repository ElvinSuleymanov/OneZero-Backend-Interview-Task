using OneZero.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneZero.Application.Interfaces
{
    public interface IMenuService
    {
        Task<DataWrapper> GetAllMenu(DateTime Date);
        Task<MenuItem> GetProductById(int Id, DateTime Date);
    }
}
