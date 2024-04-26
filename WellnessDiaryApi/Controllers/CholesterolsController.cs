using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WellnessDiaryApi.Data;
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

        // GET: api/Cholesterols
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Cholesterol>>> GetCholesterols()
        //{
        //  if (_context.Cholesterols == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Cholesterols.ToListAsync();
        //}

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

        // PUT: api/Cholesterols/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCholesterol(int id, Cholesterol cholesterol)
        {
            if (id != cholesterol.ReadingId)
            {
                return BadRequest();
            }

            _context.Entry(cholesterol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

        // POST: api/Cholesterols
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cholesterol>> PostCholesterol(Cholesterol cholesterol)
        {
          if (_context.Cholesterols == null)
          {
              return Problem("Entity set 'AppDbContext.Cholesterols'  is null.");
          }
            _context.Cholesterols.Add(cholesterol);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCholesterol", new { id = cholesterol.ReadingId }, cholesterol);
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

        ////////////////////
        ///

        // Method to determine cholesterol level category and description
        private (string category, string description) GetCholesterolCategory(decimal totalCholesterol, decimal hdl, decimal ldl)
        {
            // Define your cholesterol level categories and descriptions based on your requirements
            // Here's just a sample, you can adjust it according to your needs

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

    }
}
