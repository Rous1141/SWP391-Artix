using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("SendEmail")]
        public ActionResult SendEmail(EmailModel emailData)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(emailData.FromEmail),
                Subject = emailData.Subject,
                IsBodyHtml = true,
                Body = $@"
                        <html>
                                <div style=""text-align: center;"">
    <!-- Button HTML (to Trigger Modal) -->
    <a href=""#myModal"" class=""trigger-btn"" data-toggle=""modal"" style=""display: inline-block; margin: 100px auto;"">Click to Open Confirm Modal</a>
</div>

<!-- Modal HTML -->
<div id=""myModal"" class=""modal fade"" style=""display: none;"">
    <div class=""modal-dialog modal-confirm"" style=""margin-top: 80px;"">
        <div class=""modal-content"" style=""padding: 20px; border-radius: 5px; border: none;"">
            <div class=""modal-header"" style=""border-bottom: none; position: relative;"">
                <div class=""icon-box"" style=""color: #fff; position: absolute; margin: 0 auto; left: 0; right: 0; top: -70px; width: 95px; height: 95px; border-radius: 50%; z-index: 9; background: #82ce34; padding: 15px; text-align: center; box-shadow: 0px 2px 2px rgba(0, 0, 0, 0.1);"">
                    <i class=""material-icons"" style=""font-size: 58px; position: relative; top: 3px;"">&#xE876;</i>
                </div>
                <h4 class=""modal-title w-100"" style=""text-align: center; font-size: 26px; margin: 30px 0 -15px;"">Awesome!</h4>
            </div>
            <div class=""modal-body"" style=""text-align: center;"">
                <p style=""font-size: 14px; color: #636363;"">Your booking has been confirmed. Check your email for details.</p>
            </div>
            <div class=""modal-footer"" style=""border: none; text-align: center; border-radius: 5px; font-size: 13px;"">
                <button class=""btn btn-success btn-block"" data-dismiss=""modal"" style=""color: #fff; border-radius: 4px; background: #82ce34; text-decoration: none; transition: all 0.4s; line-height: normal; border: none;"">OK</button>
            </div>
        </div>
    </div>
</div>

                        </html>"
            };

            foreach (var toEmail in emailData.ToEmails.Split(";"))
            {
                message.To.Add(new MailAddress(toEmail));
            }

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("anh0180666@huce.edu.vn", "Anh686868@"),
                EnableSsl = true,
            };

            smtp.Send(message);

            return Ok("Email Sent!");
        }
    }
                
}
