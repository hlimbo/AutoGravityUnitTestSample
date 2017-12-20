using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    //class determines if the model selected from ModelPage 
    //brings you to either the Trim Page or the Inventory Page
    class PageSelector : BasePage
    {
        private const string TRIM_CLASS = "vehicleTrims___3EZv9";
        private const string INV_CLASS = "Inventory___b9C0J";
        private Random rng_;

        public PageSelector(Random rng,IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        //throws NoSuchElementException if page type could not be found
        //otherwise return the page type found on success
        private IWebElement PageType()
        {
            IWebElement page;
            if (!TryToFindElement(driver_, By.ClassName(TRIM_CLASS), out page))
            {
                if (!TryToFindElement(driver_, By.ClassName(INV_CLASS), out page))
                {
                    throw new NoSuchElementException("PageType() no such class name: " + INV_CLASS);
                }
            }

            return page;
        }

        //gives the new page object to initialize  via PageFactory (e.g. Is this page an Inventory or a Trim Page?)
        public BasePage GetNewPage()
        {
            if (PageType().GetAttribute("class").Contains(TRIM_CLASS))
                return new TrimPage(rng_, driver_);
            else
                return new InventoryPage(rng_, driver_);
        }

        private bool TryToFindElement(IWebDriver src, By by, out IWebElement dstElement)
        {
            bool status;
            try
            {
                dstElement = src.FindElement(by);
                status = true;
            }
            catch (NoSuchElementException)
            {
                dstElement = null;
                status = false;
            }

            return status;
        }
    }
}
