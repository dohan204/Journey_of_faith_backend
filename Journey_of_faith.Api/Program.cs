using Journey_of_faith.Api.middlewares;
using Journey_of_faith.Application;
using Journey_of_faith.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "allowFrontend", policy =>
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});
// Add services to the container.

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRegisterService(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseCors("allowFrontend");
app.UseExceptionHandler();
app.UseHttpsRedirection();
var currentDirectoryFile = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
if(!Directory.Exists(currentDirectoryFile))
{
    Directory.CreateDirectory(currentDirectoryFile);
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(currentDirectoryFile),
    RequestPath = "/uploads"
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
