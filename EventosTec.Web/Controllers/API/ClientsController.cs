using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventosTec.Web.Models;
using EventosTec.Web.Models.Entities;
using EventosTec.Web.Models.ModelApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventosTec.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//---agreagado
    public class ClientsController : ControllerBase
    {
        private readonly DataDbContext _context;

        public ClientsController(DataDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public IEnumerable<Client> GetClients()
        {
            return _context.Clients;
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _context.Clients.Include(a => a.Events)
                .FirstOrDefaultAsync(a => a.Id == id);

            //-----------------------------------------------------------
            var response = new ClientResponse //ERROR
            {
               // FullName = client.User.FullName,
               // Description = client.User.Description,
                Id = client.Id,//1
                Address = client.Address,//2
               // Email = client.User.Email,
              //  PhoneNumber = client.User.PhoneNumber,
                UserName = client.User.UserName,//3 user
                Events = client.Events.Select(p => new EventResponse//4 events
                {
                   /* Id = p.Id,
                    Name = p.Name,
                    Duration = p.Duration,
                    EventDate = p.EventDate,
                    People = p.People,
                    Description = p.Description,*/
                }).ToList(),
            };//-------------------------------------------------------

            if (client == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient([FromRoute] int id, [FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        public async Task<IActionResult> PostClient([FromBody] Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return Ok(client);
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}