using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementAPi.Models
{
    public class StudentSubject
    {
        [Key]
        public int StudentSubjectId { get; set; }

        public int UserId { get; set; }

        public int SubjectId { get; set; }

        [ForeignKey("UserId")]
        public virtual Student Student { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
    }
}
