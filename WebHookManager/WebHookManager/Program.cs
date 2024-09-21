using System.Net;
using System.Net.Mail;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        [FromBody]Arkham arkham, 
        [FromServices]StorageRepository repo) =>
    {
        await repo.InsertAsync(id, JsonSerializer.Serialize(arkham));
        
        // Header -> Arkham-Webhook-Token : we5kjWqphpZs9I
        // var from = "noreply@test.com";
        // var to = "skorcius@skorcius.xyz";
        // var msg = new MailMessage()
        // {
        //     From = new MailAddress(from),
        //     To = { to },
        //     Subject = "Arkham",
        //     Body = JsonSerializer.Serialize(arhkam),
        //     
        // };
        //
        // var client = new SmtpClient("smtp-mail.outlook.com", 587);
        // client.Credentials = new NetworkCredential("", "");
        // client.EnableSsl = true;
        //
        // client.Send(msg);

        return Results.NoContent();
    })
    .WithName("webhook")
    .WithOpenApi();

app.Run();