using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPi.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }

        [Required]
        [StringLength(100)]
        public string SubjectName { get; set; }

        // Navigation property
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
