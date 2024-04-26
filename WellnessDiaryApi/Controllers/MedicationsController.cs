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
    public class MedicationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicationsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Medications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medication>>> GetMedications()
        {
          if (_context.Medications == null)
          {
              return NotFound();
          }
            return await _context.Medications.ToListAsync();
        }

        // GET: api/Medications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medication>> GetMedication(int id)
        {
          if (_context.Medications == null)
          {
              return NotFound();
          }
            var medication = await _context.Medications.FindAsync(id);

            if (medication == null)
            {
                return NotFound();
            }

            return medication;
        }

        // PUT: api/Medications/5
        [HttpPut("{id}")]        
        public async Task<IActionResult> PutMedication(int id, MedicationDTO medicationDTO)
        {
            if (id != medicationDTO.MedicationId)
            {
                return BadRequest("ID mismatch between URL and body.");
            }

            var medication = await _context.Medications.FindAsync(id);

            if (medication == null)
            {
                return NotFound("Medication not found.");
            }
            medication.MedicationName=medicationDTO.MedicationName;
            medication.EndDate = medicationDTO.EndDate;
            medication.StartDate = medicationDTO.StartDate;
            medication.Frequency = medicationDTO.Frequency;


            try
            {
                await _context.SaveChangesAsync();
                return Ok("Medication successfully updated.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicationExists(id))
                {
                    return NotFound("Medication not found.");

                    
                }
                else
                {
                    throw; 
                }
            }

            //return NoContent();
        }


        // POST: api/Medications
        [HttpPost]        
        public async Task<ActionResult<SymptomDTO>> PostMedication(MedicationDTO medicationDTO)
        {
            if (medicationDTO == null || medicationDTO.UserId == null || string.IsNullOrWhiteSpace(medicationDTO.MedicationName))
            {
                return BadRequest("Invalid medication data provided.");
            }

            var medication = new Medication
            {
                UserId = medicationDTO.UserId.Value,
                MedicationName = medicationDTO.MedicationName,
                Dosage = medicationDTO.Dosage,
                Frequency = medicationDTO.Frequency,
                StartDate = medicationDTO.StartDate,
                EndDate = medicationDTO.EndDate
            };

            _context.Medications.Add(medication);

            try
            {
                await _context.SaveChangesAsync();

                return Ok("Medication successfully added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the medication record: {ex.Message}");
            }
        }

        // DELETE: api/Medications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            if (_context.Medications == null)
            {
                return NotFound();
            }
            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //GET: api/Medications/1/medications
        [HttpGet("{userId}/symptoms")]
        public async Task<ActionResult<IEnumerable<MedicationDTO>>> GetUserMedication(int userId)
        {
            var userMedication = await _context.Medications
                .Where(s => s.UserId == userId)
                .ToListAsync();

            if (userMedication == null || userMedication.Count == 0)
            {
                return NotFound("No Medication found for the user.");
            }

            var symptomDTOs = userMedication.Select(s => new MedicationDTO
            {
                MedicationId = s.MedicationId,
                UserId = s.UserId,
                MedicationName = s.MedicationName,
                Frequency = s.Frequency,
                StartDate = s.StartDate,
                EndDate = s.EndDate
            }).ToList();

            return Ok(symptomDTOs);
        }



        private bool MedicationExists(int id)
        {
            return (_context.Medications?.Any(e => e.MedicationId == id)).GetValueOrDefault();
        }
    }
}
