
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System.Threading.Tasks;
using TradeHub.Extenstion;
using TradeHub.Hubs;
using TradeHub.Mapping;
using TradeHub.MiddleWares;
using TradeHub.Repository;
using TradeHub.Service.Companies.Queries;
using TradHub.Core.Entity.Identity;
using TradHub.Core.Settings;
namespace TradeHub
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<GlobalProblemDetailsExceptionFilter>();
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                 System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });
            builder.Services.AddHttpContextAccessor();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSignalR();

            builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }));



            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            }
            );
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

                  builder.Services.AddMediatR(cfg =>
                 cfg.RegisterServicesFromAssembly(typeof(GetCompanyByIdQueryHandler).Assembly));

                            builder.Services.AddSwaggerGen(options =>
                            {
                                options.SwaggerDoc("v1", new OpenApiInfo
                                {
                                    Title = "TradeHub API",
                                    Version = "v1"
                                });

                                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                {
                                    Name = "Authorization",
                                    Type = SecuritySchemeType.Http,
                                    Scheme = "bearer",
                                    BearerFormat = "JWT",
                                    In = ParameterLocation.Header,
                                    Description = "Enter JWT token"
                                });

                                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityService(builder.Configuration);

            builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

           builder.Services.Configure<PaymobSettings>(
                builder.Configuration.GetSection("PaymobSettings"));

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<AppDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                logger.LogInformation("Starting database migration...");
                await _dbContext.Database.MigrateAsync();
                logger.LogInformation("Database migration completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database.");
                throw;
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseSerilogRequestLogging();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "TradeHub API v1");
                    options.RoutePrefix = string.Empty;
                });
                app.UseSerilogRequestLogging();
            }
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            
            app.MapControllers();

            app.MapHub<NotificationHub>("/hubs/notifications");

            app.Run();
        }
    }
}
