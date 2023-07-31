using Microsoft.Extensions.Configuration;

namespace Specs.Utils
{
    /// <summary>
    /// Class for the Test Configuration
    /// </summary>
    public static class TestConfiguration
    {
        /// <summary>
        /// Initializes static members of the <see cref="TestConfiguration"/> class.
        /// </summary>
        static TestConfiguration()
        {
            var config = BuildConfiguration();
            TestConfig = config.GetSection("TestConfig");
        }

        /// <summary>
        /// Build the Configuration
        /// </summary>
        /// <returns>Configuration</returns>
        public static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.Development.json", false, true);
            return builder.Build();
        }

        /// <summary>
        /// Gets the Test Mode
        /// </summary>
        public static string TestMode => GetEnvironmentValue("TestMode");

        /// <summary>
        /// Gets the REST URL
        /// </summary>
        public static string RestUrl => GetEnvironmentValue("RestUrl");

        /// <summary>
        /// Gets the In Memory DB name
        /// </summary>
        public static string InMemoryDbName => GetEnvironmentValue("InMemoryDbName");

        /// <summary>
        /// Gets the Web Portal Directory
        /// </summary>
        public static string WebPortalDir => GetEnvironmentValue("WebPortalDir");

        /// <summary>
        /// Gets the Selenium UI Mode
        /// </summary>
        public static string SeleniumUiMode => GetEnvironmentValue("SeleniumUiMode");

        /// <summary>
        /// Gets the Browser Type
        /// </summary>
        public static string BrowserType => GetEnvironmentValue("BrowserType");

        /// <summary>
        /// Gets the Test Browser Width
        /// </summary>
        public static string TestBrowserWidth => GetEnvironmentValue("TestBrowserWidth");

        /// <summary>
        /// Gets the Test Browser Height
        /// </summary>
        public static string TestBrowserHeight => GetEnvironmentValue("TestBrowserHeight");

        private static IConfigurationSection TestConfig { get; }
        
        /// <summary>
        /// Gets the configuration environment value using a specified key in the app settings
        /// </summary>
        /// <param name="key">Configuration key</param>
        /// <returns>Configuration value</returns>
        private static string GetEnvironmentValue(string key)
        {
            var env = Environment.GetEnvironmentVariable(key);
            return env ?? TestConfig[key];
        }
    }
}
