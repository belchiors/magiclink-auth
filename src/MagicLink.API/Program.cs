using MagicLink.API.Interfaces;
using MagicLink.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Set up token service
builder.Services.AddScoped<ITokenService, TokenService>(options => {
    var secretKey = builder.Environment.IsProduction()
        ? Environment.GetEnvironmentVariable("SECRET_KEY")
        : builder.Configuration["Local:SECURITY_KEY"];
    
    return new TokenService(secretKey, options.GetRequiredService<ILogger<TokenService>>());
});

builder.Services.AddSingleton<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
