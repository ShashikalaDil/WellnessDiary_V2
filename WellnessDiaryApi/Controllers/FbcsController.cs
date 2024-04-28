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
    public class FbcsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FbcsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Fbcs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fbc>>> GetFbcs()
        {
          if (_context.Fbcs == null)
          {
              return NotFound();
          }
            return await _context.Fbcs.ToListAsync();
        }

        // GET: api/Fbcs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fbc>> GetFbc(int id)
        {
          if (_context.Fbcs == null)
          {
              return NotFound();
          }
            var fbc = await _context.Fbcs.FindAsync(id);

            if (fbc == null)
            {
                return NotFound();
            }

            return fbc;
        }

        // PUT: api/Fbcs/5
        

        // POST: api/Fbcs
       

        private bool FbcExists(int id)
        {
            return (_context.Fbcs?.Any(e => e.ReadingId == id)).GetValueOrDefault();
        }

        // GET: api/Fbcs/{userId}/fbc
        [HttpGet("{userId}/fbc")]
        public async Task<ActionResult<IEnumerable<FbcDTO>>> GetUserFbclList(int userId)
        {
            var userFbclList = await _context.Fbcs
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.RecordedDateTime)
                .ToListAsync();

            if (userFbclList == null || userFbclList.Count == 0)
            {
                return NotFound("No BMI records found for the user.");
            }

            return Ok(userFbclList);
        }

        // PUT: api/Fbc/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFbc(int id, FbcDTO fbcDTO)
        {
            if (id != fbcDTO.ReadingId)
            {
                return BadRequest();
            }

            var fbc = await _context.Fbcs.FindAsync(id);

            if (fbc == null)
            {
                return NotFound();
            }

            try
            {
                fbc.UserId = fbcDTO.UserId;
                fbc.Hemoglobin = fbcDTO.Hemoglobin;
                fbc.WhiteBloodCellCount = fbcDTO.WhiteBloodCellCount;
                fbc.PlateletCount = fbcDTO.PlateletCount;
                fbc.RecordedDateTime = fbcDTO.RecordedDateTime;
                fbc.Rbc = fbcDTO.Rbc;
                fbc.Neutrophils = fbcDTO.Neutrophils;
                fbc.Eosinophils = fbcDTO.Eosinophils;
                fbc.Lymphocytes = fbcDTO.Lymphocytes;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FbcExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Fbc
        [HttpPost]
        public async Task<ActionResult<FbcDTO>> PostFbc(FbcDTO fbcDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var fbc = new Fbc
                {
                    UserId = fbcDTO.UserId,
                    Hemoglobin = fbcDTO.Hemoglobin,
                    WhiteBloodCellCount = fbcDTO.WhiteBloodCellCount,
                    PlateletCount = fbcDTO.PlateletCount,
                    RecordedDateTime = fbcDTO.RecordedDateTime,
                    Rbc = fbcDTO.Rbc,
                    Neutrophils = fbcDTO.Neutrophils,
                    Eosinophils = fbcDTO.Eosinophils,
                    Lymphocytes = fbcDTO.Lymphocytes
                };

                _context.Fbcs.Add(fbc);
                await _context.SaveChangesAsync();

                // Generate messages for FBC components
                var hemoglobinMessage = GetMessageForHemoglobin(fbcDTO.Hemoglobin);
                var wbcMessage = GetMessageForWhiteBloodCellCount(fbcDTO.WhiteBloodCellCount);
                var plateletMessage = GetMessageForPlateletCount(fbcDTO.PlateletCount);
                var rbcMessage = GetMessageForRbc(fbcDTO.Rbc);
                var neutrophilsMessage = GetMessageForNeutrophils(fbcDTO.Neutrophils);
                var eosinophilsMessage = GetMessageForEosinophils(fbcDTO.Eosinophils);
                var lymphocytesMessage = GetMessageForLymphocytes(fbcDTO.Lymphocytes);

                var fbcDtoWithMessages = new
                {
                    Fbc = fbcDTO,
                    HemoglobinMessage = hemoglobinMessage,
                    WhiteBloodCellCountMessage = wbcMessage,
                    PlateletCountMessage = plateletMessage,
                    RbcMessage = rbcMessage,
                    NeutrophilsMessage = neutrophilsMessage,
                    EosinophilsMessage = eosinophilsMessage,
                    LymphocytesMessage = lymphocytesMessage
                };

                //return CreatedAtAction(nameof(GetFbc), new { id = fbc.ReadingId }, fbcDTO);
                return CreatedAtAction(nameof(GetFbc), new { id = fbc.ReadingId }, fbcDtoWithMessages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving the FBC reading.");
            }
        }


        // Method to get message for Hemoglobin level
        private string GetMessageForHemoglobin(decimal? hemoglobin)
        {
            if (hemoglobin.HasValue)
            {
                if (hemoglobin < 12)
                {
                    return "Hemoglobin level is low.";
                }
                else if (hemoglobin >= 12 && hemoglobin < 16)
                {
                    return "Hemoglobin level is normal.";
                }
                else
                {
                    return "Hemoglobin level is high.";
                }
            }
            return "Hemoglobin level is not available.";
        }

        // Method to get message for White Blood Cell Count
        private string GetMessageForWhiteBloodCellCount(decimal? wbc)
        {
            if (wbc.HasValue)
            {
                if (wbc < 4)
                {
                    return "White blood cell count is low.";
                }
                else if (wbc >= 4 && wbc < 10)
                {
                    return "White blood cell count is normal.";
                }
                else
                {
                    return "White blood cell count is high.";
                }
            }
            return "White blood cell count is not available.";
        }

        // Method to get message for Platelet Count
        private string GetMessageForPlateletCount(decimal? platelets)
        {
            if (platelets.HasValue)
            {
                if (platelets < 150)
                {
                    return "Platelet count is low.";
                }
                else if (platelets >= 150 && platelets < 450)
                {
                    return "Platelet count is normal.";
                }
                else
                {
                    return "Platelet count is high.";
                }
            }
            return "Platelet count is not available.";
        }

        // Method to get message for RBC (Red Blood Cell) count
        private string GetMessageForRbc(decimal? rbc)
        {
            if (rbc.HasValue)
            {
                if (rbc < 4.2m)
                {
                    return "Red blood cell count is low.";
                }
                else if (rbc >= 4.2m && rbc < 6.3m)
                {
                    return "Red blood cell count is normal.";
                }
                else
                {
                    return "Red blood cell count is high.";
                }
            }
            return "Red blood cell count is not available.";
        }

        // Method to get message for Neutrophils level
        private string GetMessageForNeutrophils(decimal? neutrophils)
        {
            if (neutrophils.HasValue)
            {
                if (neutrophils < 2)
                {
                    return "Neutrophils level is low.";
                }
                else if (neutrophils >= 2 && neutrophils < 7)
                {
                    return "Neutrophils level is normal.";
                }
                else
                {
                    return "Neutrophils level is high.";
                }
            }
            return "Neutrophils level is not available.";
        }

        // Method to get message for Eosinophils level
        private string GetMessageForEosinophils(decimal? eosinophils)
        {
            if (eosinophils.HasValue)
            {
                if (eosinophils < 0.05m)
                {
                    return "Eosinophils level is low.";
                }
                else if (eosinophils >= 0.05m && eosinophils < 0.5m)
                {
                    return "Eosinophils level is normal.";
                }
                else
                {
                    return "Eosinophils level is high.";
                }
            }
            return "Eosinophils level is not available.";
        }

        // Method to get message for Lymphocytes level
        private string GetMessageForLymphocytes(decimal? lymphocytes)
        {
            if (lymphocytes.HasValue)
            {
                if (lymphocytes < 1)
                {
                    return "Lymphocytes level is low.";
                }
                else if (lymphocytes >= 1 && lymphocytes < 3)
                {
                    return "Lymphocytes level is normal.";
                }
                else
                {
                    return "Lymphocytes level is high.";
                }
            }
            return "Lymphocytes level is not available.";
        }
    }
}
