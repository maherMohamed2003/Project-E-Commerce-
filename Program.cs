using System.Text;
using E_Commerce_Proj.Authentication;
using E_Commerce_Proj.Data;
using E_Commerce_Proj.Validation.CartValid;
using E_Commerce_Proj.Validation.CategoryValid;
using E_Commerce_Proj.Validation.Customer;
using E_Commerce_Proj.Validation.Feedback;
using E_Commerce_Proj.Validation.ProductValid;
using E_Commerce_Proj.Validation.ReviewValid;
using E_CommerceApi.Middlewares;
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
        o.RegisterValidatorsFromAssemblyContaining<AddReviewValidation>();
        o.RegisterValidatorsFromAssemblyContaining<AddCartItemValidation>();
        o.RegisterValidatorsFromAssemblyContaining<UpdateProductValidation>();
        o.RegisterValidatorsFromAssemblyContaining<UpdateCategoryValidation>();
    });
builder.Services.AddDbContextSettings().AddSwaggerSettings().AddReposetories();

var jwt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
builder.Services.AddSingleton(jwt);
#endregion

builder.Services.AddAuthentication()
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .SetIsOriginAllowed(origin => true)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.UseSwagger();
}
app.MapGet("/", () => Results.Redirect("/swagger/"));
app.UseHttpsRedirection();
#region Middlewares

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<CounterMiddleware>();
app.UseMiddleware<HandleRequestsMiddleware>();

#endregion
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
