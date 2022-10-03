using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

// Add services to the container.
builder.Services.AddLogging(builder =>
    builder.AddDebug().AddConsole()
    .AddConfiguration(configuration.GetSection("Logging"))
    .SetMinimumLevel(LogLevel.Debug)
);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient("API", client =>
{
  client.BaseAddress = new Uri("https://host.docker.internal:8443/");
  client.DefaultRequestHeaders.Accept.Clear();
  client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
  client.Timeout = TimeSpan.FromMilliseconds(30000);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
  //Proxy = new WebProxy(),
  ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
});

var app = builder.Build();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
