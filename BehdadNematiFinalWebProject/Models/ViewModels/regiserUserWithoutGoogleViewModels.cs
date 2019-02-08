using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BehdadNematiFinalWebProject.Models.viewModels
{
    public class regiserUserWithoutGoogleViewModels
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string lastname { get; set; }
        [Required]
        public string username { get; set; }
        public string phonenumber { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        [Compare("password")]
        public string passwordConfirm { get; set; }
        [Required]
        public string email { get; set; }
        public bool rememberme { get; set; }

    }
}
