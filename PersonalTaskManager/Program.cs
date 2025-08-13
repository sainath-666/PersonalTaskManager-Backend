using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PersonalTaskManager.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Personal Task Manager API",
        Version = "v1",
        Description = "API for managing personal tasks and employees."
    });
});

// Register repositories
//builder.Services.AddScoped<TaskRepository>(); // your existing one
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

// Add authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
