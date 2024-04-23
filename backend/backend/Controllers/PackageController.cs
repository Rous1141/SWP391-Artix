using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities;
using backend.Service;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;




[Route("api/[controller]")]
[ApiController]
public class PackageController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IVnPayService _vnPayService;
    public PackageController(IVnPayService vnPayService, ApplicationDbContext context)
    {
        _vnPayService = vnPayService;
        _context = context;
    }

    // GET: api/Package
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Package>>> GetPackages()
    {
        return await _context.Package.ToListAsync();
    }

    // GET: api/Package/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Package>> GetPackage(int id)
    {
        var package = await _context.Package.FindAsync(id);

        if (package == null)
        {
            return NotFound();
        }

        return package;
    }
    // POST: api/Package
    [HttpPost]
    public async Task<ActionResult<Package>> PostPackage(Package package)
    {
        _context.Package.Add(package);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPackage), new { id = package.PackageID }, package);
    }


    [HttpPost("Purchase")]
    public async Task<IActionResult> PurchasePackage([FromBody] Package package)
    {
        var packageInDb = await _context.Package.FindAsync(package.PackageID);
        if (packageInDb == null)
        {
            return NotFound("Gói package không tồn tại");
        }

        
       
        _context.Package.Update(packageInDb);
        await _context.SaveChangesAsync();

        // Lấy ID của người dùng hiện tại từ HTTP Context
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            // Tìm current package tương ứng với người dùng hiện tại
            var userIdInt = int.Parse(userId);
            var currentPackage = await _context.CurrentPackage.FirstOrDefaultAsync(cp => cp.CreatorID == userIdInt);


            if (currentPackage != null)
            {
                // Cập nhật PackageId của CurrentPackage thành 2
                currentPackage.PackageID = 2;
                _context.CurrentPackage.Update(currentPackage);
                await _context.SaveChangesAsync();
            }
        }

        // Tạo URL thanh toán cho gói package
        var paymentUrl = _vnPayService.CreatePaymentUrlForPackage(packageInDb, HttpContext);

        // Trả về URL thanh toán cho người dùng
        return Ok(paymentUrl);
    }



    // GET: api/Package/Callback
    [HttpGet("Callback")]
    public async Task<IActionResult> PaymentCallbackPackage()
    {
        var response = _vnPayService.PaymentExecute(Request.Query);
        int packageId = int.Parse(HttpContext.Request.Query["PackageId"]);

        // Kiểm tra response
        if (response == null)
        {
            // Nếu response là null, điều này có thể dẫn đến lỗi
            return Redirect("~/fail-page");
        }

        // Lấy thông tin gói package từ cơ sở dữ liệu
        var package = await _context.CurrentPackage.FindAsync(packageId);
        if (package == null)
        {
            return NotFound("Gói package không tồn tại");
        }

        // Kiểm tra kết quả phản hồi từ VNPay
        if (response.Success && response.VnPayResponseCode == "00")
        {
            // Nếu thanh toán thành công, cập nhật trạng thái của gói package
            _context.CurrentPackage.Update(package);
            await _context.SaveChangesAsync();

            // Thực hiện các hành động khác sau khi thanh toán thành công

            return Redirect("http://localhost:3000/characters/package");
        }
        else
        {
            // Nếu thanh toán không thành công, xử lý phản hồi tương ứng
            // (ví dụ: gửi email thông báo, cập nhật trạng thái của gói package, v.v.)
            _context.CurrentPackage.Update(package);
            await _context.SaveChangesAsync();

            return Redirect("http://localhost:3000/characters/package");
        }
    }




    [HttpPut("{id}")]
    public async Task<IActionResult> PutPackage(int id, Package package)
    {
        if (id != package.PackageID)
        {
            return BadRequest();
        }

        _context.Entry(package).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PackageExists(id))
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
    // DELETE: api/Package/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackage(int id)
    {
        var package = await _context.Package.FindAsync(id);
        if (package == null)
        {
            return NotFound();
        }

        _context.Package.Remove(package);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PackageExists(int id)
    {
        return _context.Package.Any(e => e.PackageID == id);
    }




}
