using MVC_Practical.BAL;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<internHelper>();
builder.Services.AddScoped<topicHelper>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//configuer postgresSQl
builder.Services.AddSingleton<NpgsqlConnection>((ServiceProvider) => {
    var connectionString = ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString("interDemo");
    return new NpgsqlConnection(connectionString);  
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
