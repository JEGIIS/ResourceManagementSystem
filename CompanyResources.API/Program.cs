using CompanyResources.API.Data;
using CompanyResources.API.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CORS (TO NAPRAWIA TWÓJ B£¥D) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // Pozwala na dowolny adres (UI)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Wa¿ne dla SignalR i Auth!
    });
});

// --- 2. Baza Danych ---
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("CompanyDb"));

// --- 3. SignalR ---
builder.Services.AddSignalR();

// --- 4. Autentykacja JWT ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // TEN KLUCZ PONI¯EJ MA TERAZ PONAD 64 ZNAKI:
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ToJestBardzoDlugiIBezpiecznyKluczKtoryMaWystarczajacoDuzoZnakowDlaSHA512!!!")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();

// --- 5. Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Wpisz token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } }
    });
});

var app = builder.Build();

// --- 6. Pipeline (Kolejnoœæ jest krytyczna!) ---

// A. Najpierw Swagger (w dev)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// B. Potem CORS (Musi byæ PRZED Auth!)
app.UseCors("AllowAll");

// C. Potem Autentykacja i Autoryzacja
app.UseAuthentication();
app.UseAuthorization();

// D. Na koñcu mapowanie endpointów
app.MapControllers();
app.MapHub<ResourceHub>("/resourceHub");

// Seedowanie danych (Admin)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!db.Users.Any())
    {
        db.Users.Add(new User { Email = "admin@firma.pl", PasswordHash = "admin123", Role = "Admin" });
        db.SaveChanges();
    }
}

app.Run();