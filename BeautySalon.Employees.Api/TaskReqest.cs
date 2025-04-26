namespace BeautySalon.Employees.Api
{
    public class TaskEntity
    {
        public Guid TaskId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public TaskStatus Status { get; set; }

        
    }
    public class CreateTaskDto
    {
        public Guid EmployeeId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
    }

    public class TaskDto
    {
        public Guid TaskId { get; set; }
        public Guid ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public TaskStatus Status { get; set; }
    }
    public interface ITaskRepository
    {
        Task<Task> GetByIdAsync(Guid taskId);
        Task<IEnumerable<TaskEntity>> GetByEmployeeIdAsync(Guid employeeId);
        Task AddAsync(TaskEntity task);
        Task UpdateAsync(TaskEntity task);
    }
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateTask(CreateTaskDto dto)
        {
            var task = new TaskEntity
            {
                TaskId = Guid.NewGuid(),
                EmployeeId = dto.EmployeeId,
                ProjectId = dto.ProjectId,
                StartTime = dto.StartTime,
                Duration = dto.Duration,
                Status = TaskStatus.Created
            };

            var adsa = await _repository.AddAsync(task);
            return task.TaskId;
        }

        public async Task<TaskDto> GetTaskById(Guid taskId)
        {
            var task = await _repository.GetByIdAsync(taskId);
            return new TaskDto
            {
                TaskId = task.TaskId,
                ProjectId = task.ProjectId,
                StartTime = task.StartTime,
                Duration = task.Duration,
                Status = task.Status
            };
        }
    }

    public interface ITaskService
    {
    }
}
