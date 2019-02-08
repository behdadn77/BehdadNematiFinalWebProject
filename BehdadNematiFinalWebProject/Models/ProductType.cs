using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        public string name { get; set; }
        public ICollection<product> products { get; set; }
    }
}
