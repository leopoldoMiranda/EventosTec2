using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventosTec.Web.Models;
using EventosTec.Web.Models.Entities;
using EventosTec.Web.Models.ModelApi;

namespace EventosTec.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly DataDbContext _context;

        public CategoriesController(DataDbContext context)
        {
            _context = context;
        }

//------------------------AGREGADO-------------------------------------------
        // GET: Categories
        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories;
        }
        //--------GET:api/categories/5----------------AGREGADO-------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult>GetCategory([FromRoute] int id)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.Include(a => a.Events)
                            .FirstOrDefaultAsync(a => a.Id == id);

            var response = new CategoryResponse
            {
                Description = category.Description,
                Name = category.Name,
                Id = category.Id,
                Events = category.Events.Select(p => new EventResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Duration = p.Duration,
                    EventDate = p.EventDate,
                    People = p.People,
                    Description = p.Description,
                }).ToList(),
            };

            if (category == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //----PUT:api/categories/5--------------------AGREGADO-------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult>PutCategory([FromRoute]int id,[FromBody]Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != category.Id)
            {
                return BadRequest();
            }
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExist(id))
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

        //--------POST:api/categories---------AGREGADO--------------------------------------------------
        [HttpPost]
        public async Task<IActionResult>PostCategory([FromBody]Category category)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        //--------DELETE: api/Categories/5-----------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }

        private bool CategoryExist(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        //-------------------------------------------------------------------
/*
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }*/
    }
}
