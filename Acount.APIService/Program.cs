using Acount.APIService.Common;
using Acount.APIService.Contracts;
using Acount.APIService.DataAccess;
using Microsoft.AspNetCore.HttpLogging;
using Acount.APIService.MIddleware;
using Acount.APIService.Process;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
	.AddNewtonsoftJson(options =>
	{
		options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
	});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(s =>
{
	s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer"
	});
	s.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var logger = new LoggerConfiguration().MinimumLevel.Verbose().MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
.ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddDbContext<PGDBContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddTransient<IUserManagement, ProcessManager>();
builder.Services.AddTransient<IAuth, ProcessManager>();

builder.Services.AddHttpLogging(logging =>
{
	logging.LoggingFields = HttpLoggingFields.RequestHeaders |
							HttpLoggingFields.RequestBody |
							HttpLoggingFields.ResponseHeaders |
							HttpLoggingFields.ResponseBody;
	logging.RequestHeaders.Add("sec-ch-ua");
	logging.ResponseHeaders.Add("MyResponseHeader");
	logging.MediaTypeOptions.AddText("application/javascript");
	logging.RequestBodyLogLimit = 4096;
	logging.ResponseBodyLogLimit = 4096;
});

builder.Services.AddAuthentication(x =>
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTGlobal.Key))
	};
});

//var logger = new LoggerConfiguration().MinimumLevel.Verbose().MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
//.ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
//builder.Logging.AddSerilog(logger);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseMiddleware<LoggingMiddleware>();

GlobalSettings.Configuration = app.Services.GetRequiredService<IConfiguration>();
BusinessDBContext.serviceProvider = app.Services.GetRequiredService<IServiceProvider>();
SystemLogger.loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
GlobalSettings globalSettings = new();

GlobalSettings.Configuration = builder.Configuration;
SystemLogger.loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

//GlobalSettings globalSettings = new();


app.Use(async (context, next) =>
{
	context.Request.EnableBuffering();
	await next();
});

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");


//app.UseAuthentication();
//app.UseAuthorization();
app.UseCors("MyPolicy");

app.MapControllers();

app.Run();
