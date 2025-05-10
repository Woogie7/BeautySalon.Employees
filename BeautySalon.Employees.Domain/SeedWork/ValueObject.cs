namespace BeautySalon.Employees.Domain.SeedWork
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if(obj is null || obj.GetType() != GetType())
            {
                return false;
            }

            var valueObj = obj as ValueObject;

            return this.GetEqualityComponents().SequenceEqual(valueObj.GetEqualityComponents());
        }

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        { 
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
             .Select(x => x != null ? x.GetHashCode() : 0)
             .Aggregate((x, y) => x ^ y);
        }
    }
}
