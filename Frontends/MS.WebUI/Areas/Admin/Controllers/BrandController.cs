using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.BrandDtos;
using MS.WebUI.Services.CatalogServices.BrandServices;
using Newtonsoft.Json;
using System.Text;

namespace MS.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }
        void BrandViewbagList()
        {
            ViewBag.v0 = "Marka İşlemleri";
            ViewBag.v1 = "Ana Sayfa";
            ViewBag.v2 = "Markalar";
            ViewBag.v3 = "Marka Listesi";
        }

        public async Task<IActionResult> Index()
        {
            BrandViewbagList();
            var values = await _brandService.GetAllBrandAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateBrand()
        {
            BrandViewbagList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand(CreateBrandDto createBrandDto)
        {
            await _brandService.CreateBrandAsync(createBrandDto);
            return Redirect("/Admin/Brand/Index");
        }

        public async Task<IActionResult> DeleteBrand(string id)
        {
            await _brandService.DeleteBrandAsync(id);
            return Redirect("/Admin/Brand/Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBrand(string id)
        {
            BrandViewbagList();
            var values = await _brandService.GetByIdBrandAsync(id);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBrand(UpdateBrandDto updateBrandDto)
        {
            await _brandService.UpdateBrandAsync(updateBrandDto);
            return Redirect("/Admin/Brand/Index");
        }
    }
}
