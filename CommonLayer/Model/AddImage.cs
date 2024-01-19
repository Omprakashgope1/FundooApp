using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Model
{
    public class AddImage
    {
        public IFormFile Image { get; set; }
        public long id { get; set; }
    }
}
