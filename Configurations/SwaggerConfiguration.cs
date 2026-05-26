using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TravelMasterApi.Configurations;
using TravelMasterApi.SecurityManagers;

namespace TravelMasterApi.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection _service, string _title, string _version)
        {
            _service.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Nhập token vào đây:",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });


                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                opt.OperationFilter<SecurityRequirementsOperationFilter>();

            });
            _service.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader()
                                                                //new HeaderApiVersionReader("x-api-version")
                                                                //new MediaTypeApiVersionReader("x-api-version")
                                                                );
            });
            // Add ApiExplorer to discover versions
            _service.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            _service.ConfigureOptions<ConfigureSwaggerOptions>();


            return _service;
        }

        public static void ConfigurationSwaggerUI(this WebApplication _app)
        {
            var apiVersionDescriptionProvider = _app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            //if (_app.Environment.IsDevelopment())
            //{
            _app.UseSwagger();
            _app.UseSwaggerUI(options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",

                        description.GroupName.ToUpperInvariant());
                }
            });
            //}
        }
        public static IApplicationBuilder UseCustomPrefix(this IApplicationBuilder _app, string prefix)
        {
            _app.UsePathBase($"/{prefix}");
            _app.Use((context, next) =>
            {
                context.Request.PathBase = $"/{prefix}";
                return next();
            });
            return _app;
        }
    }
}
