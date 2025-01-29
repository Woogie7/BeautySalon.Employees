using BeautySalon.Employees.Domain;
using BeautySalon.Employees.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace BeautySalon.Employees.Persistence.Context
{
    internal class EmployeeDBContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
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

            var status = Enum
                    .GetValues<EmployeeStatusEnum>()
                    .Select(r => new EmployeeStatus
                    {
                        Id = (int)r,
                        Name = r.ToString()
                    });

            modelBuilder.Entity<EmployeeStatus>().HasData(status);

            var dateOfweek = Enum
                    .GetValues<CustomDateOfWeekEnum>()
                    .Select(r => new CustomDateOfWeek
                    {
                        Id = (int)r,
                        Name = r.ToString()
                    });

            modelBuilder.Entity<CustomDateOfWeek>().HasData(dateOfweek);
        }
    }
}
