using Microsoft.EntityFrameworkCore;
using FastFoodDeliveryApi.Configurations;
using Microsoft.OpenApi.Models;
using FastFoodDeliveryApi.Data;
using FastFoodDeliveryApi.Data.Seeder;
using FastFoodDeliveryApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Read the JWT key from the configuration
var jwtKey = builder.Configuration["Jwt:Key"];
if (jwtKey == null) throw new InvalidOperationException("JWT key is missing in configuration.");

// Add JWT Authentication service (from your configuration class)
builder.Services.AddJwtAuthentication(jwtKey);

// Add DbContext (you need to pass the connection string or configure it appropriately)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // Change this line to match your database setup
builder.Services.AddScoped<DatabaseSeeder>();

// Add other services
builder.Services.AddScoped<TokenService>();
builder.Services.AddControllers();

// Bind the SmtpSettings section from appsettings.json
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Register EmailService
builder.Services.AddTransient<EmailService>();

// Add Swagger service
builder.Services.AddSwaggerGen(c =>
{
    // Define the BearerAuth security scheme for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    // Apply the BearerAuth security scheme globally
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedRolesAsync();
}

// Use Swagger only in the Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization(); // Enable authorization middleware

app.MapControllers();

app.Run();