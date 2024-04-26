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
    public class DietNutritionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DietNutritionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DietNutritions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DietNutrition>>> GetDietNutritions()
        {
          if (_context.DietNutritions == null)
          {
              return NotFound();
          }
            return await _context.DietNutritions.ToListAsync();
        }

        // GET: api/DietNutritions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DietNutrition>> GetDietNutrition(int id)
        {
          if (_context.DietNutritions == null)
          {
              return NotFound();
          }
            var dietNutrition = await _context.DietNutritions.FindAsync(id);

            if (dietNutrition == null)
            {
                return NotFound();
            }

            return dietNutrition;
        }

        // PUT: api/DietNutritions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDietNutrition(int id, DietNutrition dietNutrition)
        {
            if (id != dietNutrition.NutritionId)
            {
                return BadRequest();
            }

            _context.Entry(dietNutrition).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DietNutritionExists(id))
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

        // POST: api/DietNutritions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DietNutrition>> PostDietNutrition(DietNutrition dietNutrition)
        {
          if (_context.DietNutritions == null)
          {
              return Problem("Entity set 'AppDbContext.DietNutritions'  is null.");
          }
            _context.DietNutritions.Add(dietNutrition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDietNutrition", new { id = dietNutrition.NutritionId }, dietNutrition);
        }

        // DELETE: api/DietNutritions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDietNutrition(int id)
        {
            if (_context.DietNutritions == null)
            {
                return NotFound();
            }
            var dietNutrition = await _context.DietNutritions.FindAsync(id);
            if (dietNutrition == null)
            {
                return NotFound();
            }

            _context.DietNutritions.Remove(dietNutrition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DietNutritionExists(int id)
        {
            return (_context.DietNutritions?.Any(e => e.NutritionId == id)).GetValueOrDefault();
        }
    }
}
