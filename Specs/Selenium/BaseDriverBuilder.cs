using OpenQA.Selenium;
using System.Reflection;

namespace Specs.Selenium
{
    /// <summary>
    /// Base class for the Web Driver builders
    /// </summary>
    public abstract class BaseDriverBuilder : IWebDriverBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDriverBuilder"/> class.
        /// </summary>
        /// <param name="options">the options for the driver</param>
        protected BaseDriverBuilder(DriverOptions options)
        {
            Options = options;
            CommandTimeout = TimeSpan.FromMinutes(30);
        }

        /// <summary>
        /// Gets the driver options.
        /// </summary>
        protected DriverOptions Options { get; }

        /// <summary>
        /// Gets the driver command timeout
        /// </summary>
        protected TimeSpan CommandTimeout { get; }

        /// <inheritdoc/>
        public IWebDriver Build()
        {
            return MakeLocalWebDriver();
        }

        /// <inheritdoc/>
        public abstract IWebDriverBuilder SetHeadless();

        /// <summary>
        /// Get Assembly path
        /// </summary>
        /// <returns>Full path to the assembly</returns>
        protected static string GetAssemblyPath()
        {
            var codeBase = Assembly.GetExecutingAssembly().Location ?? throw new Exception();
            var uriBuilder = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uriBuilder.Path);
            return Path.GetFullPath(path);
        }

        /// <summary>
        /// Makes the instance of the local WebDriver.
        /// </summary>
        /// <returns>the driver instance</returns>
        protected abstract IWebDriver MakeLocalWebDriver();
    }
}
