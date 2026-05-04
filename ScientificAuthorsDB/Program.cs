using Microsoft.EntityFrameworkCore;
using ScientificAuthorsDB.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавяме поддръжка за MVC (Controllers и Views)
builder.Services.AddControllersWithViews();

// ТУК свързваме ApplicationDbContext с адреса от appsettings.json!
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();