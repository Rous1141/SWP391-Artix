using backend.Entities;
using backend.Entities.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace backend.Service
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        public VnPayService(IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _memoryCache = memoryCache;
        }
        public string CreatePaymentUrl(OrderDetail model, HttpContext context)
        {
            _memoryCache.Set($"Order_{model.ArtWorkID}", model, TimeSpan.FromMinutes(10));
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (model.Price * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.ArtWorkID}");
            pay.AddRequestData("vnp_OrderType", "VNPay");
            pay.AddRequestData("vnp_ReturnUrl", $"{urlCallBack}?ArtWorkID={model.ArtWorkID}");
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }







        public OrderDetail GetPaymentModelFromCache(int ArtWorkID)
        {
            return _memoryCache.Get<OrderDetail>($"Order_{ArtWorkID}");
        }
        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
            return response;
        }



        public string CreatePaymentUrlForPackage(Package package, HttpContext context)
        {
            _memoryCache.Set($"Package_{package.PackageID}", package, TimeSpan.FromMinutes(10));
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallbackPackage:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", (package.PackagePrice * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{package.PackageID}");
            pay.AddRequestData("vnp_OrderType", "VNPay");
            pay.AddRequestData("vnp_ReturnUrl", $"{urlCallBack}?PackageId={package.PackageID}");
            pay.AddRequestData("vnp_TxnRef", tick);
            var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }
        public Package GetPaymentModelFromCachePackage(int PackageID)
        {
            return _memoryCache.Get<Package>($"Package_{PackageID}");
        }



    }
}
