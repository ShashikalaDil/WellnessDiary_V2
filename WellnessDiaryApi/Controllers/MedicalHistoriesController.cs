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
    public class MedicalHistoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MedicalHistoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicalHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalHistory>>> GetMedicalHistories()
        {
          if (_context.MedicalHistories == null)
          {
              return NotFound();
          }
            return await _context.MedicalHistories.ToListAsync();
        }

        // GET: api/MedicalHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalHistory>> GetMedicalHistory(int id)
        {
          if (_context.MedicalHistories == null)
          {
              return NotFound();
          }
            var medicalHistory = await _context.MedicalHistories.FindAsync(id);

            if (medicalHistory == null)
            {
                return NotFound();
            }

            return medicalHistory;
        }

        // PUT: api/MedicalHistories/5
        [HttpPut("{id}")]
        //public async Task<IActionResult> PutMedicalHistory(int id, MedicalHistory medicalHistory)
        //{
        //    if (id != medicalHistory.HistoryId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(medicalHistory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MedicalHistoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        public async Task<ActionResult<MedicalHistoryDTO>> PutMedicalHistory(int id, MedicalHistoryDTO medicalHistoryDTO)
        {
            if (medicalHistoryDTO == null || medicalHistoryDTO.UserId == null || string.IsNullOrWhiteSpace(medicalHistoryDTO.Diagnosis))
            {
                return BadRequest("Invalid medical history data provided.");
            }

            var existingMedicalHistory = await _context.MedicalHistories.FindAsync(id);

            if (existingMedicalHistory == null)
            {
                return NotFound("medical history not found.");
            }
            existingMedicalHistory.Diagnosis = medicalHistoryDTO.Diagnosis;
            existingMedicalHistory.Surgery = medicalHistoryDTO.Surgery;
            existingMedicalHistory.Allergies = medicalHistoryDTO.Allergies;
            existingMedicalHistory.FamilyMedicalHistory = medicalHistoryDTO.FamilyMedicalHistory;
            
            try
            {
                await _context.SaveChangesAsync();

                return Ok("Medical history successfully updated.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the medical history record: {ex.Message}");
            }
        }

        // POST: api/MedicalHistories
        [HttpPost]  
        public async Task<ActionResult<MedicalHistoryDTO>> PostMedicalHistory(MedicalHistoryDTO medicalHistoryDTO )
        {
            if (medicalHistoryDTO == null || medicalHistoryDTO.UserId == null || string.IsNullOrWhiteSpace(medicalHistoryDTO.Diagnosis))
            {
                return BadRequest("Invalid medication data provided.");
            }

            var medicalHistory = new MedicalHistory
            {
                UserId = medicalHistoryDTO.UserId,
                Diagnosis = medicalHistoryDTO.Diagnosis,
                Surgery = medicalHistoryDTO.Surgery,
                Allergies = medicalHistoryDTO.Allergies,
                FamilyMedicalHistory=medicalHistoryDTO.FamilyMedicalHistory,
            };

            _context.MedicalHistories.Add(medicalHistory);

            try
            {
                await _context.SaveChangesAsync();

                return Ok("Medical History successfully added.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the Medical History record: {ex.Message}");
            }
        }



        // DELETE: api/MedicalHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalHistory(int id)
        {
            if (_context.MedicalHistories == null)
            {
                return NotFound();
            }
            var medicalHistory = await _context.MedicalHistories.FindAsync(id);
            if (medicalHistory == null)
            {
                return NotFound();
            }

            _context.MedicalHistories.Remove(medicalHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        //GET: api/MedicalHistories/1/medicalHistories
        [HttpGet("{userId}/medicalHistories")]
        public async Task<ActionResult<IEnumerable<MedicalHistoryDTO>>> GetUserMedicalHistories(int userId)
        {
            var userSMedicalHistories = await _context.MedicalHistories
                .Where(s => s.UserId == userId)
                .ToListAsync();

            if (userSMedicalHistories == null || userSMedicalHistories.Count == 0)
            {
                return NotFound("No medical history found for the user.");
            }

            var medicalHistoryDTOs = userSMedicalHistories.Select(s => new MedicalHistoryDTO
            {
                HistoryId=s.HistoryId,
                UserId = userId,
                Allergies = s.Allergies,
                Diagnosis = s.Diagnosis,
                Surgery= s.Surgery,
                FamilyMedicalHistory= s.FamilyMedicalHistory,
                

            }).ToList();

            return Ok(medicalHistoryDTOs);
        }


        private bool MedicalHistoryExists(int id)
        {
            return (_context.MedicalHistories?.Any(e => e.HistoryId == id)).GetValueOrDefault();
        }
    }
}
