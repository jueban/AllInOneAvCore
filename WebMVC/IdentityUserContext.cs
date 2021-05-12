using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC
{
    public class IdentityUserContext : IdentityDbContext<AppUser>
    {
        public IdentityUserContext(DbContextOptions options) : base(options)
        {

        }
    }
}
