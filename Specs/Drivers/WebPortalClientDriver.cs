using Data.DataPort;
using Data.Models.DTO;
using System.Diagnostics;
using Data.Context;
using Data.Services;
using Microsoft.AspNetCore;
using Specs.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using OpenQA.Selenium;
using Specs.PageObjects;
using Specs.Selenium;

namespace Specs.Drivers
{
    public class WebPortalClientDriver : IClientDriver
    {
        private Process webPortalProcess = null!;
        private IWebDriver webDriver = null!;
        private IWebHost restServer = null!;
        private Size configuredSize;
        private const int WebPortalPort = 44475;
        private ReactAppPage reactApp = null!;

        /// <inheritdoc/>
        public void SetUp(IToDoItemDataPort dataPort)
        {
            webPortalProcess = StartWebPortal();
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

            StartWebDriver();
            webDriver.Navigate().GoToUrl($"https://localhost:{WebPortalPort}");
            reactApp = new ReactAppPage(webDriver);
        }

        /// <inheritdoc/>
        public void RequestToDoItemList()
        {
            reactApp.GetFetchDataMenu().Click();
        }

        /// <inheritdoc/>
        public IEnumerable<ToDoItemDto> GetRequestedToDoItemList()
        {
            var rowElements = reactApp.GetToDoItemTableRows();

            var actualResult = rowElements
                .Select(rowElement => new ToDoItemDto
                {
                    Id = long.Parse(rowElement.FindElement(By.XPath(".//td[1]")).Text),
                    Name = rowElement.FindElement(By.XPath(".//td[2]")).Text,
                    IsComplete = bool.Parse(rowElement.FindElement(By.XPath(".//td[3]")).Text)
                })
                .ToList();

            return actualResult;
        }

        /// <inheritdoc/>
        public void TearDown()
        {
            webDriver.Dispose();
            restServer.StopAsync();
            
            var webPortal = Process.GetProcessById(webPortalProcess.Id);
            if (webPortal.HasExited)
            {
                return;
            }
            
            webPortal.Kill();
            webPortal.WaitForExit();
        }

        /// <summary>
        /// Start the Web Portal React App
        /// </summary>
        /// <returns>WebPortal process</returns>
        private static Process StartWebPortal()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var rootPath = Path.Join(currentDir, "..\\..\\..\\..\\");
            var webPortalPath = Path.Join(rootPath, TestConfiguration.WebPortalDir);

            // Start the Web Portal
            var webPortalProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C npm start --prefix {webPortalPath}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };

            webPortalProcess.Start();

            return webPortalProcess;
        }

        /// <summary>
        /// Start the WebDriver
        /// </summary>
        public void StartWebDriver()
        {
            webDriver?.Close();
            webDriver?.Quit();
            webDriver = MakeDriver();
        }

        /// <summary>
        /// Create a Selenium WebDriver
        /// </summary>
        /// <returns>WebDriver</returns>
        private IWebDriver MakeDriver()
        {
            var builder = SeleniumWebDriverBuilderFactory.MakeWebDriverBuilder(TestConfiguration.BrowserType);

            if (TestConfiguration.SeleniumUiMode == "Headless")
            {
                builder = builder.SetHeadless();
            }

            var driver = builder.Build();
            var manage = driver.Manage();
            manage.Cookies.DeleteAllCookies();

            configuredSize = new Size(
                Convert.ToInt32(TestConfiguration.TestBrowserWidth),
                Convert.ToInt32(TestConfiguration.TestBrowserHeight));
            manage.Window.Size = configuredSize;

            return driver;
        }
    }
}
