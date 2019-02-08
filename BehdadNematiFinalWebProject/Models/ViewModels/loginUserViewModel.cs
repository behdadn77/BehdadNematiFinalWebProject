using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models.viewModels
{
    public class loginUserViewModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public bool rememberme { get; set; }

    }
}
