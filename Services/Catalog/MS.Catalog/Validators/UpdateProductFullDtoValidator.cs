using FluentValidation;
using MS.Catalog.Dtos.ProductDtos;

namespace MS.Catalog.Validators
{
    public class UpdateProductFullDtoValidator : AbstractValidator<UpdateProductFullDto>
    {
        public UpdateProductFullDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId boş olamaz")
                .Length(24).WithMessage("ProductId 24 karakter olmalıdır");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Ürün adı boş olamaz")
                .MaximumLength(200).WithMessage("Ürün adı 200 karakterden uzun olamaz");

            RuleFor(x => x.ProductPrice)
                .GreaterThan(0).WithMessage("Fiyat 0’dan büyük olmalıdır");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kategori seçimi zorunludur");

            // Opsiyonel: Görsel ve açıklama validation
            RuleFor(x => x.ProductImageUrl)
                .NotEmpty().WithMessage("Kapak görseli boş olamaz");

            RuleFor(x => x.ProductDescription)
                .NotEmpty().WithMessage("Ürün açıklaması boş olamaz");
        }
    }
}
