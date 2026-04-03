var builder = WebApplication.CreateBuilder(args);

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

//Controller
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();


//Controller
app.MapControllers();

app.Run();
