using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrentPackageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CurrentPackageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CurrentPackage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrentPackage>>> GetCurrentPackages()
        {
            return await _context.CurrentPackage.ToListAsync();
        }

        // GET: api/CurrentPackage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrentPackage>> GetCurrentPackage(int id)
        {
            var currentPackage = await _context.CurrentPackage.FindAsync(id);

            if (currentPackage == null)
            {
                return NotFound();
            }

            return currentPackage;
        }
        [HttpGet("ByCreatorID/{creatorid}")]
        public async Task<ActionResult<CurrentPackage>> GetCurrentPackageByCreatorID(int creatorid)
        {
            var currentPackage = await _context.CurrentPackage
                .FirstOrDefaultAsync(o => o.CreatorID == creatorid);

            if (currentPackage == null)
            {
                return NotFound();
            }

            return Ok(currentPackage);
        }


        [HttpGet("CountCreatorsByPackage")]
        public async Task<ActionResult<PackageCountDTO>> CountCreatorsByPackage()
        {
            var countPackage1 = await _context.CurrentPackage.Where(cp => cp.PackageID == 1).Select(cp => cp.CreatorID).Distinct().CountAsync();
            var countPackage2 = await _context.CurrentPackage.Where(cp => cp.PackageID == 2).Select(cp => cp.CreatorID).Distinct().CountAsync();

            var result = new PackageCountDTO
            {
                CountPackage1 = countPackage1,
                CountPackage2 = countPackage2
            };

            return Ok(result);
        }

        // POST: api/CurrentPackage
        [HttpPost]
        public async Task<ActionResult<CurrentPackage>> PostCurrentPackage(CurrentPackage currentPackage)
        {
            _context.CurrentPackage.Add(currentPackage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurrentPackage), new { id = currentPackage.CurrentPackageID }, currentPackage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurrentPackage(int id, CurrentPackage currentPackage)
        {
            if (id != currentPackage.CurrentPackageID)
            {
                return BadRequest();
            }

            _context.Entry(currentPackage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrentPackageExists(id))
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

        // DELETE: api/CurrentPackage/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurrentPackage(int id)
        {
            var currentPackage = await _context.CurrentPackage.FindAsync(id);
            if (currentPackage == null)
            {
                return NotFound();
            }

            _context.CurrentPackage.Remove(currentPackage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CurrentPackageExists(int id)
        {
            return _context.CurrentPackage.Any(e => e.CurrentPackageID == id);
        }


    }
}
public class PackageCountDTO
{
    public int CountPackage1 { get; set; }
    public int CountPackage2 { get; set; }
}
