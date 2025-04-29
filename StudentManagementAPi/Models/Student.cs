using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPi.Models
{
    public class Student
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Gmail { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public string ImagePath { get; set; }

        // Navigation property
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
