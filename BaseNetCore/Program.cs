using Microsoft.Extensions.FileProviders;
using TravelMasterApi.Configurations;
using TravelMasterApi.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Thêm cấu hình Global
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSetting = appSettingsSection.Get<AppSettings>();
// 
var _tokenSettingSection = builder.Configuration.GetSection("Token");
builder.Services.Configure<JwtTokenSettings>(_tokenSettingSection);
var _tokenSetting = _tokenSettingSection.Get<JwtTokenSettings>();
//
GlobalSetting.Include(appSetting, _tokenSetting);

//// Cấu hình Mysql
builder.Services.ConfigureMySql(appSetting.ConnectionStrings);

// Configuration jwt authentication
builder.Services.AddJwtAuthentication();

// Configuration Swagger
builder.Services.AddSwaggerConfiguration("Travel Master API", "v1");

builder.Services.AddControllers()
      .AddNewtonsoftJson(options =>
      {
          options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
          options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
      });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigurationSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Resources")),
    RequestPath = new PathString("/Resources")
});
app.UseAuthorization();
app.MapControllers();
app.Run();
