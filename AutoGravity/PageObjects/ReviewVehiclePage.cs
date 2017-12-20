using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class ReviewVehiclePage : BasePage
    {
        //used to grab the specs we are interested in testing against based on what the inventory card listed in the previous page
        public enum SPECS
        {
            STOCK=0,
            VIN,
            EXT_COLOR,
            INT_COLOR,
        }

        private const string BUTTON_CLASS = "buttonNext___2w_Xa";
        private const string SPEC_VALUES_CLASS = "overviewSpecValues___2MS8o";
        private const string PRICE_CLASS = "h3___FJGQg";
        private const string CAR_CLASS = "h2___1ghTj";
        private const string YEAR_CLASS = "h1___2yrzK";
        private const string DEALER_NAME = "dealerName___B6EKG";

        public ReviewVehiclePage(IWebDriver driver) : base(driver) { }

        [FindsBy(How = How.ClassName,Using = BUTTON_CLASS)]
        public IWebElement NextButton { get; set; }

        [FindsBy(How = How.ClassName, Using = PRICE_CLASS)]
        public IWebElement Price { get; set; }

        [FindsBy(How = How.ClassName,Using = CAR_CLASS)]
        public IWebElement CarName { get; set; }

        [FindsBy(How = How.ClassName,Using = YEAR_CLASS)]
        public IWebElement Year { get; set; }

        [FindsBy(How = How.ClassName,Using = DEALER_NAME)]
        public IWebElement DealerName { get; set; }

        [FindsBy(How = How.ClassName,Using = SPEC_VALUES_CLASS)]
        public IList<IWebElement> SpecValues { get; set; }
        public bool HasSpecValues
        {
            get { return SpecValues.Count > 0; }
        }

        public IWebElement Stock
        {
            get
            {
                IWebElement element = HasSpecValues ? SpecValues[(int)SPECS.STOCK] : null;
                if (element == null) throw new NoSuchElementException("Stock property no such class name: " + SPEC_VALUES_CLASS);
                return element;
            }
        }

        public IWebElement Vin
        {
            get
            {
                IWebElement element = HasSpecValues ? SpecValues[(int)SPECS.VIN] : null;
                if (element == null) throw new NoSuchElementException("Vin property no such class name: " + SPEC_VALUES_CLASS);
                return element;
            }
        }

        public IWebElement ExteriorColor
        {
            get
            {
                IWebElement element = HasSpecValues ? SpecValues[(int)SPECS.EXT_COLOR] : null;
                if (element == null) throw new NoSuchElementException("ExteriorColor property no such class name: " + SPEC_VALUES_CLASS);
                return element;
            }
        }

        public IWebElement InteriorColor
        {
            get
            {
                IWebElement element = HasSpecValues ? SpecValues[(int)SPECS.INT_COLOR] : null;
                if (element == null) throw new NoSuchElementException("InteriorColor property no such class name: " + SPEC_VALUES_CLASS);
                return element;
            }
        }



    }
}
