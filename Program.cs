using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using MyApi.Models;
using FluentValidation;
using MyApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Asp.Versioning.ApiExplorer;
using Asp.Versioning;

namespace MyApi;

class Program
{
    static async Task Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder();
        var services = builder.Services;
        var jwksurl = builder.Configuration["Jwt:JwksUrl"];

        builder.Services.AddProblemDetails();
        builder.Services.AddMvc();

        _ = services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        services
            .AddMvcCore()
            .AddJsonOptions(o =>
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

        builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKeyResolver = (_, _, _, _) => GetIssuerSigningKey(),
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
        services.AddSingleton<IAuthorizationHandler, RequireScopeHandler>();
        services.AddAuthorization(c =>
        {
            c.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                .RequireAuthenticatedUser()
                .AddRequirements(
                    new ScopeRequirement(builder.Configuration["Jwt:Issuer"], "ReadData")
                ).Build());
            c.AddPolicy("BearerWrite", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                .RequireAuthenticatedUser().AddRequirements(
                    new ScopeRequirement(builder.Configuration["Jwt:Issuer"], "Write")
                ).Build());
        });

        services
        .AddSwaggerGen(c =>
        {

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BooksDemo API", Version = "v1" });

            // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            // c.IncludeXmlComments(xmlPath);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });
            // c.AddSecurityRequirement(new OpenApiSecurityRequirement{
            //         {
            //             new OpenApiSecurityScheme
            //             {
            //                 Reference = new OpenApiReference
            //                 {
            //                     Type=ReferenceType.SecurityScheme,
            //                     Id="Bearer"
            //                 }
            //             },
            //             new string[]{}
            //         }
            // });

            c.OperationFilter<AuthorizeCheckOperationFilter>();
            // c.OperationFilter<DefaultResponseOperationFilter>();
            // c.SchemaFilter<BookSchemaFilter>();
        });

        //services.ConfigureOptions<ConfigureSwaggerOptions>();

        _ = services
                .AddValidatorsFromAssemblyContaining<BookValidator>()
                .AddFluentValidationAutoValidation(c => c.DisableDataAnnotationsValidation = true);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BooksDemo API v1"));


        await app.RunAsync();

        IList<JsonWebKey> GetIssuerSigningKey()
        {
            var json = new HttpClient().GetStringAsync(jwksurl).Result;
            var kset = JsonWebKeySet.Create(json);
            return kset.Keys;
        }

    }
}