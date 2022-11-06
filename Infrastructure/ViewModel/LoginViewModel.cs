using Domain.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("RigisterEmail"))]
        [EmailAddress(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("RigisterEmailError"))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("Password"))]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
