using System;
using System.Collections.Generic;
using CheckInService.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckInService.Repository;

public partial class CheckInDbContext : DbContext
{
    public CheckInDbContext()
    {
    }

    public CheckInDbContext(DbContextOptions<CheckInDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CheckIn> CheckIns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=(local)\\sqlexpress; database=CheckInDb; integrated security=sspi; trustservercertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CheckIn>(entity =>
        {
            entity.HasKey(e => e.CheckInId).HasName("PK__CheckIn__E6497684A74764D1");

            entity.ToTable("CheckIn");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
