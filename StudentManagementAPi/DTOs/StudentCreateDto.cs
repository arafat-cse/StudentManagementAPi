﻿namespace StudentManagementAPi.DTOs
{
    public class StudentCreateDto
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gmail { get; set; }
        public string Address { get; set; }
        public IFormFile? Image { get; set; }
        public List<int> SubjectIds { get; set; } = new List<int>();
    }
}
