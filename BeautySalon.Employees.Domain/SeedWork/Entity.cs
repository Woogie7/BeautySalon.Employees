﻿namespace BeautySalon.Employees.Domain.SeedWork
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }

}
