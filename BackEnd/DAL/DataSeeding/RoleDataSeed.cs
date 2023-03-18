using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataSeeding
{
    public static class RoleDataSeed
    {
        public static void SeedRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasData(
                    new Role() { IdRole = "o4kINXogbG", Name = "Admin" },
                    new Role() { IdRole = "L5uojNlToi", Name = "User" }
                );
        }
    }
}
