using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class HomePage : BasePage
    {
        private const string HOME = "button.homeButton___3IaYm";
        public HomePage(IWebDriver driver) : base(driver) {}

        [FindsBy(How = How.CssSelector, Using = HOME)]
        public IWebElement GetStartedButton { get; set; }
    }
}
