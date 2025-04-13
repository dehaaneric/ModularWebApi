using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularWebApi.Core;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ModularWebApi.Plugin.Module1
{
    public class Module1Plugin : PluginBase
    {
        public override string Name => "Module1";
        public override string Description => "Sample module demonstrating plugin functionality";

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register module-specific services
            //services.AddDbContext<Module1DbContext>(options =>
            //    options.UseSqlServer(
            //        configuration.GetConnectionString("DefaultConnection"),
            //        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Module1")));

            services.AddScoped<IModule1Service, Module1Service>();
        }

        public override void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            // Register module-specific endpoints
            var group = endpoints.MapGroup("/api/module1").WithTags("Module1");

            //group.MapGet("/items", GetAllItemsAsync);
            //group.MapGet("/items/{id}", GetItemByIdAsync);
            //group.MapPost("/items", CreateItemAsync);
            // Add other endpoints...
        }

        // API endpoint handlers
        //private async Task<IResult> GetAllItemsAsync(IModule1Service service)
        //{
        //    var items = await service.GetItemsAsync();
        //    return Results.Ok(items);
        //}

        //private async Task<IResult> GetItemByIdAsync(int id, IModule1Service service)
        //{
        //    var item = await service.GetItemByIdAsync(id);
        //    if (item == null)
        //        return Results.NotFound();

        //    return Results.Ok(item);
        //}

        //private async Task<IResult> CreateItemAsync(ItemDto dto, IModule1Service service)
        //{
        //    var item = await service.CreateItemAsync(dto);
        //    return Results.Created($"/api/module1/items/{item.Id}", item);
        //}
    }

    // Step 6: DbContext for the plugin module

    //public class Module1DbContext : DbContext
    //{
    //    public Module1DbContext(DbContextOptions<Module1DbContext> options)
    //        : base(options)
    //    {
    //    }
    //    public DbSet<Item> Items { get; set; } = null!;

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        // Set schema for all entities in this context
    //        modelBuilder.HasDefaultSchema("Module1");

    //        // Configure entities
    //        modelBuilder.Entity<Item>(entity =>
    //        {
    //            entity.HasKey(e => e.Id);
    //            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
    //            // Additional configuration...
    //        });
    //    }
    //}
}
