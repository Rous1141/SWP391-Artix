using backend.Entities;
using backend.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    public class VnpayPaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IVnPayService _vnPayService;

        public VnpayPaymentController(ApplicationDbContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }

        [HttpPost("Payment")]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] OrderDetail model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(url);
        }

        [HttpGet("Check")]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            int ArtWorkID = int.Parse(HttpContext.Request.Query["ArtWorkID"]);

            // Kiểm tra response
            if (response == null)
            {
                // Nếu response là null, điều này có thể dẫn đến lỗi
                return Redirect("~/fail-page");
            }

            // Lấy model từ cache
            var model = _vnPayService.GetPaymentModelFromCache(ArtWorkID);

            // Kiểm tra model
            if (model == null)
            {
                // Nếu model là null, không thể thêm vào OrderDetail
                return Redirect("~/fail-page");
            }

            if (response.Success && response.VnPayResponseCode == "00")
            {
                var artwork = await _context.Artworks
           .Include(a => a.ArtworkTag)
           .Select(a => new Artworks
           {
               ArtworkID = a.ArtworkID,
               CreatorID = a.CreatorID,
               ArtworkName = a.ArtworkName
           })
           .FirstOrDefaultAsync(a => a.ArtworkID == model.ArtWorkID);

                if (artwork == null)
                {
                    return NotFound();
                }
                // Thêm model vào OrderDetail và lưu thay đổi
                _context.OrderDetail.Add(model);
                await _context.SaveChangesAsync();
                var message = new MailMessage()
                {
                    From = new MailAddress("anh0180666@huce.edu.vn"),
                    Subject = "Orders",
                    IsBodyHtml = true,

                    Body = $@"
                            <html>
                                <head>
                                    <style>
                                        body {{
                                            text-align: center;
                                            padding: 40px 0;
                                            background: #EBF0F5;
                                        }}
                                        h1 {{
                                            color: #88B04B;
                                            font-family: 'Nunito Sans', 'Helvetica Neue', sans-serif;
                                            font-weight: 900;
                                            font-size: 40px;
                                            margin-bottom: 10px;
                                        }}
                                        p {{
                                            color: #404F5E;
                                            font-family: 'Nunito Sans', 'Helvetica Neue', sans-serif;
                                            font-size:20px;
                                            margin: 0;
                                        }}
                                        i {{
                                            color: #9ABC66;
                                            font-size: 100px;
                                            line-height: 200px;
                                            margin-left:-15px;
                                        }}
                                        .card {{
                                            background: white;
                                            padding: 60px;
                                            border-radius: 4px;
                                            box-shadow: 0 2px 3px #C8D0D8;
                                            display: inline-block;
                                            margin: 0 auto;
                                        }}
                                    </style>
                                </head>
                                <body>
                                    <div class='card'>
                                        <div style='border-radius:200px; height:200px; width:200px; background: #F8FAF5; margin:0 auto;'>
                                            <i class='checkmark'>✓</i>
                                        </div>
                                        <h1>Success Buy</h1> 
                                        <p>You have successfully purchased product <strong>{artwork.ArtworkName}</strong>, Thanks you!!!</p>
                                    </div>
                                </body>
                            </html>"
                };

                var messageSeller = new MailMessage()
                {
                    From = new MailAddress("anh0180666@huce.edu.vn"),
                    Subject = "Orders",
                    IsBodyHtml = true,

                    Body = $@"
                            <html>
                                <head>
                                    <style>
                                        body {{
                                            text-align: center;
                                            padding: 40px 0;
                                            background: #EBF0F5;
                                        }}
                                        h1 {{
                                            color: #88B04B;
                                            font-family: 'Nunito Sans', 'Helvetica Neue', sans-serif;
                                            font-weight: 900;
                                            font-size: 40px;
                                            margin-bottom: 10px;
                                        }}
                                        p {{
                                            color: #404F5E;
                                            font-family: 'Nunito Sans', 'Helvetica Neue', sans-serif;
                                            font-size:20px;
                                            margin: 0;
                                        }}
                                        i {{
                                            color: #9ABC66;
                                            font-size: 100px;
                                            line-height: 200px;
                                            margin-left:-15px;
                                        }}
                                        .card {{
                                            background: white;
                                            padding: 60px;
                                            border-radius: 4px;
                                            box-shadow: 0 2px 3px #C8D0D8;
                                            display: inline-block;
                                            margin: 0 auto;
                                        }}
                                    </style>
                                </head>
                                <body>
                                    <div class='card'>
                                        <div style='border-radius:200px; height:200px; width:200px; background: #F8FAF5; margin:0 auto;'>
                                            <i class='checkmark'>✓</i>
                                        </div>
                                        <h1>Success Sold</h1> 
                                        <p>You have successfully sold the product <strong>{artwork.ArtworkName}</strong>, Thanks you!!!</p>
                                    </div>
                                </body>
                            </html>"
                };
                Creator buyler = _context.Creators.Where(a => a.CreatorID == model.Order.BuyerID).FirstOrDefault();
                Creator seller = _context.Creators.Where(a => a.CreatorID == model.Order.SellerID).FirstOrDefault();
                message.To.Add(new MailAddress(buyler.Email)); // thay bằng email nguwoif mua
                messageSeller.To.Add(new MailAddress(seller.Email)); // select ra và thay bằng id người bán
                var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("anh0180666@huce.edu.vn", "Anh686868@"),
                    EnableSsl = true,
                };

                smtp.Send(message);
                smtp.Send(messageSeller);
                return Redirect($"http://localhost:3000/characters/artwordrecomment/artwork/{ArtWorkID}");
            }
            else
            {
                return Redirect($"http://localhost:3000/characters/artwordrecomment/artwork/{ArtWorkID}");
            }
        }



        //[HttpGet("CheckCurrentPackage")]
        //public async Task<IActionResult> PaymentCallbackPackage()
        //{
        //    var response = _vnPayService.PaymentExecute(Request.Query);
        //    int packageId = int.Parse(HttpContext.Request.Query["PackageID"]); // Lấy PackageID từ yêu cầu

        //    // Kiểm tra response
        //    if (response == null)
        //    {
        //        // Nếu response là null, điều này có thể dẫn đến lỗi
        //        return Redirect("~/fail-page");
        //    }

        //    // Lấy model từ cache
        //    var model = _vnPayService.GetPaymentModelFromCachePackage(packageId);

        //    // Kiểm tra model
        //    if (model == null)
        //    {
        //        // Nếu model là null, không thể thêm vào OrderDetail
        //        return Redirect("~/fail-page");
        //    }

        //    if (response.Success && response.VnPayResponseCode == "00")
        //    {
        //        var package = await _context.Package
           
        //   .Select(a => new Package
        //   {
        //       PackageID = a.PackageID,
        //       PackageName = a.PackageName,
        //       PackageDescription = a.PackageDescription,
        //       PackagePrice = a.PackagePrice
        //   })
        //   .FirstOrDefaultAsync(a => a.PackageID == model.PackageID);

        //        if (package == null)
        //        {
        //            return NotFound();
        //        }
        //        // Thêm model vào OrderDetail và lưu thay đổi
        //        _context.Package.Add(model);
        //        await _context.SaveChangesAsync();
                
               

               
        //        return Redirect("http://localhost:3000/characters/package");
        //    }
        //    else
        //    {
        //        return Redirect("http://localhost:3000/characters/package");
        //    }
        //}




    }
}
