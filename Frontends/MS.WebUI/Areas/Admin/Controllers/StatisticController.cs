using Microsoft.AspNetCore.Mvc;
using MS.WebUI.Services.CommentServices;
using MS.WebUI.Services.StatisticServices.CatalogStatisticServices;
using MS.WebUI.Services.StatisticServices.DiscountStatisticServices;
using MS.WebUI.Services.StatisticServices.MessageStatisticServices;
using MS.WebUI.Services.StatisticServices.UserStatisticServices;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticController : Controller
    {
        private readonly ICatalogStatisticService _statisticService;
        private readonly IUserStatisticService _userStatisticService;
        private readonly ICommentService _commentService;
        private readonly IDiscountStatisticService _discountStatisticService;
        private readonly IMessageStatisticService _messageStatisticService;

        public StatisticController(ICatalogStatisticService statisticService, IUserStatisticService userStatisticService, ICommentService commentService, IDiscountStatisticService discountStatisticService, IMessageStatisticService messageStatisticService)
        {
            _statisticService = statisticService;
            _userStatisticService = userStatisticService;
            _commentService = commentService;
            _discountStatisticService = discountStatisticService;
            _messageStatisticService = messageStatisticService;
        }

        public async Task<IActionResult> Index()
        {
            var getBrandCount = await _statisticService.GetBrandCount();
            var getProductCount = await _statisticService.GetProductCount();
            var getCategoryCount = await _statisticService.GetCategoryCount();
            var getMaxPriceProductName = await _statisticService.GetMaxPriceProductName();
            var getMinPriceProductName = await _statisticService.GetMinPriceProductName();
            var getUserCount = await _userStatisticService.GetUserCount();
            var getTotalCommentCount = await _commentService.GetTotalCommentCount();
            var getActiveCommentCount = await _commentService.GetActiveCommentCount();
            var getPassiveCommentCount = await _commentService.GetPassiveCommentCount();
            var getDiscountCouponCount = await _discountStatisticService.GetDiscountCouponCount();
            var getMessageTotalCount = await _messageStatisticService.GetTotalMessageCount();
            //var getProductAvgPrice = await _statisticService.GetProductAvgPrice();

            ViewBag.getBrandCount = getBrandCount;
            ViewBag.getProductCount = getProductCount;
            ViewBag.getCategoryCount = getCategoryCount;
            ViewBag.getMaxPriceProductName = getMaxPriceProductName;
            ViewBag.getMinPriceProductName = getMinPriceProductName;
            ViewBag.getUserCount = getUserCount;
            ViewBag.getTotalCommentCount = getTotalCommentCount;
            ViewBag.getActiveCommentCount = getActiveCommentCount;
            ViewBag.getPassiveCommentCount = getPassiveCommentCount;
            ViewBag.getDiscountCouponCount = getDiscountCouponCount;
            ViewBag.getMessageTotalCount = getMessageTotalCount;
            //ViewBag.getProductAvgPrice = getProductAvgPrice;
            return View();
        }
    }
}
