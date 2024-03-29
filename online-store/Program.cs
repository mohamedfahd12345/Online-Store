global using online_store.Models;
global using Microsoft.EntityFrameworkCore;
global using online_store.DTOs;
global using online_store.Helper;
global using Microsoft.AspNetCore.Authorization;
global using online_store.Services.CachingServices;

using online_store.Repositories.Auth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using online_store.Repositories.category;
using online_store.Repositories.PRODUCTS;
using online_store.Repositories.CART;

using online_store.Authentication_Services;
using online_store.MiddleWares;

using online_store.Repositories.ORDER;
using Serilog;
using Supabase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();

//Serilog
/*builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{

    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) //read configuration settings from built-in IConfiguration
    .ReadFrom.Services(services); //read out current app's services and make them available to serilog
});*/

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

var connetionString = builder.Configuration.GetSection("ConnectionStrings:MyDatabase").Value;
builder.Services.AddDbContext<OnlineStoreContext>(options =>
    options.UseSqlServer(connetionString)
);
string redisConnectionStrings = builder.Configuration.GetSection("Redis").Value;
builder.Services.AddStackExchangeRedisCache(redisOptions => {
    
    redisOptions.Configuration = redisConnectionStrings;
});

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository,CartRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();

builder.Services.AddScoped<TokenServices>();
builder.Services.AddScoped<HashServices>();
builder.Services.AddSingleton<ICacheService,CacheService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("JwtSettings:SecretKey").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });



builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        // builder.WithOrigins("http://example.com");    
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
var SupabaseUrl = builder.Configuration["SUPABASE_URL"];
var SupabaseKey = builder.Configuration["SUPABASE_KEY"];

builder.Services.AddScoped<Supabase.Client>(_ =>
    new Supabase.Client(
        SupabaseUrl,
        SupabaseKey ,
        new SupabaseOptions
        {
            AutoConnectRealtime = true ,
            

        }));

//================================= -> MIDDLEWARES <- =========================================
var app = builder.Build();
//app.UseSerilogRequestLogging();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseHealthChecks("/health");

app.MapControllers();

//app.Run();
app.Run("http://*:8080");
