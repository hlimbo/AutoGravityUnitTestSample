using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;


namespace AutoGravity.PageObjects
{
    class TrimPage : BasePage
    {
        private const string TRIM_CLASS = "trimListRow___3xZy7";
        private const string CAR_NAME = "trimName___9if8r";
        private Random rng_;
        private IWebElement randomTrimCard_;
        public TrimPage(Random rng,IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.ClassName, Using = TRIM_CLASS)]
        public IList<IWebElement> TrimCards { get; set; }
        public bool HasTrimCards
        {
            get { return TrimCards.Count > 0; }
        }

        public IWebElement SelectRandomTrimCard()
        {
            randomTrimCard_ = HasTrimCards ? TrimCards[rng_.Next(TrimCards.Count)] : null;
            if (randomTrimCard_ == null) throw new NoSuchElementException();
            return randomTrimCard_;
        }

        //used for debugging
        public IWebElement TrimCardTitle
        {
            get
            {
                if (randomTrimCard_ == null) throw new NoSuchElementException("TrimCardTitle invalid class name: " + TRIM_CLASS);
                try
                {
                    return randomTrimCard_.FindElement(By.ClassName(CAR_NAME));
                }
                catch (NoSuchElementException)
                {
                    throw new NoSuchElementException("TrimCardTitle invalid class name: " + CAR_NAME);
                }
            }
        }


    }
}
