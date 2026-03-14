using E_Commerce_Proj.Data;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Dependancy Injection
    builder.Services.AddDbContextSettings().AddSwaggerSettings();
#endregion

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}
app.MapGet("/", () =>  Results.Redirect("/swagger/"));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
