using KostaTest.Domain.Repositories;
using KostaTest.Domain.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(
    serviceProvider => new DepartmentRepository(connectionString: connection));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>(
    serviceProvider => new EmployeeRepository(connectionString: connection));

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
