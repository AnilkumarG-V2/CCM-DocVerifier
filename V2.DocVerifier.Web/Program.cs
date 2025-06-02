using V2.DocVerifier.Services.UI.Interfaces;
using V2.DocVerifier.Services.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IDocVerifier, DocVerifierUIService>();
builder.Services.AddTransient<IDocValidator, DocValidatorUIService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient<IDocVerifier, DocVerifierUIService>(client => {
    client.BaseAddress = new Uri(builder.Configuration["DocVerifierBaseURL"]);
});

builder.Services.AddHttpClient<IDocValidator, DocValidatorUIService>(client => {
    client.BaseAddress = new Uri(builder.Configuration["DocVerifierBaseURL"]);
});

var app = builder.Build();
app.UseSession();
app.UseExceptionHandler("/Error");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();