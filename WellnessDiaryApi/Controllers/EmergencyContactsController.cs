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
    public class EmergencyContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmergencyContactsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/EmergencyContacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmergencyContact>>> GetEmergencyContacts()
        {
          if (_context.EmergencyContacts == null)
          {
              return NotFound();
          }
            return await _context.EmergencyContacts.ToListAsync();
        }

        // GET: api/EmergencyContacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmergencyContact>> GetEmergencyContact(int id)
        {
          if (_context.EmergencyContacts == null)
          {
              return NotFound();
          }
            var emergencyContact = await _context.EmergencyContacts.FindAsync(id);

            if (emergencyContact == null)
            {
                return NotFound();
            }

            return emergencyContact;
        }

        // PUT: api/EmergencyContacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmergencyContact(int id, EmergencyContact emergencyContact)
        {
            if (id != emergencyContact.ContactId)
            {
                return BadRequest();
            }

            _context.Entry(emergencyContact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmergencyContactExists(id))
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

        // POST: api/EmergencyContacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmergencyContact>> PostEmergencyContact(EmergencyContact emergencyContact)
        {
          if (_context.EmergencyContacts == null)
          {
              return Problem("Entity set 'AppDbContext.EmergencyContacts'  is null.");
          }
            _context.EmergencyContacts.Add(emergencyContact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmergencyContact", new { id = emergencyContact.ContactId }, emergencyContact);
        }

        // DELETE: api/EmergencyContacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmergencyContact(int id)
        {
            if (_context.EmergencyContacts == null)
            {
                return NotFound();
            }
            var emergencyContact = await _context.EmergencyContacts.FindAsync(id);
            if (emergencyContact == null)
            {
                return NotFound();
            }

            _context.EmergencyContacts.Remove(emergencyContact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmergencyContactExists(int id)
        {
            return (_context.EmergencyContacts?.Any(e => e.ContactId == id)).GetValueOrDefault();
        }
    }
}
