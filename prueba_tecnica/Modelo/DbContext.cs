using Microsoft.EntityFrameworkCore;

namespace prueba_tecnica.Modelo
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Seguridad> Seguridad { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seguridad>(entity =>
            {
                entity.ToTable("Seguridad", schema: "Seguridad");
                entity.HasKey(e => e.id);
                entity.Property(e => e.nombres).IsRequired().HasMaxLength(100);
                entity.Property(e => e.apellidos).IsRequired().HasMaxLength(100);
                entity.Property(e => e.fechanacimiento).IsRequired();
                entity.Property(e => e.direccion).IsRequired().HasMaxLength(255);
                entity.Property(e => e.password).IsRequired().HasMaxLength(255);
                entity.Property(e => e.telefono).IsRequired().HasMaxLength(15);
                entity.Property(e => e.email).IsRequired().HasMaxLength(320);
                entity.Property(e => e.fechacreacion).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }

}
