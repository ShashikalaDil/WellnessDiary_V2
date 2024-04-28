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
    public class CholesterolsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CholesterolsController(AppDbContext context)
        {
            _context = context;
        }

        
        // GET: api/Cholesterols/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cholesterol>> GetCholesterol(int id)
        {
          if (_context.Cholesterols == null)
          {
              return NotFound();
          }
            var cholesterol = await _context.Cholesterols.FindAsync(id);

            if (cholesterol == null)
            {
                return NotFound();
            }

            return cholesterol;
        }

        
        // DELETE: api/Cholesterols/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCholesterol(int id)
        {
            if (_context.Cholesterols == null)
            {
                return NotFound();
            }
            var cholesterol = await _context.Cholesterols.FindAsync(id);
            if (cholesterol == null)
            {
                return NotFound();
            }

            _context.Cholesterols.Remove(cholesterol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CholesterolExists(int id)
        {
            return (_context.Cholesterols?.Any(e => e.ReadingId == id)).GetValueOrDefault();
        }               

        // Method to determine cholesterol level category and description
        private (string category, string description) GetCholesterolCategory(decimal totalCholesterol, decimal hdl, decimal ldl)
        {            
            string totalCholesterolMessage = GetMessageForTotalCholesterol(totalCholesterol);
            string hdlMessage = GetMessageForHDL(hdl);
            string ldlMessage = GetMessageForLDL(ldl);

            // Combine messages for Total Cholesterol, HDL, and LDL
            string combinedMessage = $"{totalCholesterolMessage} {hdlMessage} {ldlMessage}";

            // Determine overall category based on individual components
            if (totalCholesterol < 200 && hdl >= 40 && ldl < 100)
            {
                return ("Normal", combinedMessage);
            }
            else if (totalCholesterol >= 200 && hdl >= 40 && ldl >= 100)
            {
                return ("High", combinedMessage);
            }
            else
            {
                return ("Unknown", combinedMessage);
            }
        }

        // Method to get message for Total Cholesterol
        private string GetMessageForTotalCholesterol(decimal totalCholesterol)
        {
            if (totalCholesterol < 200)
            {
                return "Total Cholesterol level is low.";
            }
            else if (totalCholesterol >= 200 && totalCholesterol < 240)
            {
                return "Total Cholesterol level is normal.";
            }
            else
            {
                return "Total Cholesterol level is high.";
            }
        }

        // Method to get message for HDL
        private string GetMessageForHDL(decimal hdl)
        {
            if (hdl < 40)
            {
                return "HDL level is low.";
            }
            else if (hdl >= 40 && hdl < 60)
            {
                return "HDL level is normal.";
            }
            else
            {
                return "HDL level is high.";
            }
        }

        // Method to get message for LDL
        private string GetMessageForLDL(decimal ldl)
        {
            if (ldl < 100)
            {
                return "LDL level is low.";
            }
            else if (ldl >= 100 && ldl < 130)
            {
                return "LDL level is normal.";
            }
            else
            {
                return "LDL level is high.";
            }
        }

        // GET: api/Cholesterols/{userId}/cholesterol
        [HttpGet("{userId}/cholesterol")]
        public async Task<ActionResult<IEnumerable<CholesterolDTO>>> GetUserCholesterolList(int userId)
        {
            var userCholesterolList = await _context.Cholesterols
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.RecordedDateTime)
                .ToListAsync();

            if (userCholesterolList == null || userCholesterolList.Count == 0)
            {
                return NotFound("No BMI records found for the user.");
            }

            return Ok(userCholesterolList);
        }

        // POST: api/Cholesterol
        [HttpPost]
        public async Task<ActionResult<CholesterolDTO>> PostCholesterol(CholesterolDTO cholesterolDTO)
        {
            try
            {
                // Ensure the cholesterolDTO is valid
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Map the CholesterolDTO to Cholesterol entity
                var cholesterol = new Cholesterol
                {
                    UserId = cholesterolDTO.UserId,
                    TotalCholesterol = cholesterolDTO.TotalCholesterol,
                    Hdl = cholesterolDTO.Hdl,
                    Ldl = cholesterolDTO.Ldl,
                    RecordedDateTime = cholesterolDTO.RecordedDateTime,
                    StatusId = cholesterolDTO.StatusId
                };

                // Add the cholesterol entity to the database
                _context.Cholesterols.Add(cholesterol);
                await _context.SaveChangesAsync();

                // Generate messages for total cholesterol, HDL, and LDL levels
                var totalCholesterolMessage = GetMessageForTotalCholesterol(cholesterolDTO.TotalCholesterol);
                var hdlMessage = GetMessageForHDL(cholesterolDTO.Hdl);
                var ldlMessage = GetMessageForLDL(cholesterolDTO.Ldl);

                // Construct a response object with cholesterol DTO and messages
                var response = new
                {
                    Cholesterol = cholesterolDTO,
                    TotalCholesterolMessage = totalCholesterolMessage,
                    HdlMessage = hdlMessage,
                    LdlMessage = ldlMessage
                };

                // Return 201 Created response with the created cholesterolDTO and messages
                return CreatedAtAction(nameof(GetCholesterol), new { id = cholesterol.ReadingId }, response);


            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while saving the cholesterol reading.");
            }
        }

        // PUT: api/Cholesterol/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCholesterol(int id, CholesterolDTO cholesterolDTO)
        {
            if (id != cholesterolDTO.ReadingId)
            {
                return BadRequest(); // Bad request if the reading ID in the URL doesn't match the reading ID in the request body
            }

            // Retrieve the existing cholesterol reading from the database
            var cholesterol = await _context.Cholesterols.FindAsync(id);

            if (cholesterol == null)
            {
                return NotFound(); // Return 404 Not Found if the reading with the specified ID doesn't exist
            }

            try
            {
                // Update the properties of the existing cholesterol reading
                cholesterol.UserId = cholesterolDTO.UserId;
                cholesterol.TotalCholesterol = cholesterolDTO.TotalCholesterol;
                cholesterol.Hdl = cholesterolDTO.Hdl;
                cholesterol.Ldl = cholesterolDTO.Ldl;
                cholesterol.RecordedDateTime = cholesterolDTO.RecordedDateTime;
                cholesterol.StatusId = cholesterolDTO.StatusId;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                // Generate messages for total cholesterol, HDL, and LDL levels
                var totalCholesterolMessage = GetMessageForTotalCholesterol(cholesterolDTO.TotalCholesterol);
                var hdlMessage = GetMessageForHDL(cholesterolDTO.Hdl);
                var ldlMessage = GetMessageForLDL(cholesterolDTO.Ldl);

                var response = new
                {
                    TotalCholesterolMessage = totalCholesterolMessage,
                    HdlMessage = hdlMessage,
                    LdlMessage = ldlMessage,
                    SuccessMessage = "Cholesterol reading updated successfully."
                };

                return Ok(response);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CholesterolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

    }
}
