using OpenQA.Selenium;

namespace Specs.Selenium
{
    /// <summary>
    /// Interface for Web Driver builders
    /// </summary>
    public interface IWebDriverBuilder
    {
        /// <summary>
        /// Set web driver to use headless mode
        /// </summary>
        /// <returns>WebDriverBuilder</returns>
        IWebDriverBuilder SetHeadless();
        
        /// <summary>
        /// Build the web driver
        /// </summary>
        /// <returns>Web driver</returns>
        IWebDriver Build();
    }
}