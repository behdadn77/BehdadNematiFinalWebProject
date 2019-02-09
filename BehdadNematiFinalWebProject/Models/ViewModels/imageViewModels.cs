using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models.ViewModels
{
    public class imageViewModels
    {
        public int Id { get; set; }
        public IFormFile img { get; set; }
        public int product_Id { get; set; }
    }
}
