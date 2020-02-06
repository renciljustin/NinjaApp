using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace NinjaApp.Core.Models
{
    public class Role: IdentityRole
    {
        public ICollection<UserRole> Users { get; set; }
    }
}