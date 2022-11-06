using Domain.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
    public class ChangePasswordViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("Password"))]
        [MaxLength(20, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("NameMaxLength"))]
        [MinLength(6, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("PasswordMinLength"))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("ConfirmPassword"))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("ConfirmPasswordError"))]
        public string ConfirmPassword { get; set; }
    }
}
