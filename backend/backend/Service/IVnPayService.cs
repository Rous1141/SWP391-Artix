using backend.Entities;
using backend.Entities.DTO;
using Microsoft.AspNetCore.Http;

namespace backend.Service
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(OrderDetail model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
        OrderDetail GetPaymentModelFromCache(int ArtWorkID);
        string CreatePaymentUrlForPackage(Package package, HttpContext context);
        Package GetPaymentModelFromCachePackage(int PackageID);
    }
}
