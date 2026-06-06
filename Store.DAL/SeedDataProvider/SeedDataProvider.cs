using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Store.DAL
{
    public static class SeedDataProvider
    {
        /*------------------------------------------------------------------*/
        #region Roles

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = ["Admin", "User"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        #endregion

        /*------------------------------------------------------------------*/
        #region Users

        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@store.com") is null)
            {
                var adminUser = new AppUser
                {
                    DisplayName = "Store Admin",
                    UserName = "admin@store.com",
                    Email = "admin@store.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        public static async Task SeedRegularUsersAsync(UserManager<AppUser> userManager)
        {
            var users = new[]
            {
                new { DisplayName = "John Smith",   Email = "john@store.com",   Password = "User@123" },
                new { DisplayName = "Sara Ahmed",   Email = "sara@store.com",   Password = "User@123" },
                new { DisplayName = "Ali Hassan",   Email = "ali@store.com",    Password = "User@123" },
            };

            foreach (var u in users)
            {
                if (await userManager.FindByEmailAsync(u.Email) is null)
                {
                    var user = new AppUser
                    {
                        DisplayName = u.DisplayName,
                        UserName = u.Email,
                        Email = u.Email,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, u.Password);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, "User");
                }
            }
        }

        #endregion

        /*------------------------------------------------------------------*/
        #region Categories

        public static async Task SeedCategoriesAsync(AppDbContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            var categories = new List<Category>
            {
                new()
                {
                    Name        = "Electronics",
                    Description = "Smartphones, laptops, tablets, and all electronic gadgets.",
                    ImageUrl    = "/images/categories/electronics.jpg",
                    CreatedAt   = DateTime.UtcNow
                },
                new()
                {
                    Name        = "Clothing",
                    Description = "Men's, women's, and children's fashion and apparel.",
                    ImageUrl    = "/images/categories/clothing.jpg",
                    CreatedAt   = DateTime.UtcNow
                },
                new()
                {
                    Name        = "Home & Kitchen",
                    Description = "Furniture, appliances, cookware, and home décor.",
                    ImageUrl    = "/images/categories/home-kitchen.jpg",
                    CreatedAt   = DateTime.UtcNow
                },
                new()
                {
                    Name        = "Books",
                    Description = "Novels, textbooks, self-help, and academic resources.",
                    ImageUrl    = "/images/categories/books.jpg",
                    CreatedAt   = DateTime.UtcNow
                },
                new()
                {
                    Name        = "Sports & Fitness",
                    Description = "Gym equipment, sportswear, outdoor gear, and accessories.",
                    ImageUrl    = "/images/categories/sports.jpg",
                    CreatedAt   = DateTime.UtcNow
                },
                new()
                {
                    Name        = "Beauty & Personal Care",
                    Description = "Skincare, haircare, fragrances, and grooming essentials.",
                    ImageUrl    = "/images/categories/beauty.jpg",
                    CreatedAt   = DateTime.UtcNow
                },
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }

        #endregion

        /*------------------------------------------------------------------*/
        #region Products

        public static async Task SeedProductsAsync(AppDbContext context)
        {
            if (await context.Products.AnyAsync()) return;

            // Fetch seeded category IDs by name
            var electronics   = await context.Categories.FirstAsync(c => c.Name == "Electronics");
            var clothing       = await context.Categories.FirstAsync(c => c.Name == "Clothing");
            var homeKitchen    = await context.Categories.FirstAsync(c => c.Name == "Home & Kitchen");
            var books          = await context.Categories.FirstAsync(c => c.Name == "Books");
            var sports         = await context.Categories.FirstAsync(c => c.Name == "Sports & Fitness");
            var beauty         = await context.Categories.FirstAsync(c => c.Name == "Beauty & Personal Care");

            var now = DateTime.UtcNow;

            var products = new List<Product>
            {
                /*── Electronics ─────────────────────────────────────────────*/
                new()
                {
                    Name        = "iPhone 15 Pro",
                    Description = "Apple iPhone 15 Pro with A17 Pro chip, 48MP camera, and titanium design.",
                    Price       = 999.99m,
                    Stock       = 50,
                    ImageUrl    = "/images/products/iphone15pro.jpg",
                    CategoryId  = electronics.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Samsung Galaxy S24 Ultra",
                    Description = "Samsung Galaxy S24 Ultra with S Pen, 200MP camera, and Snapdragon 8 Gen 3.",
                    Price       = 1199.99m,
                    Stock       = 35,
                    ImageUrl    = "/images/products/galaxy-s24-ultra.jpg",
                    CategoryId  = electronics.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "MacBook Pro 14\" M3",
                    Description = "Apple MacBook Pro 14-inch with M3 Pro chip, 18GB RAM, and 512GB SSD.",
                    Price       = 1999.99m,
                    Stock       = 20,
                    ImageUrl    = "/images/products/macbook-pro-14.jpg",
                    CategoryId  = electronics.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Sony WH-1000XM5 Headphones",
                    Description = "Industry-leading noise cancelling wireless headphones with 30-hour battery life.",
                    Price       = 349.99m,
                    Stock       = 75,
                    ImageUrl    = "/images/products/sony-xm5.jpg",
                    CategoryId  = electronics.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "iPad Air 11\" M2",
                    Description = "Apple iPad Air with M2 chip, Liquid Retina display, and Apple Pencil Pro support.",
                    Price       = 599.99m,
                    Stock       = 40,
                    ImageUrl    = "/images/products/ipad-air-m2.jpg",
                    CategoryId  = electronics.Id,
                    CreatedAt   = now
                },

                /*── Clothing ────────────────────────────────────────────────*/
                new()
                {
                    Name        = "Classic Slim-Fit Oxford Shirt",
                    Description = "Premium 100% cotton slim-fit Oxford shirt, available in multiple colors.",
                    Price       = 49.99m,
                    Stock       = 200,
                    ImageUrl    = "/images/products/oxford-shirt.jpg",
                    CategoryId  = clothing.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Women's Casual Hoodie",
                    Description = "Soft fleece casual hoodie for women, perfect for everyday wear.",
                    Price       = 39.99m,
                    Stock       = 150,
                    ImageUrl    = "/images/products/womens-hoodie.jpg",
                    CategoryId  = clothing.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Levi's 501 Original Jeans",
                    Description = "Iconic straight-fit jeans made from 100% cotton denim.",
                    Price       = 69.99m,
                    Stock       = 120,
                    ImageUrl    = "/images/products/levis-501.jpg",
                    CategoryId  = clothing.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Nike Air Max 270",
                    Description = "Lifestyle sneakers with Max Air unit for all-day comfort.",
                    Price       = 129.99m,
                    Stock       = 90,
                    ImageUrl    = "/images/products/nike-air-max-270.jpg",
                    CategoryId  = clothing.Id,
                    CreatedAt   = now
                },

                /*── Home & Kitchen ──────────────────────────────────────────*/
                new()
                {
                    Name        = "Instant Pot Duo 7-in-1",
                    Description = "Multi-use pressure cooker: pressure cook, slow cook, rice cooker, steamer, and more.",
                    Price       = 89.99m,
                    Stock       = 60,
                    ImageUrl    = "/images/products/instant-pot-duo.jpg",
                    CategoryId  = homeKitchen.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Nespresso Vertuo Pop Coffee Maker",
                    Description = "Compact coffee maker with Centrifusion technology for authentic espresso.",
                    Price       = 119.99m,
                    Stock       = 45,
                    ImageUrl    = "/images/products/nespresso-vertuo.jpg",
                    CategoryId  = homeKitchen.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "KitchenAid Stand Mixer",
                    Description = "5-quart tilt-head stand mixer with 10-speed settings and multiple attachments.",
                    Price       = 449.99m,
                    Stock       = 25,
                    ImageUrl    = "/images/products/kitchenaid-mixer.jpg",
                    CategoryId  = homeKitchen.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "IKEA KALLAX Shelf Unit",
                    Description = "Versatile 4-cube shelf unit, perfect for living rooms and home offices.",
                    Price       = 79.99m,
                    Stock       = 30,
                    ImageUrl    = "/images/products/kallax-shelf.jpg",
                    CategoryId  = homeKitchen.Id,
                    CreatedAt   = now
                },

                /*── Books ───────────────────────────────────────────────────*/
                new()
                {
                    Name        = "Clean Code by Robert C. Martin",
                    Description = "A handbook of agile software craftsmanship — essential for every developer.",
                    Price       = 34.99m,
                    Stock       = 100,
                    ImageUrl    = "/images/products/clean-code.jpg",
                    CategoryId  = books.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "The Pragmatic Programmer",
                    Description = "Classic guide to software development best practices and professional growth.",
                    Price       = 39.99m,
                    Stock       = 80,
                    ImageUrl    = "/images/products/pragmatic-programmer.jpg",
                    CategoryId  = books.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Atomic Habits by James Clear",
                    Description = "An easy and proven way to build good habits and break bad ones.",
                    Price       = 19.99m,
                    Stock       = 200,
                    ImageUrl    = "/images/products/atomic-habits.jpg",
                    CategoryId  = books.Id,
                    CreatedAt   = now
                },

                /*── Sports & Fitness ────────────────────────────────────────*/
                new()
                {
                    Name        = "Adjustable Dumbbell Set (5–52.5 lbs)",
                    Description = "Space-saving adjustable dumbbells replacing 15 sets of weights.",
                    Price       = 299.99m,
                    Stock       = 40,
                    ImageUrl    = "/images/products/adjustable-dumbbells.jpg",
                    CategoryId  = sports.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Yoga Mat Premium Non-Slip",
                    Description = "6mm thick eco-friendly TPE yoga mat with alignment lines.",
                    Price       = 34.99m,
                    Stock       = 150,
                    ImageUrl    = "/images/products/yoga-mat.jpg",
                    CategoryId  = sports.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Under Armour UA Tech T-Shirt",
                    Description = "Ultra-soft, natural feel performance t-shirt with moisture-wicking technology.",
                    Price       = 24.99m,
                    Stock       = 180,
                    ImageUrl    = "/images/products/ua-tech-tshirt.jpg",
                    CategoryId  = sports.Id,
                    CreatedAt   = now
                },

                /*── Beauty & Personal Care ──────────────────────────────────*/
                new()
                {
                    Name        = "CeraVe Moisturizing Cream",
                    Description = "Rich, non-greasy moisturizing cream for normal to dry skin with ceramides.",
                    Price       = 16.99m,
                    Stock       = 300,
                    ImageUrl    = "/images/products/cerave-cream.jpg",
                    CategoryId  = beauty.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Dyson Airwrap Multi-Styler",
                    Description = "Complete hair styler using controlled airflow instead of extreme heat.",
                    Price       = 599.99m,
                    Stock       = 20,
                    ImageUrl    = "/images/products/dyson-airwrap.jpg",
                    CategoryId  = beauty.Id,
                    CreatedAt   = now
                },
                new()
                {
                    Name        = "Neutrogena Hydro Boost Serum",
                    Description = "Hyaluronic acid facial serum for intense hydration and plump, bouncy skin.",
                    Price       = 24.99m,
                    Stock       = 250,
                    ImageUrl    = "/images/products/neutrogena-hydro-boost.jpg",
                    CategoryId  = beauty.Id,
                    CreatedAt   = now
                },
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        #endregion
    }
}
