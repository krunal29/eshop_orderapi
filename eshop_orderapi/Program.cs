using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using eshop_orderapi.Business.Helpers;
using eshop_orderapi.Domain;
using eshop_orderapi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

#region  Configure Services

builder.Services.AddLocalization(opt => { opt.ResourcesPath = "Resource"; });
builder.Services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
builder.Services.Configure<RequestLocalizationOptions>(
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
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession();
builder.Services.AddDbContext<eshop_orderapiContext>(options => options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<eshop_orderapiContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/LogIn";
    options.ReturnUrlParameter = "redirectUrl";
    options.Cookie.MaxAge = TimeSpan.FromDays(1);
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
});
builder.Services.AddSingleton<IFileHelper, FileHelper>();
#endregion


// Build the application
var app = builder.Build();

#region Configure 
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();

app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Login}/{action=Index}/{id?}");
});

AppSettings.Initialize(builder.Configuration);

app.Use(async (context, next) =>
{
    using (var customStream = new MemoryStream())
    {
        // Create a backup of the original response stream
        var backup = context.Response.Body;

        // Assign readable/writeable stream
        context.Response.Body = customStream;

        await next();

        // Restore the response stream
        context.Response.Body = backup;

        // Move to start and read response content
        customStream.Seek(0, SeekOrigin.Begin);
        var content = new StreamReader(customStream).ReadToEnd();

        // Write custom content to response
        await context.Response.WriteAsync(content);
    }
});

MemoryCacheHelper.Initialize();
app.Run();
#endregion



