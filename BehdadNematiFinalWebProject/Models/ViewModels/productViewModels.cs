using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models.viewModels
{
    public class productViewModels
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public int count { get; set; }
        public int price { get; set; }
        public int brand_Id { get; set; }
        public int productType_Id { get; set; }
    }
}
