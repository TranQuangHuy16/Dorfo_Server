using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class TopMenuItemDto
    {
        public Guid MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Value { get; set; } // Có thể là doanh thu hoặc số lượng bán
    }
}
