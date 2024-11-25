using BLL;
using BLL.Interface;
using DLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUser, UserBLL>();
builder.Services.AddScoped<IProduct, ProductBLL>();
builder.Services.AddSingleton<IAuthService,AuthService>();
builder.Services.AddScoped<ICategory, CategoryBLL>();
builder.Services.AddScoped<ISubCategory, SubCategoryBLL>();
builder.Services.AddScoped<ISupplier, SupplierBLL>();
builder.Services.AddScoped<ICart, CartBLL>();
builder.Services.AddScoped<IUserInfo,UserInfoBLL>();
builder.Services.AddScoped<IProvince, ProvinceBLL>();
builder.Services.AddScoped<ICommune, CommuneBLL>();
builder.Services.AddScoped<IDistrict, DistrictBLL>();
builder.Services.AddScoped<ICell, CellsBLL>();
builder.Services.AddScoped<IAddress, AddressBLL>();
builder.Services.AddScoped<IOrder, OrderBLL>();
builder.Services.AddScoped<IWareHouse, WareHouseBLL>();
builder.Services.AddScoped<IShelve, ShelveBLL>();
builder.Services.AddScoped<ICustomerType, CustomerTypeBLL>();
builder.Services.AddScoped<IDepartment, DepartmentBLL>();
builder.Services.AddScoped<IEmployee, EmployeeBLL>();
builder.Services.AddScoped<IReport, ReportBLL>();
builder.Services.AddScoped<IWarehouseReceipt, WarehouseReceiptBLL>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DB
builder.Services.AddDbContext<DbVINA>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbVINA"))
);

// Thêm dịch vụ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200") // Your Angular app URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Cấu hình JWT với Issuer và Audience
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

// Cấu hình Swagger và thêm các tùy chọn cho JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Documentation", Version = "v1" });

    // Cấu hình xác thực JWT cho Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập 'Bearer' [space] và sau đó là token của bạn trong ô bên dưới.\n\nVí dụ: 'Bearer abcdef12345'"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseCors(builder => builder
    .WithOrigins("http://localhost:4200") // Replace with your Angular app's origin
    .AllowAnyMethod()
    .AllowAnyHeader()
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
