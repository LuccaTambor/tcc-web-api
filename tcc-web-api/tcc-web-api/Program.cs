using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using tcc_web_api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TCCDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringDatabase")));

builder.Services.AddControllers();
//builder.Services.AddControllers().AddJsonOptions(options => {
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
