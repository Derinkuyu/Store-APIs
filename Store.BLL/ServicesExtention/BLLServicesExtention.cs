using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Store.BLL
{
    public static class BLLServicesExtention
    {
        public static void AddBLLServices(this IServiceCollection services)
        {
            /*------------------------------------------------------------------*/
            // Managers
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<ICartManager, CartManager>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IImageManager, ImageManager>();
            /*------------------------------------------------------------------*/
            // Error mapper (used by ImageManager which still validates internally)
            services.AddScoped<IErrorMapper, ErrorMapper>();
            /*------------------------------------------------------------------*/
            // FluentValidation validators — used by the MVC pipeline (AddFluentValidationAutoValidation)
            services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
            services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
            services.AddScoped<IValidator<CategoryCreateDto>, CategoryCreateDtoValidator>();
            services.AddScoped<IValidator<CategoryEditDto>, CategoryEditDtoValidator>();
            services.AddScoped<IValidator<ProductCreateDto>, ProductCreateDtoValidator>();
            services.AddScoped<IValidator<ProductEditDto>, ProductEditDtoValidator>();
            services.AddScoped<IValidator<CartItemAddDto>, CartItemAddDtoValidator>();
            services.AddScoped<IValidator<CartItemUpdateDto>, CartItemUpdateDtoValidator>();
            services.AddScoped<IValidator<ImageUploadDto>, ImageUploadDtoValidator>();
            /*------------------------------------------------------------------*/
            // AutoMapper — scans all Profile classes in this assembly
            services.AddAutoMapper(typeof(CategoryProfile).Assembly);
        }
    }
}
