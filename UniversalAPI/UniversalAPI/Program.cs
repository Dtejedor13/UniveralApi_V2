using Microsoft.OpenApi.Models;
using System.Reflection;
using UniversalAPI.Interfaces;
using UniversalAPI.Logger;
using UniversalAPI.Services;

var builder = WebApplication.CreateBuilder(args);

/****************** add services to the container ********************/
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dependency injection
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILogger, MyLogger>();
builder.Services.AddTransient<IControllerService, ControllerService>();

// swagger config
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My title",
        Description = "I like trains",
        Version = "v1"
    });

    var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
    options.IncludeXmlComments(filePath);
});

// cors setup
const string myCorsPolicy = "myCorsPolicy";
builder.Services.AddCors(policy => policy.AddPolicy(myCorsPolicy, builder => {
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My Services");
});

app.UseCors(myCorsPolicy);
app.MapControllers();

app.Run();
