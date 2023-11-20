using FoodShop_SWP.Common;
using FoodShop_SWP.Models.Common;

namespace FoodShop_SWP.Ultis
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
