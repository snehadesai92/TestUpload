using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QUERION.Models
{
    public class UserModel : LoginModel
    {
        [Display(Name = "Email")]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "Security Question is required.")]
        [Display(Name = "Security Question")]
        public string Question { get; set; }

        [Required]
        [Display(Name = "Security Answer")]
        [StringLength(128, ErrorMessage = "Security Answer is required.")]
        public string Answer { get; set; }
    }
}
