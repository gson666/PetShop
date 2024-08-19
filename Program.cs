using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PetSite.Data;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession();

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<PetContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionDB")));


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var con = scope.ServiceProvider.GetRequiredService<PetContext>();
    con.Database.EnsureDeleted();
    con.Database.EnsureCreated();
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseSession();


    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PetCare}/{action=Index}/{id?}");

app.Run();
