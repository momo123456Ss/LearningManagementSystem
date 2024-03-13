using CloudinaryDotNet;
using LearningManagementSystem.Entity;
using LearningManagementSystem.Models.MailSender;
using LearningManagementSystem.Repository;
using LearningManagementSystem.Repository.InterfaceRepository;
using MailKit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Using Bearer scheme {\"bearer {token}\"}",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<LearningManagementSystemContext>(option
    => option.UseSqlServer(builder.Configuration.GetConnectionString("LearningManagementSystem")));
builder.Services.AddScoped<InterfaceUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<InterfaceUserRepository, UserRepository>();
builder.Services.AddScoped<InterfaceMailRepository, MailRepository>();
builder.Services.AddScoped<InterfaceClassRepository, ClassRepository>();
builder.Services.AddScoped<InterfaceFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<InterfaceUserBelongToFacultyRepository, UserBelongToFacultyRepository>();
builder.Services.AddScoped<InterfaceSubjectRepository, SubjectRepository>();
builder.Services.AddScoped<InterfaceOtherSubjectInformationRepository, OtherSubjectInformationRepository>();
builder.Services.AddScoped<InterfaceSubjectTopicRepository, SubjectTopicRepository>();
builder.Services.AddScoped<InterfaceUserClassSubjectRepository, UserClassSubjectRepository>();
builder.Services.AddScoped<InterfaceLecturesAndResourcesRepository, LecturesAndResourcesRepository>();
builder.Services.AddScoped<InterfaceLessonResourcesRepository, LessonResourcesRepository>();
builder.Services.AddScoped<InterfaceLessonRepository, LessonRepository>();
builder.Services.AddScoped<InterfaceQaARepository, QaARepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,


        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(p => p.AddPolicy("CrossCors", build =>
{
    build.WithOrigins(builder.Configuration["Cross-Origin:Domain"]).AllowAnyMethod().AllowAnyHeader();
}));
//Mail
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<InterfaceMailRepository, MailRepository>();
//Cloudinary
builder.Services.AddSingleton(provider =>
{
    var account = new Account(
        builder.Configuration["Cloudinary:Cloud-name"],
        builder.Configuration["Cloudinary:API-key"],
        builder.Configuration["Cloudinary:API-secret"]
    );
    return new Cloudinary(account);
});
//Mapper
builder.Services.AddAutoMapper(typeof(Program));
//Files
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
});
//Role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministrator", policy => policy.RequireRole("Administrator".ToLower(), "Quản trị".ToLower()));
    options.AddPolicy("RequireTeacher", policy => policy.RequireRole("Giảng viên".ToLower()));
    options.AddPolicy("RequireStudent", policy => policy.RequireRole("Học viên".ToLower()));
    options.AddPolicy("RequireAdministratorAndTeacher", policy =>
    {
        policy.RequireRole("Quản trị".ToLower(),"Giảng viên".ToLower());
    });
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CrossCors");

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads\\Files")),
    RequestPath = "/Uploads"
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
