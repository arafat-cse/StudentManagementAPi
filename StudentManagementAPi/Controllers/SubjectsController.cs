using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentManagementAPi.Data;
using StudentManagementAPi.DTOs;

namespace StudentManagementAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
        {
            return await _context.Subjects
                .Select(s => new SubjectDto
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName
                })
                .ToListAsync();
        }
    }
}
