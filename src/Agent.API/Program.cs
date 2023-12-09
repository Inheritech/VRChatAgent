using Agent.API.HostedServices;
using Agent.API.Options;
using Agent.API.Services;
using Agent.Client;
using Agent.Data;
using Serilog;
using Agent.Client.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<VRChatOptions>(builder.Configuration.GetSection(VRChatOptions.ConfigurationKey));
builder.Services.Configure<VrcEmailOtpOptions>(builder.Configuration.GetSection(VrcEmailOtpOptions.ConfigurationKey));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<VRChatServiceAccessor>();
builder.Services.AddSingleton<EmailClientProvider>();
builder.Services.AddVRChatService<LocalStorage>();

builder.Services.AddMediatR(configuration => {
    configuration.RegisterServicesFromAssemblyContaining<Program>();
});

builder.Services.AddDbContext<AgentDataContext>();
builder.Services.AddHostedService<NotificationProcessingService>();

builder.Host.UseSerilog((context, configuration) => {
    configuration.ReadFrom.Configuration(context.Configuration);
    configuration.WriteTo.Console();
});

if (builder.Environment.IsDevelopment()) {
    builder.Configuration.AddUserSecrets<Program>();
}

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<AgentDataContext>();
    Log.Logger.Information("Applying any pending migrations.");
    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
