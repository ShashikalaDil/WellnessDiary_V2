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
    public class HeartRatesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HeartRatesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/HeartRates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HeartRate>>> GetHeartRates()
        {
          if (_context.HeartRates == null)
          {
              return NotFound();
          }
            return await _context.HeartRates.ToListAsync();
        }

        // GET: api/HeartRates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HeartRate>> GetHeartRate(int id)
        {
          if (_context.HeartRates == null)
          {
              return NotFound();
          }
            var heartRate = await _context.HeartRates.FindAsync(id);

            if (heartRate == null)
            {
                return NotFound();
            }

            return heartRate;
        }

        // PUT: api/HeartRates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHeartRate(int id, HeartRate heartRate)
        {
            if (id != heartRate.ReadingId)
            {
                return BadRequest();
            }

            _context.Entry(heartRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeartRateExists(id))
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

        // POST: api/HeartRates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HeartRate>> PostHeartRate(HeartRate heartRate)
        {
          if (_context.HeartRates == null)
          {
              return Problem("Entity set 'AppDbContext.HeartRates'  is null.");
          }
            _context.HeartRates.Add(heartRate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHeartRate", new { id = heartRate.ReadingId }, heartRate);
        }

        // DELETE: api/HeartRates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHeartRate(int id)
        {
            if (_context.HeartRates == null)
            {
                return NotFound();
            }
            var heartRate = await _context.HeartRates.FindAsync(id);
            if (heartRate == null)
            {
                return NotFound();
            }

            _context.HeartRates.Remove(heartRate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HeartRateExists(int id)
        {
            return (_context.HeartRates?.Any(e => e.ReadingId == id)).GetValueOrDefault();
        }
    }
}
