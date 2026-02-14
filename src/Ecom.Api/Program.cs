using System.Threading.RateLimiting;
using Ecom.Api.Mapping;
using Ecom.Api.Middlewares;
using Ecom.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .WithOrigins("http://www.localhost:4200");
    });
});

// Configure rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";

        var response = new ApiExceptions(
            StatusCodes.Status429TooManyRequests,
            "Too many requests. Please try again later."
        );

        var json = System.Text.Json.JsonSerializer.Serialize(response);

        await context.HttpContext.Response.WriteAsync(json, token);
    };

    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 8;
        opt.Window = TimeSpan.FromSeconds(30);
        opt.QueueLimit = 0;
        opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    });
});

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InfrastructureConfiguration(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();


app.UseCors("CorsPolicy");

app.UseMiddleware<ExceptionsMiddleware>();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers()
   .RequireRateLimiting("fixed");

app.MapControllers();

app.Run();
