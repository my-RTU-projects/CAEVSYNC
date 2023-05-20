using System.Text;
using CAEVSYNC.Api;
using CAEVSYNC.Auth.Services;
using CAEVSYNC.ConnectedAccounts.Auth.FlowContextes;
using CAEVSYNC.ConnectedAccounts.Clients;
using CAEVSYNC.Data;
using CAEVSYNC.Services;
using CAEVSYNC.Services.EventTransformation;
using CAEVSYNC.SyncWorker;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors();

builder.Services.AddScoped<CalendarSyncService>();
builder.Services.AddScoped<SyncRulesService>();
builder.Services.AddScoped<CalendarService>();
builder.Services.AddScoped<ConnectedAccountService>();

builder.Services.AddScoped<MicrosoftAuthFlowContext>();
builder.Services.AddScoped<GoogleAuthFlowContext>();

builder.Services.AddScoped<MicrosoftCalendarClient>();
builder.Services.AddScoped<GoogleCalendarClient>();
builder.Services.AddScoped<CalendarClientFactory>();
builder.Services.AddScoped<EventTransformationServiceFactory>();

builder.Services.AddTransient<UserManager<IdentityUser>>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<CalendarSyncWorker>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<CalendarSyncWorker>());

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CaevsyncDbContext>(options => options.UseSqlServer(connection));

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"/secrets"));

// Auth
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Environment.GetEnvironmentVariable("AUTH_VALID_ISSUER"),
            ValidAudience = Environment.GetEnvironmentVariable("AUTH_VALID_AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("AUTH_ISSUER_SIGNING_KEY"))
            ),
        };
    });

builder.Services
    .AddIdentityCore<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<CaevsyncDbContext>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IServiceProvider, ServiceProvider>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CaevsyncDbContext>();
    dbContext.Database.Migrate();
}

app.UseCors(builder => 
{
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();