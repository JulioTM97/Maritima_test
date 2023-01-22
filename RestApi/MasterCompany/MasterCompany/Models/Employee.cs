namespace MasterCompany.Models
{
    public class Employee
    {
        public Employee() { }
        public Employee(string _name, string _lastName, string _document, long _salary, string _gender, string _position, string _startDate) { 
        this.Name = _name;
        this.LastName = _lastName;
        this.Document = _document;
        this.Salary = _salary;
        this.Gender = _gender;
        this.Position = _position;
        this.StartDate = _startDate;
        }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public double Salary { get; set; }
        public string Gender { get; set; }
        public string Position { get; set; }
        public string StartDate { get; set; }
    }
}