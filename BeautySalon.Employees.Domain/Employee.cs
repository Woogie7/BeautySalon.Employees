using BeautySalon.Employees.Domain.SeedWork;
using BeautySalon.Employees.Domain.ValueObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeautySalon.Employees.Domain
{
public class Employee : Entity
{
    private readonly List<Schedule> _schedules = new();
    private readonly List<Skill> _skills = new();
    private readonly List<Availability> _availabilities = new();

    public Guid Id { get; private set; }
    public FullName Name { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber Phone { get; private set; }

    public EmployeeStatus Status { get; private set; }
    public int StatusId { get; private set; }
    
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<Schedule> Schedules => _schedules.AsReadOnly();
    public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();
    public IReadOnlyCollection<Availability> Availabilities => _availabilities.AsReadOnly();

    private Employee() { } // For EF

    private Employee(Guid id, FullName name, Email email, PhoneNumber phone, EmployeeStatus status)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        Status = status;
    }

    public static Employee Create(Guid id, FullName name, Email email, PhoneNumber phone, EmployeeStatus status)
    {
        return new Employee(id, name, email, phone, status);
    }
    
    public void AddSchedule(Schedule schedule)
    {
        if (_schedules.Any(s =>
            s.DateOfWeek == schedule.DateOfWeek &&
            s.OverlapsWith(schedule.StartTime, schedule.EndTime)))
        {
            throw new InvalidOperationException("Overlapping schedule not allowed.");
        }

        _schedules.Add(schedule);
    }

    public void RemoveSchedule(Guid scheduleId)
    {
        var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
        if (schedule != null)
        {
            _schedules.Remove(schedule);
        }
    }

    public void UpdateName(string first, string last) => Name = new FullName(first, last);
    public void UpdatePhone(string phone) => Phone = new PhoneNumber(phone);
    public void UpdateEmail(string email) => Email = new Email(email);
    public void UpdateStatus(int statusId) => StatusId = statusId;  

    public void AddSkill(string skillName)
    {
        if (!_skills.Any(s => s.Name == skillName))
            _skills.Add(Skill.Create(Id, skillName));
    }

    public void RemoveSkill(string skillName)
    {
        var skill = _skills.FirstOrDefault(s => s.Name == skillName);
        if (skill != null)
            _skills.Remove(skill);
    }

    public void ChangeStatus(EmployeeStatus newStatus) => Status = newStatus;
    
    public void MarkAllSchedulesUnavailable()
    {
        foreach (var schedule in Schedules)
        {
            schedule.MarkUnavailable();
        }
    }

    
    public void Deactivate()
    {
        IsActive = false;
    }
}

    public class CustomDateOfWeek
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
