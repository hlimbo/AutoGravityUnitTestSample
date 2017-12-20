using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class ModelPage : BasePage
    {
        private const string CAR_MODEL = "model___3RrUN";
        private Random rng_;//reference obtained from UnitTest
        private IWebElement randomModelType_;

        public ModelPage(Random rng, IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

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
