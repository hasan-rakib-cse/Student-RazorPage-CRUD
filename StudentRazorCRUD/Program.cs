using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using StudentRazorCRUD.Data;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext") ??
    throw new InvalidOperationException("Connection string 'AppDbContext' not found")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

var options = new RewriteOptions()
    .AddRedirect("^$", "Students/Index");

app.UseRewriter(options);

app.MapRazorPages();

app.Run();
