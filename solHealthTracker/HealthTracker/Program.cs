using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HealthTracker.Models;
using HealthTracker.Models.DBModels;
using HealthTracker.Repositories.Classes;
using HealthTracker.Repositories.Interfaces;
using HealthTracker.Services.Classes;
using HealthTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Sockets;
using System.Text;

namespace HealthTracker
{
    public class Program
    { 

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });
            });

            var keyVaultName = "HealthSync";
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            var jwt_secret = await client.GetSecretAsync("JWTToken");
            var secret = jwt_secret.Value.Value;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                    };
                });

            #region CORS
            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("MyCors", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            #endregion

            #region contexts
            const string DBsecretName = "HealthSyncdbConnectionString";
            var connection_secret = await client.GetSecretAsync(DBsecretName);
            var connectionString = connection_secret.Value.Value;

            builder.Services.AddDbContext<HealthTrackerContext>(
               options => options.UseSqlServer(connectionString)
               );
            #endregion

            #region repositories
            builder.Services.AddScoped<IRepository<int, HealthLog>, HealthLogRepository>();
            builder.Services.AddScoped<IRepository<int, IdealData>, IdealDataRepository>();
            builder.Services.AddScoped<IRepository<int, Metric>, MetricRepository>();
            builder.Services.AddScoped<IRepository<int, MonitorPreference>, MonitorPreferenceRepository>();
            builder.Services.AddScoped<IRepository<int, Suggestion>, SuggestionRepository>();
            builder.Services.AddScoped<IRepository<int, Target>, TargetRepository>();
            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int, UserDetail>, UserDetailRepository>();
            builder.Services.AddScoped<IRepository<int, UserPreference>, UserPreferenceRepository>();
            builder.Services.AddScoped<IRepository<int, OAuthAccessTokenModel>, OAuthAccessTokenRepository>();
            builder.Services.AddScoped<IRepository<int, CoachCertificate>, CertificateRepository>();
            #endregion

            #region services 
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IMetricService, MetricService>();
            builder.Services.AddScoped<IHealthLogService, HealthLogService>();
            builder.Services.AddScoped<ITargetService, TargetService>();
            builder.Services.AddScoped<IGraphService, GraphService>();
            builder.Services.AddScoped<IProblemService, ProblemService>();
            builder.Services.AddScoped<IOAuthTokenService, OAuthTokenService>();
            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("MyCors");
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
