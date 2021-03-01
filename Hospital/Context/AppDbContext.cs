using Hospital.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Doctors> Doctors { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        { }
    }
}
