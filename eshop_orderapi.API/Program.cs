using FluentMigrator.Runner;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using eshop_orderapi.API.Filters;
using eshop_orderapi.Business.Helpers;
using eshop_orderapi.Domain;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Background;
using eshop_orderapi.Interfaces.Repositories;
using eshop_orderapi.Interfaces.Services;
using eshop_orderapi.Migrations.DbMigrations;
using eshop_orderapi.Repositories;
using eshop_orderapi.Services;
using eshop_orderapi.UoW;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region  Configure Services 

builder.Services.AddMvc();
builder.Services.AddDbContext<eshop_orderapiContext>(options => options.UseLazyLoadingProxies(false).UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<eshop_orderapiContext>().AddDefaultTokenProviders();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

RegisterRequestLocalizationOptions(builder.Services);
RegisterNewtonsoftJson(builder.Services);
RegisterJwt(builder.Services);
RegisterDI(builder.Services);
RegisterHangfire(builder);
RegisterSwagger(builder.Services);
RegisterCors(builder);
RegisterFluentMigration(builder);

#endregion

var app = builder.Build();

#region Configure

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(url: "v1/swagger.json", name: "API V1");
});
app.UseHttpsRedirection();

//If uploads folder not exist then create
if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "uploads")))
{
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "uploads"));
}

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = (context =>
    {
        context.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
    }),
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads",
    ServeUnknownFileTypes = true
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
    RequestPath = "/uploads"
});

app.UseRouting();
//Enable CORS
app.UseCors("_myAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    AppPath = null,
    DashboardTitle = "Hangfire Dashboard",
    Authorization = new[] { new HangfireAuthorizationFilter() }
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHangfireDashboard();
});
app.Migrate();
AppSettings.Initialize(builder.Configuration);
MailSettings.Initialize(builder.Configuration);
UnitOfWorkHelper.Initialize(builder.Configuration);
MemoryCacheHelper.Initialize();
Jwt.Initialize(builder.Configuration);

//app.Lifetime.ApplicationStarted.Register(OnStarted); //If any database call needed on start up api

app.Run();

//If any database call needed on start up api
//async void OnStarted()
//{
//    using (var unitOfWork = new UnitOfWorkHelper().GetUnitOfWork())
//    {

//    }
//}

#endregion

#region  Configure Services Functions

static void RegisterRequestLocalizationOptions(IServiceCollection services)
{
    services.AddLocalization(opt => { opt.ResourcesPath = "Resource"; });
    services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
    services.Configure<RequestLocalizationOptions>(
                                opt =>
                                {
                                    var supportedCulters = new List<CultureInfo> {
                    new CultureInfo("en"),
                    new CultureInfo("fr"),
                                };
                                    opt.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                                    opt.SupportedCultures = supportedCulters;
                                    opt.SupportedUICultures = supportedCulters;
                                });
}

static void RegisterNewtonsoftJson(IServiceCollection services)
{
    services.AddControllers().AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
    });
}

static void RegisterJwt(IServiceCollection services)
{
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = new Jwt().Issuer,
            ValidAudience = new Jwt().Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(new Jwt().Key))
        };
    });
}

static void RegisterDI(IServiceCollection services)
{
    services.AddTransient<IUnitOfWork, UnitOfWork>();
    RegisterHelpers(services);
    RegisterServices(services);
    RegisterRepositories(services);
    RegisterBackgroundServices(services);
}
static void RegisterHelpers(IServiceCollection services)
{
    services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
    //services.AddSingleton<IFileHelper, FileHelper>();
}

static void RegisterServices(IServiceCollection services)
{
    services.AddScoped<IOrderService, OrderService>();

services.AddScoped<IOrderItemService, OrderItemService>();
}

static void RegisterRepositories(IServiceCollection services)
{
    services.AddScoped<IOrderRepository, OrderRepository>();

services.AddScoped<IOrderItemRepository, OrderItemRepository>();
}

static void RegisterBackgroundServices(IServiceCollection services)
{
    services.AddScoped<IBackgroundService, eshop_orderapi.Services.BackgroundService>();
}

static void RegisterHangfire(WebApplicationBuilder builder)
{
    builder.Services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true
                    }).WithJobExpirationTimeout(TimeSpan.FromDays(7)));
    builder.Services.AddHangfireServer();
}

static void RegisterSwagger(IServiceCollection services)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API", Version = "V1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } });
    });
}

static void RegisterCors(WebApplicationBuilder builder)
{
    var webUrl = builder.Configuration.GetSection("Urls:FrontEnd").Value;
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("_myAllowSpecificOrigins",
            builder => builder
            .WithOrigins(webUrl)
            //.SetIsOriginAllowed(origin => true) // allow any origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            );
    });
}

static void RegisterFluentMigration(WebApplicationBuilder builder)
{
    builder.Services.AddLogging(c => c.AddFluentMigratorConsole())
                        .AddFluentMigratorCore()
                        .ConfigureRunner(c => c
                            .AddSqlServer2012()
                            .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
                            .ScanIn(Assembly.GetAssembly(typeof(MigrationExtension))));
}

#endregion