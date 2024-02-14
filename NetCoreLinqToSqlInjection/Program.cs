using NetCoreLinqToSqlInjection.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddTransient<Coche>();
//builder.Services.AddSingleton<Coche>();
//builder.Services.AddSingleton<ICoche,Deportivo>();
//builder.Services.AddSingleton<ICoche, Coche>();
Coche car = new Coche();
car.Marca = "DMG";
car.Modelo = "Delorean";
car.Imagen = "delorean.jpg";
car.Velocidad = 0;
car.VelocidadMaxima = 260;
//PARA PODER ENVIAR EL OBJETO SE UTILIZA LAMBDA
builder.Services.AddSingleton<ICoche, Coche>( x => car);


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
