namespace EmployeesApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Speciality { get; set; }
        public DateTime EmployementDate { get; set; }

        public Employee() { }

        public Employee(string name, int age, string speciality, DateTime employementDate) {
            Name = name;
            Age = age;
            Speciality = speciality;
            EmployementDate = employementDate;
        }
    }
    
}
