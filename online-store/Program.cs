global using online_store.Models;
global using Microsoft.EntityFrameworkCore;
global using online_store.DTOs;
global using online_store.Helper;
using online_store.Repositories.Auth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using online_store.Repositories.category;
using online_store.Repositories.PRODUCTS;
using online_store.Authentication_Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//Add services to the DbContext
var connetionString = builder.Configuration.GetSection("ConnectionStrings:MyDatabase").Value;
builder.Services.AddDbContext<OnlineStoreContext>(options =>
    options.UseSqlServer(connetionString)
);

//========================================================================================
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<TokenServices>();
builder.Services.AddScoped<HashServices>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//========================================================================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });
//========================================================================================
var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();
