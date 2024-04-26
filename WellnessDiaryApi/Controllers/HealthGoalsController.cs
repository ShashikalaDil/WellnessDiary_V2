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
    public class HealthGoalsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HealthGoalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/HealthGoals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthGoal>>> GetHealthGoals()
        {
          if (_context.HealthGoals == null)
          {
              return NotFound();
          }
            return await _context.HealthGoals.ToListAsync();
        }

        // GET: api/HealthGoals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthGoal>> GetHealthGoal(int id)
        {
          if (_context.HealthGoals == null)
          {
              return NotFound();
          }
            var healthGoal = await _context.HealthGoals.FindAsync(id);

            if (healthGoal == null)
            {
                return NotFound();
            }

            return healthGoal;
        }

        // PUT: api/HealthGoals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHealthGoal(int id, HealthGoal healthGoal)
        {
            if (id != healthGoal.GoalId)
            {
                return BadRequest();
            }

            _context.Entry(healthGoal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HealthGoalExists(id))
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

        // POST: api/HealthGoals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HealthGoal>> PostHealthGoal(HealthGoal healthGoal)
        {
          if (_context.HealthGoals == null)
          {
              return Problem("Entity set 'AppDbContext.HealthGoals'  is null.");
          }
            _context.HealthGoals.Add(healthGoal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHealthGoal", new { id = healthGoal.GoalId }, healthGoal);
        }

        // DELETE: api/HealthGoals/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthGoal(int id)
        {
            if (_context.HealthGoals == null)
            {
                return NotFound();
            }
            var healthGoal = await _context.HealthGoals.FindAsync(id);
            if (healthGoal == null)
            {
                return NotFound();
            }

            _context.HealthGoals.Remove(healthGoal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HealthGoalExists(int id)
        {
            return (_context.HealthGoals?.Any(e => e.GoalId == id)).GetValueOrDefault();
        }
    }
}
