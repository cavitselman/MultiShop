using MS.DtoL.DiscountDtos;

namespace MS.WebUI.Services.DiscountServices
{
    public interface IDiscountService
    {
        Task<GetDiscountCodeDetailByCode> GetDiscountCode(string code);
    }
}
