using BeautySalon.Employees.Domain.SeedWork;

namespace BeautySalon.Employees.Domain.Enum
{
    public class CustomDateOfWeek : Enumeration
    {
        public static readonly CustomDateOfWeek Monday = new (1, nameof(Monday));
        public static readonly CustomDateOfWeek Tuesday  = new (2, nameof(Tuesday));
        public static readonly CustomDateOfWeek Wednesday  = new (3, nameof(Wednesday));
        public static readonly CustomDateOfWeek Thursday   = new (4, nameof(Thursday));
        public static readonly CustomDateOfWeek Friday   = new (5, nameof(Friday));
        public static readonly CustomDateOfWeek Saturday   = new (6, nameof(Saturday ));
        public static readonly CustomDateOfWeek Sunday  = new (7, nameof(Sunday ));

        protected CustomDateOfWeek(int id, string name) : base(id, name)
        {
        }
    }
}
