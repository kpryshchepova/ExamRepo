namespace EmployeesApp.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int EmployeeId { get; set; }

        public Todo() { }

        public Todo(int id, string name, string description, bool isCompleted, int employeeId)
        {
            Id = id;
            Name = name;
            Description = description;
            IsCompleted = isCompleted;
            EmployeeId = employeeId;
        }
    }
}
