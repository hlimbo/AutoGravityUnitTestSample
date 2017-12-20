using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class ModelPage : BasePage
    {
        private const string CAR_MODEL = "model___3RrUN";
        private const string INPUT = "input___26qtV";
        private const string FIND_BUTTON = "blueButton___3S9-a";
        private const string GEO_BUTTON = "geoButton___1PU6W";
        private const string LOCATION_HEADER_SELECTOR = ".headerLink___RUeIy.zipCode___1l6tm";
        private const string LOC_TEXT_SELECTOR = "span.zipCode___1l6tm";

        //use this string to see if location is already set
        private const string NA = "N/A";
        private Random rng_;//reference obtained from UnitTest
        private IWebElement randomModelType_;

        public ModelPage(Random rng, IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.CssSelector, Using = LOC_TEXT_SELECTOR)]
        public IWebElement LocationText { get; set; }
        public bool IsLocationSpecified
        {
            get { return !LocationText.Text.Equals(NA); }
        }

        [FindsBy(How = How.ClassName, Using = INPUT)]
        public IWebElement LocationInputField { get; set; }

        [FindsBy(How = How.ClassName, Using = FIND_BUTTON)]
        public IWebElement FindLocationButton { get; set; }

        [FindsBy(How = How.ClassName, Using = GEO_BUTTON)]
        public IWebElement UseMyLocationButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = LOCATION_HEADER_SELECTOR)]
        public IWebElement LocationHeaderButton { get; set; }

        [FindsBy(How = How.ClassName, Using = CAR_MODEL)]
        public IList<IWebElement> ModelsCollection{ get; set; }
        public bool HasModelsCollection
        {
            get { return ModelsCollection.Count > 0; }
        }

        public IWebElement SelectRandomModelType()
        {
            randomModelType_ = HasModelsCollection ? ModelsCollection[rng_.Next(ModelsCollection.Count)] : null;
            if (randomModelType_ == null) throw new NoSuchElementException("SelectRandomModelType() invalid class name: " + CAR_MODEL);
            return randomModelType_;            
        }
        public IWebElement SelectFirstModelType()
        {
            randomModelType_ = HasModelsCollection ? ModelsCollection[0] : null;
            if (randomModelType_ == null) throw new NoSuchElementException("SelectFirstModelType() invalid class name: " + CAR_MODEL);
            return randomModelType_;
        }

        //used for debugging
        //must call RandomModelType property before this property can be accessed
        public string RandomModelTypeTitle
        {
            get { return randomModelType_ == null ? "null" : randomModelType_.FindElement(By.ClassName("label___2tptv")).Text; }
        }
    }
}
