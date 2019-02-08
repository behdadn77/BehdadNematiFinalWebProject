using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models.viewModels
{
    public class ProductViewModel
    {
        public int id { get; set; }
        [Required]
        public string EnglishName { get; set; }
        [Required]
        public int count { get; set; }
        [Required]
        public int price { get; set; }
        [Required]
        public int brand_Id { get; set; }
        [Required]
        public int productType_Id { get; set; }
        public List<IFormFile> Pictures { get; set; }

    }
}
