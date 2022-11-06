using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class VmUser
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public string? UserImage { get; set; }
        public string? Email { get; set; }
    }
}
