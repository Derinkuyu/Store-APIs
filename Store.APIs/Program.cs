using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Store.BLL;
using Store.Common;
using Store.DAL;
using System.Text;


namespace Store.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            /*------------------------------------------------------------------*/
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value?.Errors.Count > 0)
                            .ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value!.Errors
                                    .Select(e => new Errors
                                    {
                                        Code = "ERR-VALIDATION",
                                        Description = e.ErrorMessage
                                    })
                                    .ToList()
                            );

                        var result = GeneralResult.FailureResult("Validation failed.", errors);
                        return new BadRequestObjectResult(result);
                    };
                });

            /*------------------------------------------------------------------*/
            builder.Services.AddDALServices(builder.Configuration);

            /*------------------------------------------------------------------*/
            builder.Services.AddBLLServices();

            /*------------------------------------------------------------------*/
            builder.Services.AddFluentValidationAutoValidation();

            /*------------------------------------------------------------------*/
            // JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("JWT");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
                };
            });

            /*------------------------------------------------------------------*/
            // Policy-Based Authorization
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy =>
                    policy.RequireRole("User"));
            });

            /*------------------------------------------------------------------*/
            // CORS
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            /*------------------------------------------------------------------*/
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "Store E-Commerce API",
                        Version = "v1",
                        Description = "E-Commerce REST API — N-Tier Architecture with JWT + FluentValidation"
                    };
                    return Task.CompletedTask;
                });
            });

            var app = builder.Build();

            /*------------------------------------------------------------------*/
            // Apply pending migrations & seed data on startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context     = services.GetRequiredService<AppDbContext>();
                    var roleManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<Microsoft.AspNetCore.Identity.IdentityRole>>();
                    var userManager = services.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<AppUser>>();

                    await context.Database.MigrateAsync();

                    await SeedDataProvider.SeedRolesAsync(roleManager);

                    await SeedDataProvider.SeedAdminUserAsync(userManager);
                    await SeedDataProvider.SeedRegularUsersAsync(userManager);

                    await SeedDataProvider.SeedCategoriesAsync(context);

                    await SeedDataProvider.SeedProductsAsync(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during migration or seeding.");
                }
            }


            /*------------------------------------------------------------------*/
            // HTTP pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options.Title = "Store E-Commerce API";
                    options.Theme = ScalarTheme.DeepSpace;
                    options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}
