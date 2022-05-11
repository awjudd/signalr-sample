using BlazorServer.Data;
using Microsoft.AspNetCore.ResponseCompression;
using BlazorServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.SetIsOriginAllowed(h => true)
    .AllowCredentials()    
    .AllowAnyMethod()
    .AllowAnyHeader()));

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[]
        {
            "application/octet-stream"
        });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub()
    .AllowAnonymous();
app.MapHub<ChatHub>("/chat-hub");
app.MapHub<CounterHub>("/counter-hub");

app.MapFallbackToPage("/_Host");

app.Run();
