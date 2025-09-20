using Dorfo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.Interfaces.Repositories
{
    public interface IMenuItemOptionRepository
    {
        Task<MenuItemOption?> GetByIdAsync(Guid optionId);
    }
}
