using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellnessDiaryApi.Data;
using WellnessDiaryApi.Models;
using WellnessDiaryApi.Data.Dto;

namespace WellnessDiaryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodPressuresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BloodPressuresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/BloodPressures
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<BloodPressure>>> GetBloodPressures()
        //{
        //  if (_context.BloodPressures == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.BloodPressures.ToListAsync();
        //}

        // GET: api/BloodPressures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BloodPressure>> GetBloodPressure(int id)
        {
          if (_context.BloodPressures == null)
          {
              return NotFound();
          }
            var bloodPressure = await _context.BloodPressures.FindAsync(id);

            if (bloodPressure == null)
            {
                return NotFound();
            }

            return bloodPressure;
        }

                
        // DELETE: api/BloodPressures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBloodPressure(int id)
        {
            if (_context.BloodPressures == null)
            {
                return NotFound();
            }
            var bloodPressure = await _context.BloodPressures.FindAsync(id);
            if (bloodPressure == null)
            {
                return NotFound();
            }

            _context.BloodPressures.Remove(bloodPressure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BloodPressureExists(int id)
        {
            return (_context.BloodPressures?.Any(e => e.ReadingId == id)).GetValueOrDefault();
        }

        /////////////////////////
        ///
        [HttpGet("{userId}/bloodPressures")]
        public async Task<ActionResult<IEnumerable<BloodPressure>>> GetUserBloodPressureList(int userId)
        {
            var userBPressureList = await _context.BloodPressures
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.RecordedDateTime)
                .ToListAsync();

            if (userBPressureList == null || userBPressureList.Count == 0)
            {
                return NotFound("No BMI records found for the user.");
            }

            return Ok(userBPressureList);
        }

        // POST: api/BloodPressures
        [HttpPost]
        public async Task<ActionResult<BloodPressureDTO>> PostBloodPressure(BloodPressureDTO bloodPressureDTO)
        {
            // Validate the incoming data
            if (bloodPressureDTO == null)
            {
                return BadRequest("Invalid blood pressure data.");
            }

            try
            {
                // Create BloodPressure entity from DTO
                var bloodPressure = new BloodPressure
                {
                    UserId = bloodPressureDTO.UserId,
                    Systolic = bloodPressureDTO.Systolic,
                    Diastolic = bloodPressureDTO.Diastolic,
                    RecordedDateTime = bloodPressureDTO.RecordedDateTime,
                    StatusId = bloodPressureDTO.StatusId
                };

                // Add blood pressure reading to the database
                _context.BloodPressures.Add(bloodPressure);
                await _context.SaveChangesAsync();

                // Determine blood pressure category and description
                (string category, string description) = GetBloodPressureCategory(bloodPressure.Systolic, bloodPressure.Diastolic);

                // Return the newly created blood pressure reading along with the category and description
                return CreatedAtAction("GetBloodPressure", new { id = bloodPressure.ReadingId },
                                       new { BloodPressure = bloodPressureDTO, Category = category, Description = description });

            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        // Method to determine blood pressure category
        private (string category, string description) GetBloodPressureCategory(decimal systolic, decimal diastolic)
        {
            string category = "";
            string description = "";

            if (systolic < 120 && diastolic < 80)
            {
                category = "Optimal";
                description = "Your blood pressure is optimal, which means it's in the ideal range. Keep up the good work!";
            }
            else if (systolic >= 120 && systolic <= 129 && diastolic < 80)
            {
                category = "Normal";
                description = "Your blood pressure is normal. It's slightly elevated, but still within the acceptable range.";
            }
            else if (systolic >= 130 && systolic <= 139 && diastolic >= 80 && diastolic <= 89)
            {
                category = "High-normal";
                description = "Your blood pressure is high-normal. It's above the normal range, indicating prehypertension. Consider lifestyle changes to lower it.";
            }
            else
            {
                category = "High";
                description = "Your blood pressure is high. You should consult a healthcare professional for further evaluation and treatment.";
            }

            return (category, description);
        }

        // PUT: api/BloodPressures/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBloodPressure(int id, BloodPressureDTO bloodPressureDTO)
        {
            if (id != bloodPressureDTO.ReadingId)
            {
                return BadRequest("ID mismatch.");
            }

            // Retrieve the existing blood pressure reading from the database
            var existingBloodPressure = await _context.BloodPressures.FindAsync(id);

            if (existingBloodPressure == null)
            {
                return NotFound();
            }

            try
            {
                // Update the blood pressure reading with the new values
                existingBloodPressure.UserId = bloodPressureDTO.UserId;
                existingBloodPressure.Systolic = bloodPressureDTO.Systolic;
                existingBloodPressure.Diastolic = bloodPressureDTO.Diastolic;
                existingBloodPressure.RecordedDateTime = bloodPressureDTO.RecordedDateTime;
                existingBloodPressure.StatusId = bloodPressureDTO.StatusId;

                _context.Entry(existingBloodPressure).State = EntityState.Modified;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Determine blood pressure category and description
                (string category, string description) = GetBloodPressureCategory(existingBloodPressure.Systolic, existingBloodPressure.Diastolic);

                // Return successful response along with blood pressure category and description
                return Ok(new { Message = "Blood pressure updated successfully.", Category = category, Description = description });
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // GET: api/BloodPressures/{userId}/bloodPressures
        [HttpGet("{userId}/bloodPressuresWithCategory")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserBloodPressureListWithXategory(int userId)
        {
            var userBPressureList = await _context.BloodPressures
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.RecordedDateTime)
                .ToListAsync();

            if (userBPressureList == null || userBPressureList.Count == 0)
            {
                return NotFound("No blood pressure records found for the user.");
            }

            // Create a list to store blood pressure readings along with their categories and descriptions
            var bloodPressureListWithCategory = new List<object>();

            foreach (var bloodPressure in userBPressureList)
            {
                // Determine blood pressure category and description
                (string category, string description) = GetBloodPressureCategory(bloodPressure.Systolic, bloodPressure.Diastolic);

                // Create an object containing blood pressure reading, category, and description
                var bloodPressureWithCategory = new
                {
                    ReadingId = bloodPressure.ReadingId,
                    UserId = bloodPressure.UserId,
                    Systolic = bloodPressure.Systolic,
                    Diastolic = bloodPressure.Diastolic,
                    RecordedDateTime = bloodPressure.RecordedDateTime,
                    StatusId = bloodPressure.StatusId,
                    Category = category,
                    Description = description
                };

                bloodPressureListWithCategory.Add(bloodPressureWithCategory);
            }

            return Ok(bloodPressureListWithCategory);
        }
    }
}
