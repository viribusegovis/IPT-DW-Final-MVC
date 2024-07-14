using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quize.Models;
using System.Threading.Tasks;

namespace Quize.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersApiController : ControllerBase
    {
        private readonly QuizDbContext _context;

        public MembersApiController(QuizDbContext context)
        {
            _context = context;
        }

        // GET: api/MembersApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Members>>> GetMembers()
        {
            return await _context.Members
                .Select(m => new Members
                {
                    Id = m.Id,
                    Email = m.Email,
                    Username = m.Username
                })
                .ToListAsync();
        }

        // GET: api/MembersApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Members>> GetMember(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return new Members
            {
                Id = member.Id,
                Email = member.Email,
                Username = member.Username
            };
        }

        // POST: api/MembersApi/Login
        [HttpPost("Login")]
        public async Task<ActionResult<Members>> Login([FromBody] LoginDto loginDto)
        {
            Console.WriteLine(loginDto);

            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.Email == loginDto.Email && m.Password == loginDto.Password);
            if (member == null)
            {
                return NotFound("User not found or invalid credentials");
            }

            return new Members
            {
                Id = member.Id,
                Email = member.Email,
                Username = member.Username
            };
        }

        // PUT: api/MembersApi/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Members>> UpdateMember(int id, [FromBody] MemberUpdateDto memberUpdateDto)
        {
            if (id != memberUpdateDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var member = await _context.Members.FindAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            // Update only the fields that are provided
            if (!string.IsNullOrEmpty(memberUpdateDto.Email))
            {
                member.Email = memberUpdateDto.Email;
            }
            if (!string.IsNullOrEmpty(memberUpdateDto.Password))
            {
                member.Password = memberUpdateDto.Password;
            }
            if (!string.IsNullOrEmpty(memberUpdateDto.Username))
            {
                member.Username = memberUpdateDto.Username;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Return the updated member
            return new Members
            {
                Id = member.Id,
                Email = member.Email,
                Username = member.Username
            };
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class MemberUpdateDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
    }
}
