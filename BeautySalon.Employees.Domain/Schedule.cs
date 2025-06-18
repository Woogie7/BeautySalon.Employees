using BeautySalon.Employees.Domain.Enum;
using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Domain;

public class Schedule : Entity
{
    public Guid Id { get; private set; }
    public CustomDateOfWeek DateOfWeek { get; private set; }
    public int DateOfWeekId { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public bool IsAvailable { get; private set; }

    private Schedule() { }

    public Schedule(Guid id, CustomDateOfWeek dateOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        Id = id;
        DateOfWeek = dateOfWeek;
        DateOfWeekId = dateOfWeek.Id;
        StartTime = startTime;
        EndTime = endTime;
        IsAvailable = true;
    }

    public static Schedule Create(Guid id, CustomDateOfWeek dateOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        return new Schedule(id, dateOfWeek, startTime, endTime);
    }


    public bool OverlapsWith(TimeSpan otherStart, TimeSpan otherEnd)
    {
        return !(otherEnd <= StartTime || otherStart >= EndTime);
    }
    public bool Contains(TimeSpan start, TimeSpan end)
    {
        return StartTime <= start && EndTime >= end;
    }

    public void MarkUnavailable() => IsAvailable = false;
    public void MarkAvailable() => IsAvailable = true;
    
    public void UpdateSchedule(CustomDateOfWeek dateOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        if (DateOfWeekId != dateOfWeek.Id)
        {
            DateOfWeek = dateOfWeek;
            DateOfWeekId = dateOfWeek.Id;
        }
        
        if (StartTime != startTime)
        {
            StartTime = startTime;
        }

        if (EndTime != endTime)
        {
            EndTime = endTime;
        }
    }
    
    

}
