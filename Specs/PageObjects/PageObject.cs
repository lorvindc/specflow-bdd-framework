using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Specs.PageObjects
{
    /// <summary>
    /// Class for PageObject to retrieve PageObject elements
    /// </summary>
    public abstract class PageObject
    {
        /// <summary>
        /// Wait for element using Xpath locator
        /// </summary>
        /// <param name="wait">WebDriverWait</param>
        /// <param name="xpath">Xpath locator of the element</param>
        /// <returns>WebElement</returns>
        public static IWebElement WaitForElementByXPath(WebDriverWait wait, string xpath)
        {
            return wait.Until(SeleniumExtras.WaitHelpers
                .ExpectedConditions
                .ElementExists(By.XPath(xpath)));
        }

        /// <summary>
        /// Wait for element using ID locator
        /// </summary>
        /// <param name="wait">WebDriverWait</param>
        /// <param name="id">ID locator of the element</param>
        /// <returns>WebElement</returns>
        public static IWebElement WaitForElementById(WebDriverWait wait, string id)
        {
            return wait.Until(SeleniumExtras.WaitHelpers
                .ExpectedConditions
                .ElementExists(By.Id(id)));
        }
    }
}
