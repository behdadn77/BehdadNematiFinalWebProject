using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models.ViewModels
{
    public class ProductViewModel
    {
        public int id { get; set; }
        [Required]
        public string EnglishName { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int Brand_Id { get; set; }
        [Required]
        public int ProductType_Id { get; set; }
        public List<IFormFile> Pictures { get; set; }
        public List<string> ImagesBase64List {get; set;}
        public string ImageBase64 { get; set; }
        public string thumbnailImageBase64 { get; set; }
    }
}
