using FiscalizAI.Infra.Data;
using FiscalizAI.Infra.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var postgresConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<FiscalizAIContext>(options =>
    options.UseNpgsql(postgresConnectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    //configurações de senha
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 1;

    //evitar brute force
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    //integridade de dados
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    //configurações de login - cofigurar o rabbitmq para enviar email de confirmação
    options.SignIn.RequireConfirmedEmail = false; //lembrar de alterar para true em produção
    options.SignIn.RequireConfirmedAccount = false; //lembrar de alterar para true em produção
})
.AddEntityFrameworkStores<FiscalizAIContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
