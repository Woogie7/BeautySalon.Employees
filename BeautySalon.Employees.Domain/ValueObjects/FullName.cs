using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Domain.ValueObjects;

public class FullName : ValueObject
{
    public string First { get; }
    public string Last { get; }

    public FullName(string first, string last)
    {
        if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
            throw new ArgumentException("Name cannot be empty");

        First = first;
        Last = last;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return First;
        yield return Last;
    }

    public override string ToString() => $"{First} {Last}";
}
