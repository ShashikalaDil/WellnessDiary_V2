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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFbc(int id, Fbc fbc)
        {
            if (id != fbc.ReadingId)
            {
                return BadRequest();
            }

            _context.Entry(fbc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/Fbcs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Fbc>> PostFbc(Fbc fbc)
        {
          if (_context.Fbcs == null)
          {
              return Problem("Entity set 'AppDbContext.Fbcs'  is null.");
          }
            _context.Fbcs.Add(fbc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFbc", new { id = fbc.ReadingId }, fbc);
        }

        // DELETE: api/Fbcs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFbc(int id)
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

            _context.Fbcs.Remove(fbc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FbcExists(int id)
        {
            return (_context.Fbcs?.Any(e => e.ReadingId == id)).GetValueOrDefault();
        }
    }
}
