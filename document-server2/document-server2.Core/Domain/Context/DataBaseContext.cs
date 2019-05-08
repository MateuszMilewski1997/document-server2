using Microsoft.EntityFrameworkCore;

namespace document_server2.Core.Domain.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(column =>
            {
                column.Property(name => name.Login)
                      .HasColumnType("nvarchar(20)")
                      .IsRequired(true);
                column.Property(name => name.Password)
                      .HasColumnType("nvarchar(20)")
                      .IsRequired(true);
                column.Property(name => name.Email)
                      .HasColumnType("nvarchar(40)")
                      .IsRequired(true);
                column.Property(name => name.Role_id)
                      .HasColumnType("int")
                      .IsRequired(true);
            });

            modelBuilder.Entity<Role>(column =>
            {
                column.Property(name => name.Name)
                      .HasColumnType("nvarchar(30)")
                      .IsRequired(true);
            });

            modelBuilder.Entity<User>()
                        .HasOne(a => a.Role)
                        .WithMany()
                        .HasForeignKey(x => x.Role_id)
                        .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<User> Users { get; set; }
    }
}
