using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiElementUI.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    /*options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost",
                                              "http://localhost:5173")
                                            .AllowAnyHeader()
                                            //.AllowAnyMethod()
                                            .WithMethods("PUT", "DELETE", "GET");
                      });*/

    options.AddPolicy("WelcomePolicy",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost",
                                              "http://localhost:5173")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                                            .WithMethods("PUT", "DELETE", "GET", "POST");
                      });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KitchenWare")));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Add services to the container.
//builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseDefaultFiles();
//app.UseStaticFiles();

//app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "Products",
//    pattern: "{controller=Products}/{action=Index}/{id?}");

app.MapControllers();

app.Run();