using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class InventoryPage : BasePage
    {
        private const string INV_CLASS = "InventoryCard___iUPv5";
        private const string CAR_NAME = "h1___3eE95";
        private const string DEALER_NAME = "dealerName___1fYRu";

        private Random rng_;
        private IWebElement randomInventoryCard_;

        public InventoryPage(Random rng, IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.ClassName, Using = INV_CLASS)]
        public IList<IWebElement> InventoryCards { get; set; }
        public bool HasInventoryCards
        {
            get { return InventoryCards.Count > 0; }
        }

        public IWebElement SelectRandomInventoryCard()
        {
            randomInventoryCard_ = HasInventoryCards ? InventoryCards[rng_.Next(InventoryCards.Count)] : null;
            if (randomInventoryCard_ == null) throw new NoSuchElementException("SelectRandomInventoryCard() invalid class name: " + INV_CLASS);
            return randomInventoryCard_;
        }

        public IWebElement SelectFirstInventoryCard()
        {
            if (HasInventoryCards)
                randomInventoryCard_ = InventoryCards[0];
            else
                throw new NoSuchElementException("SelectFirstInventoryCard() invalid class name: " + INV_CLASS);

            return randomInventoryCard_;
        }

        //used for debugging:
        public IWebElement InventoryCardTitle
        {
            get
            {
                if (randomInventoryCard_ == null) throw new NoSuchElementException("InventoryCardTitle invalid class name: " + INV_CLASS);
                try
                {
                    return randomInventoryCard_.FindElement(By.ClassName(CAR_NAME));
                }
                catch (NoSuchElementException)
                {
                    throw new NoSuchElementException("InventoryCardTitle invalid class name: " + CAR_NAME);
                }
            }
        }

        public IWebElement DealerName
        {
            get
            {
                if (randomInventoryCard_ == null) throw new NoSuchElementException("DealerName invalid class name: " + INV_CLASS);
                try
                {
                    return randomInventoryCard_.FindElement(By.ClassName(DEALER_NAME));
                }
                catch(NoSuchElementException)
                {
                    throw new NoSuchElementException("DealerName invalid class name: " + DEALER_NAME);
                }
            }
        }

    }
}
