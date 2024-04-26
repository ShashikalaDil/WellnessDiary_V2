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
    public class BmisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BmisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Bmis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bmi>>> GetBmis()
        {
          if (_context.Bmis == null)
          {
              return NotFound();
          }
            return await _context.Bmis.ToListAsync();
        }

        // GET: api/Bmis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bmi>> GetBmi(int id)
        {
          if (_context.Bmis == null)
          {
              return NotFound();
          }
            var bmi = await _context.Bmis.FindAsync(id);

            if (bmi == null)
            {
                return NotFound();
            }

            return bmi;
        }

        [HttpGet("{userId}/bmi")]
        public async Task<ActionResult<IEnumerable<Bmi>>> GetUserBmiList(int userId)
        {
            var userBmiList = await _context.Bmis
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.RecordedDateTime)
                .ToListAsync();

            if (userBmiList == null || userBmiList.Count == 0)
            {
                return NotFound("No BMI records found for the user.");
            }

            return Ok(userBmiList);
        }

        
        // PUT: api/Bmis/5        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBmi(int id, BmiDTOUpdate bmiDTOUpdate)
        {
            if (id != bmiDTOUpdate.ReadingId)
            {
                return BadRequest();
            }

            var bmi = await _context.Bmis.FindAsync(id);

            if (bmi == null)
            {
                return NotFound();
            }

            // Calculate BMI
            decimal bmiValue = CalculateBMI(bmiDTOUpdate.Weight, bmiDTOUpdate.Height);

            // Update height, weight, and BMI properties
            bmi.Height = bmiDTOUpdate.Height;
            bmi.Weight = bmiDTOUpdate.Weight;
            bmi.Bmivalue = bmiValue;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BmiExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Determine message based on BMI value
            //string message = GetMessageForBMI(bmiValue);
            var (category, message) = GetMessageForBMI(bmiValue);

            // Return message along with HTTP response
            //return Ok(new { Message = message });
            return Ok(new { Bmi = bmi, Category = category, Message = message });
        }


        // POST: api/Bmis    
        [HttpPost]
        public async Task<ActionResult<Bmi>> PostBmi(BmiDTOUpdate bmiDTOUpdate)
        {
            if (bmiDTOUpdate == null || bmiDTOUpdate.Weight <= 0 || bmiDTOUpdate.Height <= 0)
            {
                return BadRequest("Invalid height or weight provided.");
            }

            decimal bmiValue = CalculateBMI(bmiDTOUpdate.Weight, bmiDTOUpdate.Height);

            //string message = GetMessageForBMI(bmiValue);
            var (category, message) = GetMessageForBMI(bmiValue);

            var bmi = new Bmi
            {
                UserId = bmiDTOUpdate.UserId,
                Weight = bmiDTOUpdate.Weight,
                Height = bmiDTOUpdate.Height,
                Bmivalue = bmiValue,
                RecordedDateTime = DateTime.Now,
                //Message = message
            };
                        
            _context.Bmis.Add(bmi);

            try
            {
                await _context.SaveChangesAsync();

                //return Ok(new { Bmi = bmi, Message = message });
                return Ok(new { Bmi = bmi, Category = category, Message = message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the BMI record: {ex.Message}");
            }
        }




        // DELETE: api/Bmis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBmi(int id)
        {
            if (_context.Bmis == null)
            {
                return NotFound();
            }
            var bmi = await _context.Bmis.FindAsync(id);
            if (bmi == null)
            {
                return NotFound();
            }

            _context.Bmis.Remove(bmi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BmiExists(int id)
        {
            return (_context.Bmis?.Any(e => e.ReadingId == id)).GetValueOrDefault();
        }

        // Helper method to calculate BMI
        private decimal CalculateBMI(decimal? weightInKg, decimal? heightInCm)
        {
            if (weightInKg.HasValue && heightInCm.HasValue && heightInCm != 0)
            {
                // Convert height from cm to meters
                decimal heightInMeters = heightInCm.Value / 100;

                // Calculate BMI
                return weightInKg.Value / (heightInMeters * heightInMeters);
            }

            return 0;
        }

        
        private (string category, string message) GetMessageForBMI(decimal bmi)
        {
            // Define your message conditions based on BMI ranges
            if (bmi < 18.5m)
            {
                return ("Underweight", "It's important to focus on a balanced diet and regular exercise to gain healthy weight.");
            }
            else if (bmi < 25)
            {
                return ("Normal weight", "Keep maintaining a balanced diet and staying active to sustain your health.");
            }
            else if (bmi < 30)
            {
                return ("Overweight", "Consider consulting a healthcare professional for guidance on weight management and adopting a healthier lifestyle.");
            }
            else
            {
                return ("Obese", "It's crucial to prioritize weight management for your health. Consult a healthcare professional for personalized advice and support.");
            }
        }

        // GET: api/Bmi/{userId}/bmi
        [HttpGet("{userId}/bmiwithCategory")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserBmiListWithCategory(int userId)
        {
            var userBmiList = await _context.Bmis
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.RecordedDateTime)
                .ToListAsync();

            if (userBmiList == null || userBmiList.Count == 0)
            {
                return NotFound("No BMI records found for the user.");
            }

            // Create a list to store BMI readings along with their categories and messages
            var bmiListWithCategory = new List<object>();

            foreach (var bmiRecord in userBmiList)
            {
                // Calculate BMI
                decimal bmiValue = CalculateBMI(bmiRecord.Weight, bmiRecord.Height);

                // Get BMI category and message
                (string category, string message) = GetMessageForBMI(bmiValue);

                // Create an object containing BMI record properties along with the category and message
                var bmiWithCategory = new
                {
                    UserId = bmiRecord.UserId,
                    WeightInKg = bmiRecord.Weight,
                    HeightInCm = bmiRecord.Height,
                    RecordedDateTime = bmiRecord.RecordedDateTime,
                    BMI = bmiValue,
                    Category = category,
                    Message = message
                };

                bmiListWithCategory.Add(bmiWithCategory);
            }

            return Ok(bmiListWithCategory);
        }
    }
}
