using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeautySalon.Employees.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public EmployeeStatus Status { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Skill> Skills { get; set; }
    }

    public enum EmployeeStatus
    {
        Active,
        Inactive,
        OnVacation 
    }

    public enum CustomDateOfWeek
    {
        Monday = 0,    
        Tuesday = 1,   
        Wednesday = 2, 
        Thursday = 3,  
        Friday = 4,    
        Saturday = 5,  
        Sunday = 6    
    }

    public class Schedule
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public CustomDateOfWeek DateOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool IsAvailable { get ; set; }
    }

    public class Availability
    {
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsBusy { get; set; }
    }

    public class Skill
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Name { get; set; }

    }
}
