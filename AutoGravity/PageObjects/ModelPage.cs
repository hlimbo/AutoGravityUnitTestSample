using System;
using System.Collections;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.ObjectModel;


namespace AutoGravity.PageObjects
{
    class ModelPage : BasePage
    {
        private const string CAR_MODEL = "model___3RrUN";
        private const string INPUT = "input___26qtV";
        private const string FIND_BUTTON = "blueButton___3S9-a";
        private const string GEO_BUTTON = "geoButton___1PU6W";
        private const string LOC_MODAL = "locationModalContent___3yPhK";
        private Random rng_;//reference obtained from UnitTest
        private ReadOnlyCollection<IWebElement> modelsCollection_;
        private IWebElement randomModelType_;

        public ModelPage(Random rng, IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.ClassName, Using = LOC_MODAL)]
        public IWebElement LocationModalContent { get; set; } //used to check if this location modal pops up

        [FindsBy(How = How.ClassName, Using = INPUT)]
        public IWebElement LocationInputField { get; set; }

        [FindsBy(How = How.ClassName, Using = FIND_BUTTON)]
        public IWebElement FindLocationButton { get; set; }

        [FindsBy(How = How.ClassName, Using = GEO_BUTTON)]
        public IWebElement UseMyLocationButton { get; set; }

        [FindsBy(How = How.ClassName, Using = CAR_MODEL)]
        public ReadOnlyCollection<IWebElement> ModelsCollection
        {
            get
            {
                modelsCollection_ = driver_.FindElements(By.ClassName(CAR_MODEL));
                return modelsCollection_;
            }
        }

        //picks a random car model everytime this property is accessed
        public IWebElement SelectRandomModelType
        {
            get
            {
                randomModelType_ = ModelsCollection[rng_.Next(ModelsCollection.Count)];
                return randomModelType_;
            }
        }

        //used for debugging
        //must call RandomModelType property before this property can be accessed
        public string RandomModelTypeTitle
        {
            get { return randomModelType_ == null ? "null" : randomModelType_.FindElement(By.ClassName("label___2tptv")).Text; }
        }
    }
}
