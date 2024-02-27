using GameAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<GameManager>();

// Add CORS policy to allow Localhost
builder.Services.AddCors(options =>
{
	options.AddPolicy("MyCorsPolicy",
			builder =>
			{
				builder
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials()
					.SetIsOriginAllowed((host) => true);
			});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
