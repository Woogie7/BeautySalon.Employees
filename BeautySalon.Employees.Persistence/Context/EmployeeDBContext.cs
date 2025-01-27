using BeautySalon.Employees.Domain;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Employees.Persistence.Context
{
    internal class EmployeeDBContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Skill> Skills{ get; set; }

        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Schedules)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); 

            
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Skills)
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
