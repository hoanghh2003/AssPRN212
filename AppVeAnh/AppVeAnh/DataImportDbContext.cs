using System;
using System.Collections.Generic;
using AppVeAnh.Entities;
using Microsoft.EntityFrameworkCore;

namespace AppVeAnh;

public partial class DataImportDbContext : DbContext
{
    public DataImportDbContext()
    {
    }

    public DataImportDbContext(DbContextOptions<DataImportDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ImportedDatum> ImportedData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database= DataImportDB;UID=sa;PWD=1;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ImportedDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Imported__3214EC07188288F0");

            entity.Property(e => e.Column1).HasMaxLength(255);
            entity.Property(e => e.Column2).HasMaxLength(255);
            entity.Property(e => e.Column3).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
