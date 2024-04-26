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
    public class ExerciseActivitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ExerciseActivitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ExerciseActivities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseActivity>>> GetExerciseActivities()
        {
          if (_context.ExerciseActivities == null)
          {
              return NotFound();
          }
            return await _context.ExerciseActivities.ToListAsync();
        }

        // GET: api/ExerciseActivities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseActivity>> GetExerciseActivity(int id)
        {
          if (_context.ExerciseActivities == null)
          {
              return NotFound();
          }
            var exerciseActivity = await _context.ExerciseActivities.FindAsync(id);

            if (exerciseActivity == null)
            {
                return NotFound();
            }

            return exerciseActivity;
        }

        // PUT: api/ExerciseActivities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExerciseActivity(int id, ExerciseActivity exerciseActivity)
        {
            if (id != exerciseActivity.ActivityId)
            {
                return BadRequest();
            }

            _context.Entry(exerciseActivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseActivityExists(id))
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

        // POST: api/ExerciseActivities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ExerciseActivity>> PostExerciseActivity(ExerciseActivity exerciseActivity)
        {
          if (_context.ExerciseActivities == null)
          {
              return Problem("Entity set 'AppDbContext.ExerciseActivities'  is null.");
          }
            _context.ExerciseActivities.Add(exerciseActivity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExerciseActivity", new { id = exerciseActivity.ActivityId }, exerciseActivity);
        }

        // DELETE: api/ExerciseActivities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExerciseActivity(int id)
        {
            if (_context.ExerciseActivities == null)
            {
                return NotFound();
            }
            var exerciseActivity = await _context.ExerciseActivities.FindAsync(id);
            if (exerciseActivity == null)
            {
                return NotFound();
            }

            _context.ExerciseActivities.Remove(exerciseActivity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExerciseActivityExists(int id)
        {
            return (_context.ExerciseActivities?.Any(e => e.ActivityId == id)).GetValueOrDefault();
        }
    }
}
