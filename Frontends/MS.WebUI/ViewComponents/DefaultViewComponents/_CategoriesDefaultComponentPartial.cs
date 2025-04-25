using Microsoft.AspNetCore.Mvc;
using MS.DtoL.CatalogDtos.CategoryDtos;
using MS.WebUI.Services.CatalogServices.CategoryServices;
using Newtonsoft.Json;

namespace MS.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CategoriesDefaultComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public _CategoriesDefaultComponentPartial(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _categoryService.GetCategoriesWithProductCountAsync();
            return View(values);
        }
    }
}
