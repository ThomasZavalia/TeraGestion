using Controllers;
using Core.Interfaces;
using Core.Interfaces.Repositorios;
using Core.Mapping;
using Infraestructure;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Services;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TeraDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



// Repositorios
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuarioRepository>();
builder.Services.AddScoped<IPagosRepository, PagoRepository>();
builder.Services.AddScoped<ISesionRepository, SesionRepository>();
builder.Services.AddScoped<IObraSocialRepository, ObraSocialRepository>();


// Servicios
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<ITurnoService, TurnoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<ISesionService, SesionService>();
builder.Services.AddScoped<IObraSocialService, ObraSocialService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173", "http://localhost:5033")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});



builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(Core.Mapping.TurnoProfile).Assembly);
builder.Services.AddAutoMapper(typeof(PagoProfile).Assembly);
builder.Services.AddAutoMapper(typeof(SesionProfile).Assembly);




builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverterWithoutZone());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(a => a.Run(async context =>
{
    var feature = context.Features.Get<IExceptionHandlerPathFeature>();
    var ex = feature?.Error;
    var result = JsonSerializer.Serialize(new { error = ex?.Message });
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = ex is ArgumentException ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsync(result);
}));

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
