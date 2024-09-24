using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Supabase;
using WebHookManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Client>(_ => new Client(
    builder.Configuration.GetValue<string>("Supabase:Url")!,
    builder.Configuration.GetValue<string>("Supabase:Key")!,
    new SupabaseOptions { AutoRefreshToken = true, AutoConnectRealtime = true }
    ));
builder.Services.AddScoped<StorageRepository>();

builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection("Authentication"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/{id}", async (
        [FromRoute]string id, 
        [FromQuery]string token,
        HttpContext context,
        [FromServices]StorageRepository repo,
        [FromServices]IConfiguration configuration) =>
    {
        var configToken = configuration.GetValue<string>("Authentication:Token")!;
        if (!configToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
        {
            return Results.Unauthorized();
        }

        using var reader = new StreamReader(context.Request.Body);
        var content = await reader.ReadToEndAsync();
        await repo.InsertAsync(id, content);
        
        return Results.NoContent();
    })
    .WithName("webhook")
    .WithOpenApi();

app.Run();