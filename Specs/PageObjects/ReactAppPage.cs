using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Specs.PageObjects
{
    /// <summary>
    /// Class for the React App Page, used for retrieving elements in the React App Page
    /// </summary>
    public class ReactAppPage : PageObject
    {
        private readonly WebDriverWait longWait;
        private readonly WebDriverWait shortWait;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactAppPage"/> class.
        /// </summary>
        /// <param name="driver">WebDriver</param>
        public ReactAppPage(IWebDriver driver)
        {
            longWait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            shortWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Gets the FetchData menu element
        /// </summary>
        /// <returns>FetchData menu element</returns>
        public IWebElement GetFetchDataMenu()
        {
            return WaitForElementById(shortWait, "fetchDataMenu");
        }

        /// <summary>
        /// Gets the ToDoItem Table Rows
        /// </summary>
        /// <returns>ToDoItem Table Rows element</returns>
        public IReadOnlyCollection<IWebElement> GetToDoItemTableRows()
        {
            var toDoItemTable = WaitForElementByXPath(longWait, @"//table");

            return toDoItemTable.FindElements(By.XPath("//tbody/tr"));
        }
    }
}
