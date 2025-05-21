using BeautySalon.Employees.Domain.Enum;
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

        public bool IsActive { get; private set; }

        public IReadOnlyCollection<Schedule> Schedules => _schedules.AsReadOnly();
        public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();
        public IReadOnlyCollection<Availability> Availabilities => _availabilities.AsReadOnly();

        private Employee()
        {
        }

        private Employee(Guid id, FullName name, Email email, PhoneNumber phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
        }

        public static Employee Create(Guid id, FullName name, Email email, PhoneNumber phone)
        {
            return new Employee(id, name, email, phone)
            {
                IsActive = true
            };
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
        
        public void AddAvailability(Availability availability)
        {
            if (_availabilities.Any(a =>
                    a.StartTime < availability.EndTime &&
                    a.EndTime > availability.StartTime))
            {
                throw new InvalidOperationException("Overlapping availability");
            }

            _availabilities.Add(availability);
        }

        public Availability? RemoveAvailability(DateTime startTime, TimeSpan duration)
        {
            var availability = _availabilities.FirstOrDefault(a => a.StartTime == startTime && a.EndTime == startTime + duration);
            if (availability != null)
            {
                _availabilities.Remove(availability);
            }
            return availability;
        }




        public bool IsAvailableForBooking(DateTime startTime, DateTime endTime)
        {
            var dayOfWeek = startTime.DayOfWeek;
            var start = startTime.TimeOfDay;
            var end = endTime.TimeOfDay;

            return _schedules.Any(schedule =>
                schedule.DateOfWeek == Enumeration.FromDisplayName<CustomDateOfWeek>(dayOfWeek.ToString()) &&
                schedule.IsAvailable &&
                schedule.Contains(start, end));
        }


        public void BookTime(DateTime startTime, DateTime endTime)
        {
            var dayOfWeek = startTime.DayOfWeek;
            var start = startTime.TimeOfDay;
            var end = endTime.TimeOfDay;

            var schedule = _schedules.FirstOrDefault(s =>
                s.DateOfWeek == Enumeration.FromDisplayName<CustomDateOfWeek>(dayOfWeek.ToString()) &&
                s.Contains(start, end) &&
                s.IsAvailable);

            if (schedule == null)
                throw new InvalidOperationException("No available schedule found for this time.");

            schedule.MarkUnavailable();
        }


        public void CancelBooking(DateTime startTime, DateTime endTime)
        {
            var dayOfWeek = startTime.DayOfWeek;
            var start = startTime.TimeOfDay;
            var end = endTime.TimeOfDay;

            var schedule = _schedules.FirstOrDefault(s =>
                s.DateOfWeek == Enumeration.FromDisplayName<CustomDateOfWeek>(dayOfWeek.ToString()) &&
                s.Contains(start, end) &&
                !s.IsAvailable);

            if (schedule != null)
            {
                schedule.MarkAvailable();
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
}
