using System.Text.RegularExpressions;
using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; }

    public Email(string value)
    {
        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email format");
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents() => new[] { Value };
}

public class PhoneNumber : ValueObject
{
    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invalid phone");
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents() => new[] { Value };
}
