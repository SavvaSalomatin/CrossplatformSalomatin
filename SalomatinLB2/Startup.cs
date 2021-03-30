using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SalomatinLB2.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace SalomatinLB2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string cons = "Server=(localdb)\\mssqllocaldb;Database=SomeTastydb;Trusted_Connection=True;";
            services.AddDbContext<MyContext>(options => options.UseSqlServer(cons));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // укзывает, будет ли валидироваться издатель при валидации токена 
        ValidateIssuer = true,
        // строка, представляющая издателя 
        ValidIssuer = AuthOptions.ISSUER,

        // будет ли валидироваться потребитель токена 
        ValidateAudience = true,
        // установка потребителя токена 
        ValidAudience = AuthOptions.AUDIENCE,
        // будет ли валидироваться время существования 
        ValidateLifetime = true,

        // установка ключа безопасности 
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        // валидация ключа безопасности 
        ValidateIssuerSigningKey = true,
    };
});
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
                app.UseDeveloperExceptionPage();
            

            app.UseHttpsRedirection();

            app.UseRouting();



            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
