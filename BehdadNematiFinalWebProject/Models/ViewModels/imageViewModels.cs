﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models.ViewModels
{
    public class imageViewModels
    {
        public int Id { get; set; }
        public IFormFile Img { get; set; }
        public int Product_Id { get; set; }
    }
}
