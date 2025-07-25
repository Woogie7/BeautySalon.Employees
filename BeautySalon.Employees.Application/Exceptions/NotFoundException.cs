namespace BeautySalon.Employees.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message, Guid requestEmployeeId) : base(message) {}
    
    public NotFoundException(string message) : base(message) {}
}

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) {}
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}