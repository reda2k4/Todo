using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CC.ToDo.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Guid UserId { get; set; }

    }
}
