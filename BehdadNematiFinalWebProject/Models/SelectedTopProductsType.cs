using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models
{
    public class SelectedTopProductsType
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public ProductType productType { get; set; }
    }
}
