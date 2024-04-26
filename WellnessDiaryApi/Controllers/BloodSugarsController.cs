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
    public class BloodSugarsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BloodSugarsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BloodSugars
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<BloodSugar>>> GetBloodSugars()
        //{
        //  if (_context.BloodSugars == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.BloodSugars.ToListAsync();
        //}

        // GET: api/BloodSugars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BloodSugar>> GetBloodSugar(int id)
        {
          if (_context.BloodSugars == null)
          {
              return NotFound();
          }
            var bloodSugar = await _context.BloodSugars.FindAsync(id);

            if (bloodSugar == null)
            {
                return NotFound();
            }

            return bloodSugar;
        }
        
        // DELETE: api/BloodSugars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBloodSugar(int id)
        {
            if (_context.BloodSugars == null)
            {
                return NotFound();
            }
            var bloodSugar = await _context.BloodSugars.FindAsync(id);
            if (bloodSugar == null)
            {
                return NotFound();
            }

            _context.BloodSugars.Remove(bloodSugar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BloodSugarExists(int id)
        {
            return (_context.BloodSugars?.Any(e => e.ReadingId == id)).GetValueOrDefault();
        }

        
        // POST: api/BloodSugars
        [HttpPost]
        public async Task<ActionResult<BloodSugarDTO>> PostBloodSugar(BloodSugarDTO bloodSugarDTO)
        {
            if (bloodSugarDTO == null)
            {
                return BadRequest("Invalid blood sugar data.");
            }

            try
            {
                var bloodSugar = new BloodSugar
                {
                    UserId = bloodSugarDTO.UserId,
                    BloodSugarLevel = bloodSugarDTO.BloodSugarLevel,
                    RecordedDateTime = bloodSugarDTO.RecordedDateTime,
                    StatusId = bloodSugarDTO.StatusId
                };

                _context.BloodSugars.Add(bloodSugar);
                await _context.SaveChangesAsync();

                (string category, string description) = GetBloodSugarCategory(bloodSugar.BloodSugarLevel);

                return CreatedAtAction("GetBloodSugar", new { id = bloodSugar.ReadingId },
                                       new { BloodSugar = bloodSugarDTO, Category = category, Description = description });
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // Method to determine blood sugar level category and description
        private (string category, string description) GetBloodSugarCategory(decimal bloodSugarLevel)
        {
            string category = "";
            string description = "";

            if (bloodSugarLevel < 70)
            {
                category = "Low";
                description = "Your blood sugar level is low. You may experience symptoms such as dizziness, weakness, or confusion. Consider consuming glucose-rich foods or drinks.";
            }
            else if (bloodSugarLevel >= 70 && bloodSugarLevel <= 130)
            {
                category = "Normal";
                description = "Your blood sugar level is normal. It's within the target range recommended for most adults.";
            }
            else
            {
                category = "High";
                description = "Your blood sugar level is high. This may indicate hyperglycemia, which can lead to complications if not managed properly. Consider consulting a healthcare professional.";
            }

            return (category, description);
        }

        // PUT: api/BloodSugars/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBloodSugar(int id, BloodSugarDTO bloodSugarDTO)
        {
            if (id != bloodSugarDTO.ReadingId)
            {
                return BadRequest("ID mismatch.");
            }

            var existingBloodSugar = await _context.BloodSugars.FindAsync(id);

            if (existingBloodSugar == null)
            {
                return NotFound();
            }

            try
            {
                existingBloodSugar.UserId = bloodSugarDTO.UserId;
                existingBloodSugar.BloodSugarLevel = bloodSugarDTO.BloodSugarLevel;
                existingBloodSugar.RecordedDateTime = bloodSugarDTO.RecordedDateTime;
                existingBloodSugar.StatusId = bloodSugarDTO.StatusId;

                _context.Entry(existingBloodSugar).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                (string category, string description) = GetBloodSugarCategory(bloodSugarDTO.BloodSugarLevel);

                return Ok(new { Message = "Blood sugar reading updated successfully.", Category = category, Description = description });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // GET: api/BloodSugars/{userId}/bloodSugarsWithCategory
        [HttpGet("{userId}/bloodSugarsWithCategory")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserBloodSugarListWithCategory(int userId)
        {
            var userBloodSugarList = await _context.BloodSugars
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.RecordedDateTime)
                .ToListAsync();

            if (userBloodSugarList == null || userBloodSugarList.Count == 0)
            {
                return NotFound("No blood sugar records found for the user.");
            }

            var bloodSugarListWithCategory = new List<object>();

            foreach (var bloodSugar in userBloodSugarList)
            {
                (string category, string description) = GetBloodSugarCategory(bloodSugar.BloodSugarLevel);

                var bloodSugarWithCategory = new
                {
                    ReadingId = bloodSugar.ReadingId,
                    UserId = bloodSugar.UserId,
                    BloodSugarLevel = bloodSugar.BloodSugarLevel,
                    RecordedDateTime = bloodSugar.RecordedDateTime,
                    StatusId = bloodSugar.StatusId,
                    Category = category,
                    Description = description
                };

                bloodSugarListWithCategory.Add(bloodSugarWithCategory);
            }

            return Ok(bloodSugarListWithCategory);
        }
    }
}
