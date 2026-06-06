using AutoMapper;
using Store.DAL;

namespace Store.BLL
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Category -> CategoryReadDto
            CreateMap<Category, CategoryReadDto>();

            // CategoryCreateDto -> Category
            CreateMap<CategoryCreateDto, Category>();

            // CategoryEditDto -> Category
            CreateMap<CategoryEditDto, Category>();
        }
    }

    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Product -> ProductReadDto (flatten category name)
            CreateMap<Product, ProductReadDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            // ProductCreateDto -> Product
            CreateMap<ProductCreateDto, Product>();

            // ProductEditDto -> Product
            CreateMap<ProductEditDto, Product>();
        }
    }

    public class CartProfile : Profile
    {
        public CartProfile()
        {
            // CartItem -> CartItemReadDto (flatten product info)
            CreateMap<CartItem, CartItemReadDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product != null ? src.Product.Price : 0))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : null));
        }
    }

    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // OrderItem -> OrderItemReadDto
            CreateMap<OrderItem, OrderItemReadDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null));

            // Order -> OrderReadDto
            CreateMap<Order, OrderReadDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
