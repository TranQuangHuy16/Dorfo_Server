using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Requests
{
    public class UpdateSelectedValueRequest
    {
        public Guid CartItemId { get; set; }
        public Guid OptionId { get; set; }
        public List<Guid> SelectedValueIds { get; set; } = new();
    }
}
