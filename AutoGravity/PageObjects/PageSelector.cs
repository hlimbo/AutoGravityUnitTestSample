using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    //class determines the next page to go to from ModelPage
    class PageSelector : BasePage
    {
        private const string TRIM_CLASS = "vehicleTrims___3EZv9";
        private const string INV_CLASS = "Inventory___b9C0J";
        private const string USED_CLASS_PAGE = "list-group";
        private Random rng_;

        public PageSelector(Random rng,IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        //gives the new page object to initialize  via PageFactory (e.g. Is this page an Inventory or a Trim Page?)
        public BasePage GetNewPage()
        {
            if (DoesElementExist(driver_, By.ClassName(TRIM_CLASS)))
                return new TrimPage(rng_, driver_);
            if (DoesElementExist(driver_, By.ClassName(INV_CLASS)))
                return new InventoryPage(rng_, driver_);
            if (DoesElementExist(driver_, By.ClassName(USED_CLASS_PAGE)))
                return new UsedPage(rng_, driver_);

            throw new NoSuchElementException("GetNewPage() invalid class names");
        }

        //use this class to get the next page depending on the param newUsedText
        //newUsedText param must only either be "new" or "used"
        public BasePage GetNewOrUsedModelPage(string newUsedText)
        {
            if (newUsedText.Equals("new", StringComparison.InvariantCultureIgnoreCase))
                return new ModelPage(rng_, driver_);
            if (newUsedText.Equals("used", StringComparison.InvariantCultureIgnoreCase))
                return new UsedPage(rng_, driver_);

            throw new NoSuchElementException("GetNewOrUsedModelPage() invalid newUsedText: " + newUsedText);
        }

        private bool DoesElementExist(IWebDriver src, By by)
        {
            try
            {
                src.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
