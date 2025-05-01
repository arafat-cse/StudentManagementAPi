namespace StudentManagementAPi.DTOs
{
    public class StudentDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gmail { get; set; }
        public string Address { get; set; }
        public string? ImagePath { get; set; }
        public List<int> SubjectIds { get; set; } = new List<int>();
        public List<string> SubjectNames { get; set; } = new List<string>();
    }
}
