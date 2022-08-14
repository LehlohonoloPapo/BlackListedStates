using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlackList.Api.Data;
using BlackList.Api.Models;

namespace BlackList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlackListedStatesController : ControllerBase
    {
        private readonly BlackListApiContext _context;

        public BlackListedStatesController(BlackListApiContext context)
        {
            _context = context;
        }

        // GET: api/BlackListedStates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlackListedStates>>> GetBlackListedStates()
        {
          if (_context.BlackListedStates == null)
          {
              return NotFound();
          }
            return await _context.BlackListedStates.ToListAsync();
        }

        // GET: api/BlackListedStates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlackListedStates>> GetBlackListedStateById(int id)
        {
          if (_context.BlackListedStates == null)
          {
              return NotFound();
          }
            var blackListedStates = await _context.BlackListedStates.FindAsync(id);

            if (blackListedStates == null)
            {
                return NotFound();
            }

            return blackListedStates;
        }

        // PUT: api/BlackListedStates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlackListedStats(int id, BlackListedStates blackListedStates)
        {
            if (id != blackListedStates.ID)
            {
                return BadRequest();
            }

            _context.Entry(blackListedStates).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlackListedStatesExists(id))
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

        // POST: api/BlackListedStates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BlackListedStates>> CreateBlackListedState(BlackListedStates blackListedStates)
        {
          if (_context.BlackListedStates == null)
          {
              return Problem("Entity set 'BlackListApiContext.BlackListedStates'  is null.");
          }
            _context.BlackListedStates.Add(blackListedStates);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlackListedStates", new { id = blackListedStates.ID }, blackListedStates);
        }

        // DELETE: api/BlackListedStates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlackListedStates(int id)
        {
            if (_context.BlackListedStates == null)
            {
                return NotFound();
            }
            var blackListedStates = await _context.BlackListedStates.FindAsync(id);
            if (blackListedStates == null)
            {
                return NotFound();
            }

            _context.BlackListedStates.Remove(blackListedStates);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlackListedStatesExists(int id)
        {
            return (_context.BlackListedStates?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
