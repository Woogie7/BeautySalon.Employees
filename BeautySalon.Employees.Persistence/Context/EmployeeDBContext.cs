using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using BeautySalon.Employees.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Employees.Persistence.Context
{
    public class EmployeeDBContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CustomDateOfWeek> CustomDateOfWeeks { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Availability> Availabilities { get; set; }

        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeDBContext).Assembly);
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            var customDateOfWeeks = Enumeration.GetAll<CustomDateOfWeek>();

            modelBuilder.Entity<CustomDateOfWeek>().HasData(customDateOfWeeks);
        }
    }
}
