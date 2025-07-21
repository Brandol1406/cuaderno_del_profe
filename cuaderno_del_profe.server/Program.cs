using Azure.Core;
using cuaderno_del_profe.server.Entities;
using cuaderno_del_profe.server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cuaderno del profe API", Version = "v1" });

    // Configuración del esquema de seguridad
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT en este formato: Bearer {tu_token_aqui}"
    });

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
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<Cuaderno_del_ProfeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cuaderno_del_Profe"));
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMvcCore().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = (errorContext) =>
    {
        Dictionary<string, string> Errors = new Dictionary<string, string>();

        foreach (var modelStateEntry in errorContext.ModelState)
        {
            foreach (var error in modelStateEntry.Value.Errors)
            {
                Errors.Add(modelStateEntry.Key, error.ErrorMessage);
            }
        }

        return new BadRequestObjectResult(new OperationResult(Errors));
    };
});

builder.Services.AddScoped<IUserAccessor>(provider => {
    IHttpContextAccessor context = provider.GetService<IHttpContextAccessor>();
    int uid = Convert.ToInt32(context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "NameIdentifier")?.Value);
    string? username = Convert.ToString(context.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Name")?.Value);

    return new UserAccessor
    {
        idUsuario = uid,
        username = username
    };
});

//Hace que se pueda requerir el token en Swagger
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey)) throw new Exception("No se encontro la propiedad \"Jwt.Key\" el el archivo settings.json");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        //Los endpoints vienen collapsados por defecto (eso mejora la navegacion en la UI)
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();  

app.MapControllers();

app.UseDefaultFiles(); // Esto hace que / muestre index.html autom�ticamente
app.UseStaticFiles();

app.Run();

