﻿using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities;


[ApiController]
[Route("api/[controller]")]
public class CreatorController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CreatorController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Creator
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Creator>>> GetCreators()
    {
        var creators = await _context.Creators
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,
                AccountID = c.AccountID,
                PaymentID = c.PaymentID,
                UserName = c.UserName,
                ProfilePicture = c.ProfilePicture,
                BackgroundPicture = c.BackgroundPicture,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Phone = c.Phone,
                LastLogDate = c.LastLogDate,
                AllowCommission = c.AllowCommission,
                Biography = c.Biography,
                VIP = c.VIP,
                FollowCounts = c.FollowCounts,

            })
            .ToListAsync();

        return creators;
    }


    [HttpGet("VipCreators")]
    public async Task<ActionResult<IEnumerable<Creator>>> GetVipCreators()
    {
        var vipCreators = await _context.Creators
            .Where(c => c.VIP == true)
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,
                AccountID = c.AccountID,
                PaymentID = c.PaymentID,
                UserName = c.UserName,
                ProfilePicture = c.ProfilePicture,
                BackgroundPicture = c.BackgroundPicture,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Phone = c.Phone,
                LastLogDate = c.LastLogDate,
                AllowCommission = c.AllowCommission,
                Biography = c.Biography,
                VIP = c.VIP,
                FollowCounts = c.FollowCounts,
            })
            .ToListAsync();

        return vipCreators;
    }


    [HttpGet("GetID/UserName/Vip")]
    public async Task<ActionResult<IEnumerable<Creator>>> Get3FeaturesCreators()
    {
        var creators = await _context.Creators
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,

                UserName = c.UserName,

                VIP = c.VIP,

                Email = c.Email,

                Phone = c.Phone

            })
            .ToListAsync();

        return creators;
    }


    [HttpGet("ProfilePicture/{CreatorID}")]
    public async Task<ActionResult<Creator>> GetProfilePictureByCreatorID(int CreatorID)
    {
        var creator = await _context.Creators
            .Where(c => c.CreatorID == CreatorID)
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,
                ProfilePicture = c.ProfilePicture
            })
            .FirstOrDefaultAsync();

        if (creator == null)
        {
            return NotFound();
        }

        return creator;
    }


    [HttpGet("NotProfile/NotBackground")]
    public async Task<ActionResult<IEnumerable<Creator>>> GetCreatorsNotProAndBack()
    {
        var creators = await _context.Creators
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,
                AccountID = c.AccountID,
                PaymentID = c.PaymentID,
                UserName = c.UserName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Phone = c.Phone,
                LastLogDate = c.LastLogDate,
                AllowCommission = c.AllowCommission,
                Biography = c.Biography,
                VIP = c.VIP,
                FollowCounts = c.FollowCounts,

            })
            .ToListAsync();

        return creators;
    }
    [HttpGet("NotProfile/NotBackground/{CreatorID}")]
    public async Task<ActionResult<IEnumerable<Creator>>> GetCreatorsNotProAndBack(int CreatorID)
    {
        var creator = await _context.Creators
            .Where(c => c.CreatorID == CreatorID)
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,
                AccountID = c.AccountID,
                PaymentID = c.PaymentID,
                UserName = c.UserName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Phone = c.Phone,
                LastLogDate = c.LastLogDate,
                AllowCommission = c.AllowCommission,
                Biography = c.Biography,
                VIP = c.VIP,
                FollowCounts = c.FollowCounts,

            })
            .ToListAsync();

        if (creator == null)
        {
            return NotFound();
        }

        return creator;
    }

    [HttpGet("NotProfile/NotBackgroundByAccountID/{AccountID}")]
    public async Task<ActionResult<IEnumerable<Creator>>> GetCreatorsNotProAndBackByAccountID(int AccountID)
    {
        var creator = await _context.Creators
            .Where(c => c.AccountID == AccountID)
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,
                AccountID = c.AccountID,
                PaymentID = c.PaymentID,
                UserName = c.UserName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Phone = c.Phone,
                LastLogDate = c.LastLogDate,
                AllowCommission = c.AllowCommission,
                Biography = c.Biography,
                VIP = c.VIP,
                FollowCounts = c.FollowCounts,

            })
            .ToListAsync();

        if (creator == null)
        {
            return NotFound();
        }

        return creator;
    }


    [HttpGet("NotBackground")]
    public async Task<ActionResult<IEnumerable<Creator>>> GetCreatorsNotBack()
    {
        var creators = await _context.Creators
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,
                AccountID = c.AccountID,
                PaymentID = c.PaymentID,
                UserName = c.UserName,
                ProfilePicture = c.ProfilePicture,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Address = c.Address,
                Phone = c.Phone,
                LastLogDate = c.LastLogDate,
                AllowCommission = c.AllowCommission,
                Biography = c.Biography,
                VIP = c.VIP,
                FollowCounts = c.FollowCounts,

            })
            .ToListAsync();

        return creators;
    }



    [HttpGet("OnlyProfilePicture/{CreatorID}")]
    public async Task<ActionResult<Creator>> GetOnlyProfilePictureByCreatorID(int CreatorID)
    {
        var creator = await _context.Creators
            .Where(c => c.CreatorID == CreatorID)
            .Select(c => new Creator
            {

                ProfilePicture = c.ProfilePicture
            })
            .FirstOrDefaultAsync();

        if (creator == null)
        {
            return NotFound();
        }

        return creator;
    }

    [HttpGet("OnlyBackgroundPicture/{CreatorID}")]
    public async Task<ActionResult<Creator>> GetOnlyBackgroundPictureByCreatorID(int CreatorID)
    {
        var creator = await _context.Creators
            .Where(c => c.CreatorID == CreatorID)
            .Select(c => new Creator
            {

                BackgroundPicture = c.BackgroundPicture
            })
            .FirstOrDefaultAsync();

        if (creator == null)
        {
            return NotFound();
        }

        return creator;
    }




    [HttpGet("CountCreators")]
    public async Task<ActionResult<int>> GetCreatorCount()
    {
        int creatorCount = await _context.Creators.CountAsync();
        return creatorCount;
    }



    [HttpGet("BackgroundPictureByCreatorID/{CreatorID}")]
    public async Task<ActionResult<IEnumerable<Creator>>> GetBackgroundPictureByCreatorID(int CreatorID)
    {
        var creator = await _context.Creators
            .Where(c => c.CreatorID == CreatorID)
            .Select(c => new Creator
            {
                CreatorID = c.CreatorID,
                BackgroundPicture = c.BackgroundPicture
            })
            .FirstOrDefaultAsync();

        if (creator == null)
        {
            return NotFound();
        }

        return Ok(creator);
    }






    [HttpGet("{accountId}")]
    public async Task<ActionResult<Creator>> GetCreatorByAccountId(int accountId)
    {
        var creator = await _context.Creators.FirstOrDefaultAsync(c => c.AccountID == accountId);

        if (creator == null)
        {
            return NotFound();
        }

        return creator;
    }




    // GET: api/Creator/5
    [HttpGet("ById/{id}")]
    public async Task<ActionResult<Creator>> GetCreator(int id)
    {
        var creator = await _context.Creators.FindAsync(id);

        if (creator == null)
        {
            return NotFound();
        }

        return creator;
    }
    [HttpGet("ByUserName/{username}")]
    public async Task<List<Creator>> GetCreatorByUsername(string username)
    {
        var creator = await _context.Creators.Select(c => new Creator

        {
            CreatorID = c.CreatorID,
            AccountID = c.AccountID,
            PaymentID = c.PaymentID,
            UserName = c.UserName,
            ProfilePicture = c.ProfilePicture,
            BackgroundPicture = c.BackgroundPicture,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Email = c.Email,
            Address = c.Address,
            Phone = c.Phone,
            LastLogDate = c.LastLogDate,
            AllowCommission = c.AllowCommission,
            Biography = c.Biography,
            VIP = c.VIP,
            FollowCounts = c.FollowCounts,

        }).Where(c => c.UserName.ToLower().Contains(username.ToLower())).ToListAsync();


        return creator;
    }


    public class SearchResult
    {
        public List<Creator> Creators { get; set; }
        public List<Artworks> ArtworksByArtworkName { get; set; }
        public List<Artworks> ArtworksByTagName { get; set; }
    }

    [HttpGet("Search")]
    public async Task<ActionResult<SearchResult>> Search(string searchTerm)
    {
        var searchResult = new SearchResult();

        // Tìm kiếm creators có UserName chứa searchTerm
        var creators = await _context.Creators
            .Where(c => c.UserName.Contains(searchTerm))
            .ToListAsync();
        searchResult.Creators = creators;

        // Tìm kiếm artworks có ArtworkName chứa ít nhất một từ khóa từ searchTerm
        var artworksByArtworkName = await _context.Artworks
            .Where(a => a.ArtworkName.Contains(searchTerm))
            .ToListAsync();
        searchResult.ArtworksByArtworkName = artworksByArtworkName;

        // Tìm kiếm artworks dựa trên TagName nếu có
        var tagsContainingSearchTerm = await _context.Tags
      .Where(t => t.TagName.Contains(searchTerm))
      .ToListAsync();

        // Lấy danh sách TagID từ các tag đã lọc
        var tagIds = tagsContainingSearchTerm.Select(t => t.TagID).ToList();

        var artworkIds = await _context.ArtworkTag
       .Where(at => tagIds.Contains(at.TagID))
       .Select(at => at.ArtworkID)
       .ToListAsync();

        // Lấy danh sách artworks dựa trên danh sách ArtworkID
        searchResult.ArtworksByTagName = await _context.Artworks
    .Where(a => artworkIds.Contains(a.ArtworkID))
    .Select(a => new Artworks
    {
        ArtworkID = a.ArtworkID,
        CreatorID = a.CreatorID,
        ArtworkName = a.ArtworkName,
        Description = a.Description,
        DateCreated = a.DateCreated,
        Likes = a.Likes,
        Purchasable = a.Purchasable,
        Price = a.Price,
        ImageFile = a.ImageFile,
        ArtworkTag = a.ArtworkTag
    })
    .ToListAsync();


        return searchResult;
    }


    // POST: api/Creator
    [HttpPost]
    public async Task<IActionResult> CreateCreator([FromBody] Creator creatorModel)
    {
        if (creatorModel == null)
        {
            return BadRequest("Invalid data. Creator object is null.");
        }

        // Kiểm tra xem tệp hình ảnh có giá trị không
        if (!string.IsNullOrEmpty(creatorModel.ProfilePicture))
        {
            try
            {
                // Thực hiện xử lý kiểm tra và chuyển đổi dữ liệu Base64 nếu cần
                byte[] imageBytes = Convert.FromBase64String(creatorModel.ProfilePicture);
                // Lưu imageBytes vào cơ sở dữ liệu hoặc thực hiện các bước xử lý khác tùy thuộc vào yêu cầu của bạn
            }
            catch (FormatException)
            {
                return BadRequest("Định dạng hình ảnh không hợp lệ");
            }
        }

        // Kiểm tra xem tệp hình ảnh BackgroundPicture có giá trị không
        if (!string.IsNullOrEmpty(creatorModel.BackgroundPicture))
        {
            try
            {
                // Thực hiện xử lý kiểm tra và chuyển đổi dữ liệu Base64 nếu cần
                byte[] backgroundBytes = Convert.FromBase64String(creatorModel.BackgroundPicture);
                // Lưu backgroundBytes vào cơ sở dữ liệu hoặc thực hiện các bước xử lý khác tùy thuộc vào yêu cầu của bạn
            }
            catch (FormatException)
            {
                return BadRequest("Định dạng hình ảnh BackgroundPicture không hợp lệ");
            }
        }

        if (creatorModel.AccountID == null)
        {
            return BadRequest("AccountID is required.");
        }

        // Kiểm tra xem AccountID có tồn tại trong bảng Accounts hay không
        if (!_context.Account.Any(a => a.AccountID == creatorModel.AccountID))
        {
            return BadRequest("Invalid AccountID.");
        }

        if (creatorModel.VIP == null)
        {
            creatorModel.VIP = false;
        }



        _context.Creators.Add(creatorModel);
        await _context.SaveChangesAsync();

        // Trả về đối tượng đã được tạo
        return Ok(creatorModel);
    }


    [HttpPut("updateProfilePicture/{id}")]
    public async Task<IActionResult> UpdateProfilePicture(int id, [FromBody] string profilePicture)
    {
        var existingCreator = await _context.Creators.FindAsync(id);

        if (existingCreator == null)
        {
            return NotFound();
        }

        existingCreator.ProfilePicture = profilePicture;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CreatorExists(id))
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

    [HttpPut("updateBackground/{id}")]
    public async Task<IActionResult> UpdateBackground(int id, [FromBody] string background)
    {
        var existingCreator = await _context.Creators.FindAsync(id);

        if (existingCreator == null)
        {
            return NotFound();
        }

        existingCreator.BackgroundPicture = background;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CreatorExists(id))
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




    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCreator(int id)
    {
        var creator = await _context.Creators.FindAsync(id);
        if (creator == null)
        {
            return NotFound();
        }

        // Xóa tất cả các bản ghi trong bảng CommissionForm có ReceiverID là creator.Id
        var commissionForms = await _context.CommissionForm.Where(cf => cf.ReceiverID == id).ToListAsync();
        _context.CommissionForm.RemoveRange(commissionForms);

        // Xóa tất cả các bản ghi trong bảng Notification có CreatorID là creator.Id
        var notifications = await _context.Notification.Where(n => n.CreatorID == id).ToListAsync();
        _context.Notification.RemoveRange(notifications);

        // Xóa tất cả các bản ghi trong bảng Orders có CreatorID là creator.Id
        var orders = await _context.Orders.Where(o => o.SellerID == id).ToListAsync();
        foreach (var order in orders)
        {
            // Tìm và xóa tất cả các bản ghi trong bảng OrderDetail có OrderID là ID của đơn đặt hàng hiện tại
            var orderDetails = await _context.OrderDetail.Where(od => od.OrderID == order.OrderID).ToListAsync();
            _context.OrderDetail.RemoveRange(orderDetails);
        }
        _context.Orders.RemoveRange(orders);
        await _context.SaveChangesAsync();
        // Xóa tất cả các bản ghi trong bảng Reports có CreatorID là creator.Id
        var reports = await _context.Reports.Where(r => r.ReporterID == id).ToListAsync();
        foreach (var report in reports)
        {
            var moderators = await _context.Moderators.Where(m => m.ReportID == report.ReportID).ToListAsync();
            _context.Moderators.RemoveRange(moderators);
        }
        _context.Reports.RemoveRange(reports);


        // Xóa tất cả các bản ghi trong bảng OrderDetail có CreatorID là creator.Id


        // Lưu các thay đổi vào cơ sở dữ liệu
        await _context.SaveChangesAsync();

        // Xóa creator
        _context.Creators.Remove(creator);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    private bool CreatorExists(int id)
    {
        return _context.Creators.Any(e => e.CreatorID == id);
    }
}