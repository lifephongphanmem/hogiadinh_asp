using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLHN.Models.Systems;

namespace QLHN.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //Systems
        public DbSet<SystemInFo> SystemInFo { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Cities> Cities { get; set; }
        public DbSet<Districts> Districts { get; set; }
        public DbSet<Towns> Towns { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolesAction> RolesAction { get; set; }
        //End Systems
    }
}
