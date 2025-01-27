using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.OfferDiscountDtos;
using MS.WebUI.Services.CatalogServices.OfferDiscountServices;
using Newtonsoft.Json;

namespace MS.WebUI.ViewComponents.DefaultViewComponents
{
    public class _OfferDiscountDefaultComponentPartial : ViewComponent
    {
        private readonly IOfferDiscountService _offerDiscountService;

        public _OfferDiscountDefaultComponentPartial(IOfferDiscountService offerDiscountService)
        {
            _offerDiscountService = offerDiscountService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _offerDiscountService.GetAllOfferDiscountAsync();
            return View(values);
        }
    }
}
