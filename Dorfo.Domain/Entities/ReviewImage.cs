using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dorfo.Domain.Entities
{
    public class ReviewImage
    {
        public Guid ReviewImageId { get; set; }
        public Guid ReviewId { get; set; }
        public string ImgUrl { get; set; }
        public Review Review { get; set; }
    }
}
