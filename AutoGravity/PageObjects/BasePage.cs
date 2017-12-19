using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class BasePage
    {
        protected IWebDriver driver_;
        public BasePage(IWebDriver driver)
        {
            driver_ = driver;
            PageFactory.InitElements(driver_, this);
        }
    }
}
