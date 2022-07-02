using BuberDinner.Application;
using BuberDinner.Infraestructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication()
        .AddInfraestructure(builder.Configuration);
    builder.Services.AddControllers();
}

var app = builder.Build();

{
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}