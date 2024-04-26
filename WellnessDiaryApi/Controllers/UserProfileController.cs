using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellnessDiaryApi.Data;
using WellnessDiaryApi.Data.Dto;
using WellnessDiaryApi.Models;

namespace WellnessDiaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        //private readonly IUserService userService;

        public UserProfileController(AppDbContext context)
        {
            _context = context;
        }

               
        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }


        // GET: api/UserProfile/5        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDTO>> GetUser(int id)
        {
            
            var userData = await _context.Users.FindAsync(id);

            if (userData == null)
            {
                return NotFound(); 
            }

            var userProfileDto = new UserProfileDTO
            {
                UserId = userData.UserId,
                Username = userData.Username,
                Email = userData.Email,
                Name = userData.Name,
                Age = userData.Age,
                Gender = userData.Gender,
                Address = userData.Address,
                ContactNumber = userData.ContactNumber
            };

            return userProfileDto;
        }

        // PUT: api/UserProfile/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserProfileDTO userProfileDto)
        {
            if (id != userProfileDto.UserId)
            {
                return BadRequest(); 
            }

            var userEntity = await _context.Users.FindAsync(id);
            if (userEntity == null)
            {
                return NotFound();  
            }
             
            userEntity.Username = userProfileDto.Username;
            userEntity.Email = userProfileDto.Email;
            userEntity.Name = userProfileDto.Name;
            userEntity.Age = userProfileDto.Age;
            userEntity.Gender = userProfileDto.Gender;
            userEntity.Address = userProfileDto.Address;
            userEntity.ContactNumber = userProfileDto.ContactNumber;

            try
            {
                await _context.SaveChangesAsync();  
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();  
                }
                else
                {
                    throw;  
                }
            }

            var successMessage = new { msg = "Success: User profile updated." };
            return Ok(successMessage);
        }

        
    }
}
