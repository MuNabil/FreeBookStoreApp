using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Resources;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.ViewModel
{
    public class RegisterViewModel
    {
        public List<VmUser>? Users { get; set; }
        public NewRegister? NewRegister { get; set; }
        public List<IdentityRole>? Roles { get; set; }
        public ChangePasswordViewModel? ChangePassword { get; set; }
    }

    public class NewRegister
    {
        public string? Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("RigisterName"))]
        [MaxLength(20, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("NameMaxLength"))]
        [MinLength(3, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("NameMinLength"))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("RigisterEmail"))]
        [EmailAddress(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("RigisterEmailError"))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("RoleName"))]
        public string RoleName { get; set; }

        public string? UserImage { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("Password"))]
        [MaxLength(20, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("NameMaxLength"))]
        [MinLength(6, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("PasswordMinLength"))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("ConfirmPassword"))]
        [Compare("Password", ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = ("ConfirmPasswordError"))]
        public string ConfirmPassword { get; set; }

        
        public bool IsActive { get; set; }

    }
}
