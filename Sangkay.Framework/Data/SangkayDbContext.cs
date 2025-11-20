// Basser.Framework/Data/BasserDbContext.cs
using Sangkay.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Sangkay.Framework.Data
{
    public class SangkayDbContext : DbContext
    {
        public SangkayDbContext(DbContextOptions<SangkayDbContext> options)
            : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.Property(x => x.Address).HasMaxLength(500);
                b.Property(x => x.Contact).HasMaxLength(100);
            });
        }
    }
}

