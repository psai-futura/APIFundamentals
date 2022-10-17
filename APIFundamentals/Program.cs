using System.Text;
using APIFundamentals;
using APIFundamentals.Controllers;
using APIFundamentals.DBContexts;
using APIFundamentals.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/APIFundamentalsLog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();

//For logging using Serilog
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

//Adding Mail Service
#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

//Adding DataSource Service
builder.Services.AddSingleton<CitiesDataStore>();

builder.Services.AddDbContext<CityInfoContext>(dbContextOptions =>
    dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
    };
});

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("MustBeFromBerlin", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("city", "Berlin");
        });
    }
);


var app = builder.Build();

// Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
     app.UseSwagger();
     app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseRouting();

//Authentication using Middleware
app.UseAuthentication();

app.UseAuthorization();

//app.MapControllers();

app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    }
);
//Test code to write "Hello World"
// app.Run(async (context) =>
// {
//     await context.Response.WriteAsync("Hello World");
// });

app.Run();