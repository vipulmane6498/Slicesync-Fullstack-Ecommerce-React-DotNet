using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SliceSync.Core.IdentityEntities;
using SliceSync.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.


//Configure DbContext class with DB ConnectionString.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//Configured Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, AppDbContext, Guid>>();


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

app.UseRouting();

//auth for login
app.UseAuthentication();
app.UseAuthorization();

//Controller
app.MapControllers();

app.Run();
