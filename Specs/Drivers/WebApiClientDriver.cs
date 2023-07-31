using Data.Context;
using Data.DataPort;
using Data.Models.DTO;
using Data.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Specs.Utils;


namespace Specs.Drivers
{
    internal class WebApiClientDriver : IClientDriver
    {
        private readonly HttpClient httpClient;
        private IEnumerable<ToDoItemDto> requestedToDoItems = new List<ToDoItemDto>();
        private IWebHost restServer = null!;

        public WebApiClientDriver()
        {
            httpClient = MakeHttpClient();
        }

        /// <inheritdoc />
        public void SetUp(IToDoItemDataPort dataPort)
        {
            var restServerBuilder = WebHost.CreateDefaultBuilder();
            restServer = restServerBuilder
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddJsonFile("appsettings.Development.json");
                })
                .UseUrls(TestConfiguration.RestUrl)
                .ConfigureServices(services =>
                {
                    services.AddDbContext<ToDoItemContext>(opt
                    => opt.UseInMemoryDatabase(TestConfiguration.InMemoryDbName)); 
                    services.AddScoped(_ => dataPort);
                    services.AddScoped<IToDoItemDbService, ToDoItemDbDbService>();
                })
                .UseStartup<RestApi.Startup>()
                .Build();
            restServer.RunAsync();
        }

        /// <inheritdoc/>
        public void RequestToDoItemList()
        {
            var resourceUri = $"{TestConfiguration.RestUrl}/api/todoitems";
            var sendResult = httpClient.GetAsync(resourceUri).Result;
            var jsonResponse = sendResult.Content.ReadAsStringAsync().Result;
            requestedToDoItems = JsonConvert.DeserializeObject<IEnumerable<ToDoItemDto>>(jsonResponse) ??
                                 new List<ToDoItemDto>();
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoItemDto> GetRequestedToDoItemList()
        {
            return requestedToDoItems;
        }

        /// <inheritdoc/>
        public void TearDown()
        {
            restServer.StopAsync();
        }

        /// <summary>
        /// Make HTTP Client
        /// </summary>
        /// <returns> HttpClient instance </returns>
        private static HttpClient MakeHttpClient()
        {
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                    (_, _, _, _) => true,
                AllowAutoRedirect = true
            };

            var client = new HttpClient(handler);

            return client;
        }
    }
}
