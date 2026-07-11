using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SliceSync.API.Middlewares;
using SliceSync.Core.IdentityEntities;
using SliceSync.Core.ServiceContracts;
using SliceSync.Infrastructure.Data;
using SliceSync.Service.Services;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.


//Configure DbContext class with DB ConnectionString.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//Configured Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, AppDbContext, Guid>>();

// Re-apply JWT defaults after AddIdentity so API endpoints authenticate/challenge with bearer tokens, not cookies.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});


//convert enum to string
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // ✅
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IPizzaService, PizzaService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderStatusService, OrderStatusService>();
builder.Services.AddTransient<IOrderService, OrderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseGlobalExceptionHandlingMiddleware();
    //app.UseDeveloperExceptionPage();
}
else
{
}


//app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();

app.UseRouting();
app.UseStaticFiles();
app.UseCors("ReactClient");

//auth for login
app.UseAuthentication();
app.UseAuthorization(); 

//Controller
app.MapControllers();

app.Run();
