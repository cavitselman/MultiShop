using MS.WebUI.Handlers;
using MS.WebUI.Services.BasketServices;
using MS.WebUI.Services.CargoServices.CargoCompanyServices;
using MS.WebUI.Services.CargoServices.CargoCustomerServices;
using MS.WebUI.Services.CargoServices.CargoDetailServices;
using MS.WebUI.Services.CatalogServices.AboutServices;
using MS.WebUI.Services.CatalogServices.BrandServices;
using MS.WebUI.Services.CatalogServices.CategoryServices;
using MS.WebUI.Services.CatalogServices.ContactServices;
using MS.WebUI.Services.CatalogServices.FeatureServices;
using MS.WebUI.Services.CatalogServices.FeatureSliderServices;
using MS.WebUI.Services.CatalogServices.OfferDiscountServices;
using MS.WebUI.Services.CatalogServices.ProductDetailServices;
using MS.WebUI.Services.CatalogServices.ProductImageServices;
using MS.WebUI.Services.CatalogServices.ProductServices;
using MS.WebUI.Services.CatalogServices.SpecialOfferServices;
using MS.WebUI.Services.CommentServices;
using MS.WebUI.Services.Concrete;
using MS.WebUI.Services.DiscountServices;
using MS.WebUI.Services.Interfaces;
using MS.WebUI.Services.MessageServices;
using MS.WebUI.Services.OrderServices.OrderAddressServices;
using MS.WebUI.Services.OrderServices.OrderDetailServices;
using MS.WebUI.Services.OrderServices.OrderOrderingServices;
using MS.WebUI.Services.StatisticServices.CatalogStatisticServices;
using MS.WebUI.Services.StatisticServices.DiscountStatisticServices;
using MS.WebUI.Services.StatisticServices.MessageStatisticServices;
using MS.WebUI.Services.StatisticServices.UserStatisticServices;
using MS.WebUI.Services.UserIdentityServices;
using MS.WebUI.Settings;

namespace MS.WebUI.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, ServiceApiSettings values)
        {
            #region Identity Service
            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(values.IdentityServerUrl);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IUserIdentityService, UserIdentityService>(opt =>
            {
                opt.BaseAddress = new Uri(values.IdentityServerUrl);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IUserStatisticService, UserStatisticService>(opt =>
            {
                opt.BaseAddress = new Uri(values.IdentityServerUrl);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
            #endregion

            #region Catalog Microservice
            AddCatalogHttpClients(services, values);
            #endregion

            #region Comment Microservice
            AddCommentHttpClients(services, values);
            #endregion

            #region Basket Microservice
            AddBasketHttpClient(services, values);
            #endregion

            #region Discount Microservice
            AddDiscountHttpClients(services, values);
            #endregion

            #region Order Microservice
            AddOrderHttpClients(services, values);
            #endregion

            #region Message Microservice
            AddMessageHttpClients(services, values);
            #endregion

            #region Cargo Microservice
            AddCargoHttpClients(services, values);
            #endregion

            return services;
        }

        private static void AddCatalogHttpClients(IServiceCollection services, ServiceApiSettings values)
        {
            services.AddHttpClient<ICatalogStatisticService, CatalogStatisticService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<ICategoryService, CategoryService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IProductService, ProductService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<ISpecialOfferService, SpecialOfferService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IFeatureSliderService, FeatureSliderService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IFeatureService, FeatureService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IOfferDiscountService, OfferDiscountService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IBrandService, BrandService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IAboutService, AboutService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IProductDetailService, ProductDetailService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IProductImageService, ProductImageService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IContactService, ContactService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();
        }

        private static void AddCommentHttpClients(IServiceCollection services, ServiceApiSettings values)
        {
            services.AddHttpClient<ICommentService, CommentService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Comment.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            //services.AddHttpClient<ICommentStatisticService, CommentStatisticService>(opt =>
            //{
            //    opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Comment.Path}");
            //}).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }

        private static void AddBasketHttpClient(IServiceCollection services, ServiceApiSettings values)
        {
            services.AddHttpClient<IBasketService, BasketService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }

        private static void AddDiscountHttpClients(IServiceCollection services, ServiceApiSettings values)
        {
            services.AddHttpClient<IDiscountService, DiscountService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IDiscountStatisticService, DiscountStatisticService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Discount.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }

        private static void AddOrderHttpClients(IServiceCollection services, ServiceApiSettings values)
        {
            services.AddHttpClient<IOrderDetailService, OrderDetailService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IOrderAddressService, OrderAddressService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IOrderOrderingService, OrderOrderingService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }

        private static void AddMessageHttpClients(IServiceCollection services, ServiceApiSettings values)
        {
            services.AddHttpClient<IMessageService, MessageService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Message.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IMessageStatisticService, MessageStatisticService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Message.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }

        private static void AddCargoHttpClients(IServiceCollection services, ServiceApiSettings values)
        {
            services.AddHttpClient<ICargoCompanyService, CargoCompanyService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Cargo.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<ICargoCustomerService, CargoCustomerService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Cargo.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<ICargoDetailService, CargoDetailService>(opt =>
            {
                opt.BaseAddress = new Uri($"{values.OcelotUrl}/{values.Cargo.Path}");
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }
    }
}
