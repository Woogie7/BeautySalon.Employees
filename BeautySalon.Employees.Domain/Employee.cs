using BeautySalon.Employees.Domain.SeedWork;
using BeautySalon.Employees.Domain.ValueObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BeautySalon.Employees.Domain
{
public class Employee : Entity
{
    private readonly List<Schedule> _schedules = new();
    private readonly List<Skill> _skills = new();

    public Guid Id { get; private set; }
    public FullName Name { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber Phone { get; private set; }
    
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<Schedule> Schedules => _schedules.AsReadOnly();
    public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();

    private Employee() { } // For EF

    private Employee(Guid id, FullName name, Email email, PhoneNumber phone)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
    }

    public static Employee Create(Guid id, FullName name, Email email, PhoneNumber phone)
    {
        return new Employee(id, name, email, phone);
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
    
    public bool IsAvailableForBooking(DateTime startTime, DateTime endTime)
    {
        var start = startTime.TimeOfDay;
        var end = endTime.TimeOfDay;

        foreach (var schedule in Schedules)
        {
            if (schedule.IsAvailable && schedule.OverlapsWith(start, end))
            {
                return false;
            }
        }

        return true;
    }
    
    public void BookTime(DateTime startTime, DateTime endTime)
    {
        if (IsAvailableForBooking(startTime, endTime))
        {
            foreach (var schedule in Schedules)
            {
                if (schedule.OverlapsWith(startTime.TimeOfDay, endTime.TimeOfDay))
                {
                    schedule.MarkUnavailable();
                }
            }
        }
        else
        {
            throw new InvalidOperationException("Employee is not available at the selected time.");
        }
    }
    
    public void CancelBooking(DateTime startTime, DateTime endTime)
    {
        foreach (var schedule in Schedules)
        {
            if (schedule.OverlapsWith(startTime.TimeOfDay, endTime.TimeOfDay))
            {
                schedule.MarkAvailable();
            }
        }
    }

    public void UpdateName(string first, string last) => Name = new FullName(first, last);
    public void UpdatePhone(string phone) => Phone = new PhoneNumber(phone);
    public void UpdateEmail(string email) => Email = new Email(email);

    public void AddSkill(Guid serviceId)
    {
        if (!_skills.Any(s => s.ServiceId == serviceId))
            _skills.Add(Skill.Create(Id, serviceId));
    }

    public void RemoveSkill(Guid serviceId)
    {
        var skill = _skills.FirstOrDefault(s => s.ServiceId == serviceId);
        if (skill != null)
            _skills.Remove(skill);
    }
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
