using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPi.Data;
using StudentManagementAPi.DTOs;
using StudentManagementAPi.Models;
namespace StudentManagementAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public StudentsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //// GET: api/Students
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        //{
        //    var students = await _context.Students
        //        .Include(s => s.StudentSubjects)
        //        .ThenInclude(ss => ss.Subject)
        //        .Select(s => new StudentDto
        //        {
        //            UserId = s.UserId,
        //            UserName = s.UserName,
        //            PhoneNumber = s.PhoneNumber,
        //            Gmail = s.Gmail,
        //            Address = s.Address,
        //            ImagePath = s.ImagePath,
        //            SubjectIds = s.StudentSubjects.Select(ss => ss.SubjectId).ToList(),
        //            SubjectNames = s.StudentSubjects.Select(ss => ss.Subject.SubjectName).ToList()
        //        })
        //        .ToListAsync();

        //    return students;
        //}
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}/"; // ✅ http://localhost:5000/

            var students = await _context.Students
                .Include(s => s.StudentSubjects)
                .ThenInclude(ss => ss.Subject)
                .Select(s => new StudentDto
                {
                    UserId = s.UserId,
                    UserName = s.UserName,
                    PhoneNumber = s.PhoneNumber,
                    Gmail = s.Gmail,
                    Address = s.Address,
                    ImagePath = string.IsNullOrEmpty(s.ImagePath)? null: baseUrl + s.ImagePath.Replace("\\", "/"), // ✅ Full public URL
                    SubjectIds = s.StudentSubjects.Select(ss => ss.SubjectId).ToList(),
                    SubjectNames = s.StudentSubjects.Select(ss => ss.Subject.SubjectName).ToList()
                })
                .ToListAsync();

            return students;
        }


        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentSubjects)
                .ThenInclude(ss => ss.Subject)
                .Where(s => s.UserId == id)
                .Select(s => new StudentDto
                {
                    UserId = s.UserId,
                    UserName = s.UserName,
                    PhoneNumber = s.PhoneNumber,
                    Gmail = s.Gmail,
                    Address = s.Address,
                    ImagePath = s.ImagePath,
                    SubjectIds = s.StudentSubjects.Select(ss => ss.SubjectId).ToList(),
                    SubjectNames = s.StudentSubjects.Select(ss => ss.Subject.SubjectName).ToList()
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudent([FromForm] StudentCreateDto studentDto)
        {
            var student = new Student
            {
                UserName = studentDto.UserName,
                PhoneNumber = studentDto.PhoneNumber,
                Gmail = studentDto.Gmail,
                Address = studentDto.Address
            };

            // Process image upload
            if (studentDto.Image != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + studentDto.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await studentDto.Image.CopyToAsync(fileStream);
                }

                student.ImagePath = "/images/" + uniqueFileName;
            }

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Add subject relationships
            if (studentDto.SubjectIds != null && studentDto.SubjectIds.Any())
            {
                foreach (var subjectId in studentDto.SubjectIds)
                {
                    _context.StudentSubjects.Add(new StudentSubject
                    {
                        UserId = student.UserId,
                        SubjectId = subjectId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return await GetStudent(student.UserId);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, [FromForm] StudentUpdateDto studentDto)
        {
            if (id != studentDto.UserId)
            {
                return BadRequest();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.UserName = studentDto.UserName;
            student.PhoneNumber = studentDto.PhoneNumber;
            student.Gmail = studentDto.Gmail;
            student.Address = studentDto.Address;

            // Process image upload
            if (studentDto.Image != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(student.ImagePath))
                {
                    string oldFilePath = Path.Combine(_environment.WebRootPath, student.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + studentDto.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await studentDto.Image.CopyToAsync(fileStream);
                }

                student.ImagePath = "/images/" + uniqueFileName;
            }

            // Update subject relationships
            var existingRelations = await _context.StudentSubjects
                .Where(ss => ss.UserId == id)
                .ToListAsync();

            // Delete relations not in the updated list
            foreach (var relation in existingRelations)
            {
                if (!studentDto.SubjectIds.Contains(relation.SubjectId))
                {
                    _context.StudentSubjects.Remove(relation);
                }
            }

            // Add new relations
            foreach (var subjectId in studentDto.SubjectIds)
            {
                if (!existingRelations.Any(r => r.SubjectId == subjectId))
                {
                    _context.StudentSubjects.Add(new StudentSubject
                    {
                        UserId = id,
                        SubjectId = subjectId
                    });
                }
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Delete image if exists
            if (!string.IsNullOrEmpty(student.ImagePath))
            {
                string filePath = Path.Combine(_environment.WebRootPath, student.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("GetSubjects")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            return await _context.Subjects.ToListAsync();
        }

        [HttpGet("GetAllStudents")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            return await GetStudents();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.UserId == id);
        }
    }
}
