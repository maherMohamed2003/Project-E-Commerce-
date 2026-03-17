using System.Text;
using E_Commerce_Proj.Authentication;
using E_Commerce_Proj.Data;
using E_Commerce_Proj.Validation.CategoryValid;
using E_Commerce_Proj.Validation.Customer;
using E_Commerce_Proj.Validation.Feedback;
using E_Commerce_Proj.Validation.ProductValid;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


#region Dependancy Injection
builder.Services.AddControllers().AddFluentValidation(o =>
    {
        o.RegisterValidatorsFromAssemblyContaining<RegisterValidation>();
        o.RegisterValidatorsFromAssemblyContaining<LoginValidation>();
        o.RegisterValidatorsFromAssemblyContaining<FeedbackValidation>();
        o.RegisterValidatorsFromAssemblyContaining<AddProductValidation>();
        o.RegisterValidatorsFromAssemblyContaining<AddCategoryValidation>();
    });
builder.Services.AddDbContextSettings().AddSwaggerSettings().AddReposetories();
#endregion

var jwt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
builder.Services.AddSingleton(jwt);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
        };
    });
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}
app.MapGet("/", () => Results.Redirect("/swagger/"));
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
