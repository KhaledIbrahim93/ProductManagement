using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductBLL.DTO
{
    public class ProductRequest
    {
       
        public string Name { get; set; }
       
        public string Price { get; set; }
       public IFormFile Image { get; set; }
    
    }
    public class NewProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Price { get; set; }
    }
}
