﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using System.Net;
[ApiController]
[Route("api/artworks")]
public class ArtworksController : ControllerBase
{
    private readonly ApplicationDbContext _context; // Replace YourDbContext with your actual database context

    public ArtworksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/artworks
    [HttpGet]
    public async Task<IActionResult> GetArtworks()
    {
        var artworks = await _context.Artworks
            .OrderBy(a => a.DateCreated) // Sắp xếp theo ngày tạo
            .Take(10)
            .Include(a => a.ArtworkTag) // Kèm theo thông tin tag của artwork
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

        if (artworks == null || artworks.Count == 0)
        {
            return NotFound();
        }

        return Ok(artworks);
    }


    [HttpGet("GetArtworksWithPaymentStatus/{buyerId}")]
    public async Task<ActionResult<ArtworksResponse>> GetArtworksWithPaymentStatus(int buyerId, int pageNumber = 1, int pageSize = 6)
    {
        try
        {
            int skipCount = (pageNumber - 1) * pageSize;
            int totalRecords = await _context.Artworks.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Lấy danh sách Artworks và thêm trạng thái vào mỗi artwork trong danh sách
            var artworkViewModels = await _context.Artworks
                .Where(artwork => artwork.Purchasable) // Chỉ lấy những artwork có Purchasable là true
                .Skip(skipCount) // Bỏ qua các bản ghi không cần thiết
                .Take(pageSize) // Chỉ lấy số lượng bản ghi cần thiết cho trang hiện tại
                .Select(artwork => new ArtworkViewModel
                {
                    ArtworkID = artwork.ArtworkID,
                    CreatorID = artwork.CreatorID,
                    ArtworkName = artwork.ArtworkName,
                    Description = artwork.Description,
                    DateCreated = artwork.DateCreated,
                    Likes = artwork.Likes,
                    image = artwork.ImageFile,
                    Purchasable = artwork.Purchasable,
                    Price = artwork.Price,
                    Status = _context.OrderDetail.Any(od => od.ArtWorkID == artwork.ArtworkID && od.Order.BuyerID == buyerId)
                })
                .ToListAsync();

            // Tạo đối tượng chứa artwork và info về phân trang
            var response = new ArtworksResponse
            {
                TotalPages = totalPages,
                ArtworkViewModels = artworkViewModels
            };

            return response;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
        }
    }




    [HttpGet("GetArtworksWithPaymentStatus/{buyerId}/{artworkId}")]
    public async Task<ActionResult<ArtworkViewModel>> GetArtworksWithPaymentStatus2(int buyerId, int artworkId)
    {
        try
        {
            // Lấy thông tin Artwork với artworkId cụ thể
            var artwork = await _context.Artworks.FindAsync(artworkId);

            if (artwork == null)
            {
                return NotFound($"Không tìm thấy Artwork có ID {artworkId}");
            }
            // Kiểm tra trạng thái thanh toán của Artwork cho buyer với buyerId
            bool isPaid = await _context.OrderDetail.AnyAsync(od => od.ArtWorkID == artworkId && od.Order.BuyerID == buyerId);
            // Tạo đối tượng ArtworkViewModel với thông tin từ Artwork và trạng thái thanh toán
            var artworkViewModel = new ArtworkViewModel
            {
                Status = isPaid
            };
            return artworkViewModel;
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi không xác định: {ex.Message}");
        }
    }


