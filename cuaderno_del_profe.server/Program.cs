using cuaderno_del_profe.server.Entities;
using cuaderno_del_profe.server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Cuaderno_del_ProfeContext>(options => {
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

app.UseAuthorization();

app.MapControllers();

app.UseDefaultFiles(); // Esto hace que / muestre index.html autom�ticamente
app.UseStaticFiles();

app.Run();

