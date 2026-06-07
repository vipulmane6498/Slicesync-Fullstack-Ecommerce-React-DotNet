using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SliceSync.API.Middlewares;
using SliceSync.Core.IdentityEntities;
using SliceSync.Core.ServiceContracts;
using SliceSync.Infrastructure.Data;
using SliceSync.Service.Services;

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

//Configured Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, AppDbContext, Guid>>();


//Controller
builder.Services.AddControllers();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IPizzaService, PizzaService>();
builder.Services.AddTransient<ICartService, CartService>();

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

//auth for login
app.UseAuthentication();
app.UseAuthorization(); 

//Controller
app.MapControllers();

app.Run();
