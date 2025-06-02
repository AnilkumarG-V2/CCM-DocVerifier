using V2.DocVerifier.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDocVerifier, DocVerifierService>();
builder.Services.AddTransient<IDocValidator, DocValidatorServcie>();

builder.Services.AddHttpClient<IDocVerifier, DocVerifierService>(client => {
    client.BaseAddress = new Uri(builder.Configuration["DocVerifierURL"]);

});

builder.Services.AddHttpClient<IDocValidator, DocValidatorServcie>(client => {
    client.BaseAddress = new Uri(builder.Configuration["DocVerifierURL"]);

});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
