using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Specs.Selenium
{
    /// <summary>
    /// Class for the Chrome Web Driver builder
    /// </summary>
    public class ChromeDriverBuilder : BaseDriverBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeDriverBuilder"/> class.
        /// </summary>
        public ChromeDriverBuilder() : base(new ChromeOptions())
        {
        }

        private ChromeOptions ChromeOptions => (Options as ChromeOptions)!;

        /// <inheritdoc/>
        public override IWebDriverBuilder SetHeadless()
        {
            ChromeOptions.AddArgument("headless");
            return this;
        }

        /// <inheritdoc/>
        protected override IWebDriver MakeLocalWebDriver()
        {
            ChromeOptions.Proxy = null;

            var path = GetAssemblyPath();
            var service = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(path));
            var driver = new ChromeDriver(service, ChromeOptions, CommandTimeout);
            return driver;
        }
    }
}
