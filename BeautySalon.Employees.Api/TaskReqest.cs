namespace BeautySalon.Employees.Api
{
    public class TaskRequest
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string StartTime { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; }
    }
}
