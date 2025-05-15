using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Domain;

public class Service : Entity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public TimeSpan Duration { get; private set; }
    public decimal Price { get; private set; }

    private Service()
    {
    }

    private Service(string name, string description, TimeSpan duration, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Duration = duration;
        Price = price;
    }

    public static Service Create(string name, string description, TimeSpan duration, decimal price)
    {
        return new Service(name, description, duration, price);
    }
    public void Update(string name, string description, TimeSpan duration, decimal price)
    {
        Name = name;
        Description = description;
        Duration = duration;
        Price = price;
    }

}
    
