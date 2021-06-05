using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;

namespace SRTPluginUIJSON
{
    public class JSONServerStartup
    {
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement),
        };

        public JSONServerStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CORSPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("CORSPolicy");

            // TODO: We need a better solution so we don't have to reference a project that is platform-specific. Maybe ProcessMemory needs an Interfaces/Types project for stuff like Int24, UInt24, Int24Converter, UInt24Converter and a few things like extension methods...
            //jsonSerializerOptions.Converters.Add(new Int24Converter());
            //jsonSerializerOptions.Converters.Add(new UInt24Converter());

            app.Run(async context =>
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                if (SRTPluginUIJSON.serializer != null)
                    await (Task)SRTPluginUIJSON.serializer.Invoke(null, new object[] { context.Response.Body, SRTPluginUIJSON.gameMemory, jsonSerializerOptions, CancellationToken.None });
                else
                    context.Response.StatusCode = 404;
            });
        }
    }
}
