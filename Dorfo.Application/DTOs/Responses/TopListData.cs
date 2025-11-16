using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Application.DTOs.Responses
{
    public class TopListData
    {
        public IEnumerable<TopMerchantDto> TopMerchantsByRevenue { get; set; } = new List<TopMerchantDto>();
        public IEnumerable<TopMerchantDto> TopMerchantsByOrderCount { get; set; } = new List<TopMerchantDto>();
        public IEnumerable<TopMerchantDto> TopMerchantsByRating { get; set; } = new List<TopMerchantDto>();
        public IEnumerable<TopMenuItemDto> TopMenuItemsBySales { get; set; } = new List<TopMenuItemDto>();
        public IEnumerable<TopMenuItemDto> TopMenuItemsByRevenue { get; set; } = new List<TopMenuItemDto>();
    }
}