    [HttpGet("ArtworkNotImageFile/{ArtworkID}")]
    public async Task<IActionResult> GetArtwork(int ArtworkID)
    {
        var artwork = await _context.Artworks
            .Where(a => a.ArtworkID == ArtworkID)
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
                ArtworkTag = a.ArtworkTag
            })
            .FirstOrDefaultAsync();

        if (artwork == null)
        {
            return NotFound();
        }

        return Ok(artwork);
    }

    [HttpGet("NotImage")]
    public async Task<IActionResult> GetArtworksNotImage()
    {
        var artworks = await _context.Artworks
            .OrderBy(a => a.DateCreated) // Sắp xếp theo ngày tạo
            .Take(5) // Lấy 5 artwork đầu tiên
            .Include(a => a.ArtworkTag) // Kèm theo thông tin tag của artwork
            .Select(a => new Artworks // Tạo đối tượng DTO để chứa thông tin cần thiết
            {
                ArtworkID = a.ArtworkID,
                CreatorID = a.CreatorID,
                ArtworkName = a.ArtworkName,
                Description = a.Description,
                DateCreated = a.DateCreated,
                Likes = a.Likes,
                Purchasable = a.Purchasable,
                Price = a.Price,
                ArtworkTag = a.ArtworkTag
            })
            .ToListAsync();

        if (artworks == null || artworks.Count == 0)
        {
            return NotFound();
        }

        return Ok(artworks);
    }



    [HttpGet("recent7artworks")]
    public async Task<IActionResult> GetRecent7Artworks()
    {
        var recentArtworks = await _context.Artworks
            .OrderByDescending(a => a.DateCreated) // Sắp xếp theo ngày tạo giảm dần (tức là ngày gần nhất đăng lên sẽ ở đầu)
            .Take(7) // Chỉ lấy 7 artwork đầu tiên
            .Select(a => new
            {
                ArtworkID = a.ArtworkID,
                CreatorID = a.CreatorID,
                ArtworkName = a.ArtworkName,
                Description = a.Description,
                DateCreated = a.DateCreated,
                Likes = a.Likes,
                Purchasable = a.Purchasable,
                Price = a.Price,
                ImageFile = a.ImageFile
            })
            .ToListAsync();

        if (recentArtworks == null || recentArtworks.Count == 0)
        {
            return NotFound();
        }
        return Ok(recentArtworks);
    }

    [HttpGet("recent7artworksNotImage")]
    public async Task<IActionResult> GetRecent7ArtworksNotImage()
    {
        var recentArtworks = await _context.Artworks
            .OrderByDescending(a => a.DateCreated) // Sắp xếp theo ngày tạo giảm dần (tức là ngày gần nhất đăng lên sẽ ở đầu)
            .Take(7) // Chỉ lấy 7 artwork đầu tiên
            .Select(a => new
            {
                ArtworkID = a.ArtworkID,
                CreatorID = a.CreatorID,
                ArtworkName = a.ArtworkName,
                Description = a.Description,
                DateCreated = a.DateCreated,
                Likes = a.Likes,
                Purchasable = a.Purchasable,
                Price = a.Price,

            })
            .ToListAsync();

        if (recentArtworks == null || recentArtworks.Count == 0)
        {
            return NotFound();
        }

        return Ok(recentArtworks);
    }

    [HttpGet("recent-artwork-count")]
    public async Task<IActionResult> GetRecentArtworkCount()
    {

        var currentDate = DateTime.UtcNow;

        // Lấy ngày 7 ngày trước
        var sevenDaysAgo = currentDate.AddDays(-7);

        // Đếm số artwork được đăng trong 7 ngày gần nhất
        var recentArtworkCount = await _context.Artworks
            .Where(a => a.DateCreated >= sevenDaysAgo && a.DateCreated <= currentDate) //Lọc các tác phẩm mà ngày tạo nằm trong khoảng từ ngày 7 ngày trước đến ngày hiện tại.
            .CountAsync();//Đếm số lượng tác phẩm thỏa mãn điều kiện đã lọc.

        return Ok(recentArtworkCount);
    }




    [HttpGet("{artworkId}/tags")]
    public async Task<IActionResult> GetArtworkTags(int artworkId)
    {
        var artworkTags = await _context.ArtworkTag
            .Where(at => at.ArtworkID == artworkId) // Using LINQ  // chọn các bản ghi có ArtworkID tương ứng với artworkId
            .Join(_context.Tags,  // inner join giữa bảng ArtworkTag và Tags dựa trên TagID
                at => at.TagID,
                tag => tag.TagID,
                (at, tag) => new  //Chọn các trường TagID và TagName từ bảng Tags sau khi join.
                {
                    TagID = tag.TagID,
                    TagName = tag.TagName
                })
            .Distinct() //Loại các bản bị trùng lập ở trên 
            .ToListAsync();

        if (artworkTags == null || artworkTags.Count == 0)
        {
            return NotFound();
        }

        return Ok(artworkTags);
    }




    [HttpGet("recent-artworks")]
    public async Task<ActionResult<IEnumerable<Artworks>>> GetRecentArtworks()
    {
        var recentArtworks = await _context.Artworks
            .OrderByDescending(a => a.ArtworkID) // Sử dụng ID nếu cần
            .Take(2)
            .Include(a => a.ArtworkTag) // Kèm theo thông tin tag của artwork
        .Select(a => new Artworks // Tạo đối tượng DTO để chứa thông tin cần thiết
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

        return recentArtworks;
    }

    [HttpGet("ByCreatorID/{CreatorID}")]
    public async Task<IActionResult> GetArtworkByCreatorID(int CreatorID)
    {
        var artworks = await _context.Artworks
            .Include(a => a.ArtworkTag)
            .Where(a => a.CreatorID == CreatorID)
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

        if (artworks == null || artworks.Count == 0)
        {
            return NotFound();
        }

        return Ok(artworks);
    }
    [HttpGet("ByCreatorIDNotImage/{CreatorID}")]
    public async Task<IActionResult> GetArtworkByCreatorIDNotImage(int CreatorID)
    {
        var artworks = await _context.Artworks
            .Include(a => a.ArtworkTag)
            .Where(a => a.CreatorID == CreatorID)
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
                ArtworkTag = a.ArtworkTag
            })
            .ToListAsync();

        if (artworks == null || artworks.Count == 0)
        {
            return NotFound();
        }

        return Ok(artworks);
    }




    // GET: api/artworks/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArtworkById(int id)
    {
        var artwork = await _context.Artworks
            .Include(a => a.ArtworkTag)
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
            .FirstOrDefaultAsync(a => a.ArtworkID == id);

        if (artwork == null)
        {
            return NotFound();
        }

        return Ok(artwork);
    }


    [HttpGet("total-likes/{CreatorId}")]
    public async Task<ActionResult<int>> GetTotalLikesByCreatorId(int CreatorId)
    {
        try
        {
            // Tìm tất cả các tác phẩm của một tác giả dựa trên CreatorID
            var artworks = await _context.Artworks.Where(a => a.CreatorID == CreatorId).ToListAsync();

            // Tính tổng lượng like của tất cả các tác phẩm
            int totalLikes = artworks.Sum(a => a.Likes);

            return totalLikes;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu có
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
        }
    }

    [HttpGet("recent-artworks-no-image/{CreatorId}")]
    public async Task<ActionResult<IEnumerable<Artworks>>> GetRecentArtworksWithoutImageByCreatorId(int CreatorId)
    {
        try
        {
            // Lấy 4 tác phẩm gần nhất không có hình ảnh của một tác giả dựa trên CreatorID
            var recentArtworks = await _context.Artworks
                .Where(a => a.CreatorID == CreatorId && a.ImageFile == null)
                .OrderByDescending(a => a.DateCreated)
                .Take(4)
                .ToListAsync();

            return recentArtworks;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu có
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
        }
    }

    [HttpGet("recent-artworks-with-image/{CreatorId}")]
    public async Task<ActionResult<IEnumerable<Artworks>>> GetRecentArtworksWithImageByCreatorId(int CreatorId)
    {
        try
        {
            // Lấy 4 tác phẩm gần nhất có hình ảnh của một tác giả dựa trên CreatorID
            var recentArtworksWithImage = await _context.Artworks
                .Where(a => a.CreatorID == CreatorId && a.ImageFile != null)
                .OrderByDescending(a => a.DateCreated)
                .Take(4)
                .ToListAsync();

            return recentArtworksWithImage;
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu có
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
        }
    }



    // POST: api/artworks

    [HttpPost]
    public async Task<IActionResult> CreateArtwork([FromBody] Artworks artwork)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            // Kiểm tra xem CreatorID có tồn tại không
            if (!_context.Creators.Any(c => c.CreatorID == artwork.CreatorID))
            {
                return BadRequest("CreatorID không tồn tại");
            }

            // Kiểm tra xem TagID có tồn tại không
            if (artwork.ArtworkTag != null && artwork.ArtworkTag.Any())
            {
                var invalidTagIds = artwork.ArtworkTag
                    .Where(at => !_context.Tags.Any(t => t.TagID == at.TagID))
                    .Select(at => at.TagID)
                    .ToList();

                if (invalidTagIds.Any())
                {
                    return BadRequest($"TagID không tồn tại: {string.Join(", ", invalidTagIds)}");
                }
            }

            // Thêm artwork vào cơ sở dữ liệu
            _context.Artworks.Add(artwork);
            await _context.SaveChangesAsync();

            // Lưu trữ ArtworkID đã được tạo tự động
            var artworkId = artwork.ArtworkID;

            // Thêm ArtworkTag vào cơ sở dữ liệu
            foreach (var artworkTag in artwork.ArtworkTag)
            {
                // Kiểm tra xem đã tồn tại ArtworkTag với cùng ArtworkID và TagID chưa
                if (!_context.ArtworkTag.Any(at => at.ArtworkID == artworkId && at.TagID == artworkTag.TagID))
                {
                    // Thiết lập ArtworkID với giá trị đã được tạo tự động
                    artworkTag.ArtworkID = artworkId;
                    _context.ArtworkTag.Add(artworkTag);
                }
            }

            await _context.SaveChangesAsync();
            scope.Complete();
            return Ok(artwork);
        }
    }




    // PUT: api/artworks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutArtwork(int id, [FromBody] Artworks artworkRequest)
    {
        if (id != artworkRequest.ArtworkID)
        {
            return BadRequest("Invalid ID");
        }

        var existingArtwork = await _context.Artworks
            .Include(a => a.ArtworkTag)
            .FirstOrDefaultAsync(a => a.ArtworkID == id);//ID trùng khớp từ cơ sở dữ liệu 

        if (existingArtwork == null)
        {
            return NotFound();
        }

        _context.Entry(existingArtwork).State = EntityState.Detached;
        //khi thực hiện cập nhật, detach tác phẩm đã tồn tại khỏi context để tránh các vấn đề về tracking.
        existingArtwork.ArtworkName = artworkRequest.ArtworkName;
        existingArtwork.Description = artworkRequest.Description;
        existingArtwork.Likes = artworkRequest.Likes;
        existingArtwork.Purchasable = artworkRequest.Purchasable;
        existingArtwork.Price = artworkRequest.Price;

        // Update tags
        existingArtwork.ArtworkTag = artworkRequest.ArtworkTag.Select(tag => new ArtworkTag
        {
            TagID = tag.TagID
        }).ToList();

        _context.Entry(existingArtwork).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ArtworkExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok("Artwork updated successfully");
    }

    private bool ArtworkExists(int id)
    {
        return _context.Artworks.Any(e => e.ArtworkID == id);//kiểm tra xem tác phẩm với id được cung cấp có tồn tại trong cơ sở dữ liệu
    }


    //GET: API/artwork/{Top10Liked}
    [HttpGet("Top10Liked")]
    public async Task<ActionResult<IEnumerable<Artworks>>> GetTopLikedArtworks()
    {
        var topLikedArtworks = await _context.Artworks
            .OrderByDescending(a => a.Likes)
            .Take(10)
            .ToListAsync();

        if (topLikedArtworks == null || topLikedArtworks.Count == 0)
        {
            return NotFound();
        }

        return topLikedArtworks;
    }

    // GET: api/artworks/random11
    [HttpGet("random10")]
    public async Task<IActionResult> GetRandom11Artworks()
    {
        // Lấy danh sách tất cả các artworks từ cơ sở dữ liệu
        var allArtworks = await _context.Artworks.ToListAsync();

        // Kiểm tra xem có artworks nào không
        if (allArtworks.Count == 0)
        {
            return NotFound("Không có artworks nào trong cơ sở dữ liệu.");
        }

        // Lấy 11 artworks ngẫu nhiên từ danh sách tất cả các artworks
        var randomArtworks = GetRandomElements(allArtworks, 10);

        return Ok(randomArtworks);
    }

    // Hàm chọn ngẫu nhiên các phần tử từ danh sách
    private List<Artworks> GetRandomElements(List<Artworks> list, int count)
    {//list, là danh sách các tác phẩm ban đầu, và count, là số lượng tác phẩm muốn lấy ra.
        var random = new Random();
        var randomArtworks = new List<Artworks>();

        while (randomArtworks.Count < count)// randomArtworks có đủ số lượng tác phẩm cần lấy ra.
        {
            var index = random.Next(0, list.Count);// Tạo một số ngẫu nhiên từ 0 đến list.Count - 1, vị trí của một tác phẩm trong danh sách ban đầu.
            var artwork = list[index];

            // Kiểm tra xem artwork đã được chọn trước đó chưa
            if (!randomArtworks.Contains(artwork))
            {
                randomArtworks.Add(artwork);//tác phẩm đã được chọn trước đó chưaa,chưa thêm tác phẩm vào danh sách randomArtworks
            }
        }

        return randomArtworks;
    }


    [HttpGet("recent-likes-summary")]
    public async Task<IActionResult> GetRecentArtworkLikesSummary()
    {
        try
        {
            // Lấy ngày hiện tại
            var currentDate = DateTime.UtcNow;

            // Lấy ngày 7 ngày trước
            var sevenDaysAgo = currentDate.AddDays(-7);

            // Lấy danh sách ngày trong khoảng từ ngày 7 ngày trước đến ngày hiện tại
            var dateRange = Enumerable.Range(0, 7).Select(offset => currentDate.AddDays(-offset).Date).ToList();

            // Tạo danh sách để lưu trữ kết quả
            var likeSummary = new List<ArtworkLikesByDate>();

            // Duyệt qua từng ngày trong khoảng thời gian
            foreach (var date in dateRange)
            {

                // Lấy thông tin của các artworks được tạo vào ngày đó
                var artworks = await _context.Artworks
                    .Where(a => a.DateCreated.Date == date)
                    .ToListAsync();

                // Tính tổng lượng like của các artworks
                var likes = artworks.Sum(a => a.Likes);

                // Thêm thông tin của từng artwork vào danh sách kết quả
                foreach (var artwork in artworks)
                {
                    likeSummary.Add(new ArtworkLikesByDate
                    {
                        Date = date,
                        likes = likes,
                        ArtworkID = artwork.ArtworkID,
                        ArtworkName = artwork.ArtworkName
                    });
                }

            }

            return Ok(likeSummary);
        }
        catch (Exception ex)
        {
            // Xử lý lỗi nếu có
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing your request.");
        }
    }

    public class ArtworkLikesByDate
    {
        public DateTime Date { get; set; }
        public int likes { get; set; }

        public int ArtworkID { get; set; }
        public string ArtworkName { get; set; }
    }




    [HttpDelete("{artworkId}")]
    public async Task<IActionResult> DeleteArtwork(int artworkId)
    {

        // Kiểm tra xem tác phẩm tồn tại hay không
        var artwork = await _context.Artworks.FindAsync(artworkId);
        if (artwork == null)
        {
            return NotFound("Artwork not found.");
        }

        // Xóa các dữ liệu liên quan trước
        // Ví dụ: Xóa các bình luận liên quan đến tác phẩm
        var comments = await _context.Comments.Where(c => c.ArtWorkID == artworkId).ToListAsync();
        _context.Comments.RemoveRange(comments);

        var artworkTags = await _context.ArtworkTag.Where(at => at.ArtworkID == artworkId).ToListAsync();
        _context.ArtworkTag.RemoveRange(artworkTags);

        // Ví dụ: Xóa các báo cáo liên quan đến tác phẩm
        var reports = await _context.Reports.Where(r => r.ReportedCreatorID == artworkId).ToListAsync();
        _context.Reports.RemoveRange(reports);
        var orderdetail = await _context.OrderDetail.Where(or => or.ArtWorkID == artworkId).ToListAsync();
        _context.OrderDetail.RemoveRange(orderdetail);

        // Ví dụ: Xóa các thông báo liên quan đến tác phẩm
        var notifications = await _context.Notification.Where(n => n.ArtWorkID == artworkId).ToListAsync();
        _context.Notification.RemoveRange(notifications);
        // Tiếp tục xóa các dữ liệu liên quan khác nếu cần
        await _context.SaveChangesAsync();
        // Sau khi xóa các dữ liệu liên quan, xóa tác phẩm
        _context.Artworks.Remove(artwork);
        await _context.SaveChangesAsync();

        return Ok("Artwork deleted successfully.");
    }

}