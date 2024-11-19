using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using ProvaPratica.Repository.Data;
using ProvaPratica.Service.Services;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Authorization;
using ProvaPratica.Service.Interfaces;
using ProvaPratica;
using System;

namespace ProvaPratica
{
    public class Startup
    {
        public IWebHostEnvironment CurrentEnvironment { get; set; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Key").Value ?? throw new Exception("Key no configurada."));

            

            services.AddDbContext<AppDbContext>(options =>
            {
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            });

            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IPostService, PostService>();

            services.AddCors(options =>
            {
                options.AddPolicy("corsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });

            services.AddControllers(options =>
            {
                if (!CurrentEnvironment.IsDevelopment())
                    options.Filters.Add(new AuthorizeFilter());

                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                }

                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                }
            }).AddOData(opt => opt.Select()
                .Expand()
                .SetMaxTop(null)
                .SkipToken()
                .OrderBy()
                .Count()
                .Filter()
                .EnableQueryFeatures(1000)
            ).AddODataNewtonsoftJson();

            services.AddFluentValidationAutoValidation(fv =>
            {
                fv.DisableDataAnnotationsValidation = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProvaPratica API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Ocorreu um erro ao atualizar o banco de dados.");
                }
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProvaPratica API V1");
            });

            if (CurrentEnvironment.IsDevelopment())
                app.UseHttpsRedirection();

            app.UseRequestLocalization();

            app.UseRouting();

            app.UseCors("corsPolicy");
            
            app.UseAuthentication(); //
            app.UseAuthorization(); //

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}