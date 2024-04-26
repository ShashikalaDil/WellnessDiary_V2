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
    public class SymptomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SymptomsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Symptoms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Symptom>>> GetSymptoms()
        {
          if (_context.Symptoms == null)
          {
              return NotFound();
          }
            return await _context.Symptoms.ToListAsync();
        }

        // GET: api/Symptoms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Symptom>> GetSymptom(int id)
        {
          if (_context.Symptoms == null)
          {
              return NotFound();
          }
            var symptom = await _context.Symptoms.FindAsync(id);

            if (symptom == null)
            {
                return NotFound();
            }

            return symptom;
        }

        // PUT: api/Symptoms/5   
        [HttpPut("{id}")]
        public async Task<ActionResult<SymptomDTO>> PutSymptom(int id, SymptomDTO symptomDTO)
        {
            if (symptomDTO == null || symptomDTO.UserId == null || string.IsNullOrWhiteSpace(symptomDTO.SymptomName))
            {
                return BadRequest("Invalid symptom data provided.");
            }

            var existingSymptom = await _context.Symptoms.FindAsync(id);

            if (existingSymptom == null)
            {
                return NotFound("Symptom not found.");
            }

            existingSymptom.SymptomName = symptomDTO.SymptomName;
            existingSymptom.Severity = symptomDTO.Severity;
            existingSymptom.RecordedDateTime = symptomDTO.RecordedDateTime ?? DateTime.UtcNow;
            existingSymptom.Notes = symptomDTO.Notes;

            try
            {
                await _context.SaveChangesAsync();

                return Ok("Symptom successfully updated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the symptom record: {ex.Message}");
            }
        }



        // POST: api/Symptoms     

        [HttpPost]
        public async Task<ActionResult<SymptomDTO>> PostSymptom(SymptomDTO symptomDTO)
        {
            if (symptomDTO == null || symptomDTO.UserId == null || string.IsNullOrWhiteSpace(symptomDTO.SymptomName))
            {
                return BadRequest("Invalid symptom data provided.");
            }

            // You can add further validation as needed

            var symptom = new Symptom
            {
                UserId = symptomDTO.UserId.Value,
                SymptomName = symptomDTO.SymptomName,
                Severity = symptomDTO.Severity,
                RecordedDateTime = symptomDTO.RecordedDateTime ?? DateTime.UtcNow,
                Notes = symptomDTO.Notes
            };

            _context.Symptoms.Add(symptom);

            try
            {
                await _context.SaveChangesAsync();

                //return Ok(symptomDTO);
                return Ok("Symptom successfully saved.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the symptom record: {ex.Message}");
            }
        }

        //GET: api/Symptoms/1/symptoms
        [HttpGet("{userId}/symptoms")]
        public async Task<ActionResult<IEnumerable<SymptomDTO>>> GetUserSymptoms(int userId)
        {
            var userSymptoms = await _context.Symptoms
                .Where(s => s.UserId == userId)
                .ToListAsync();

            if (userSymptoms == null || userSymptoms.Count == 0)
            {
                return NotFound("No symptoms found for the user.");
            }

            var symptomDTOs = userSymptoms.Select(s => new SymptomDTO
            {
                SymptomId = s.SymptomId,
                UserId = s.UserId,
                SymptomName = s.SymptomName,
                Severity = s.Severity,
                RecordedDateTime = s.RecordedDateTime,
                Notes = s.Notes
            }).ToList();

            return Ok(symptomDTOs);
        }



        // DELETE: api/Symptoms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSymptom(int id)
        {
            if (_context.Symptoms == null)
            {
                return NotFound();
            }
            var symptom = await _context.Symptoms.FindAsync(id);
            if (symptom == null)
            {
                return NotFound();
            }

            _context.Symptoms.Remove(symptom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SymptomExists(int id)
        {
            return (_context.Symptoms?.Any(e => e.SymptomId == id)).GetValueOrDefault();
        }
    }
}
