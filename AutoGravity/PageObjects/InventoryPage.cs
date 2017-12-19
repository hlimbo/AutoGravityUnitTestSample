using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class InventoryPage : BasePage
    {
        private const string INV_CLASS = "InventoryCard___iUPv5";
        private const string INV_CLASS_ALT = "trimListRow___3xZy7";
        private const string CAR_NAME = "h1___3eE95";
        private const string CAR_NAME_ALT = "trimName___9if8r";
        private Random rng_;
        private IWebElement randomInventoryCard_;

        public InventoryPage(Random rng, IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.ClassName, Using = INV_CLASS, Priority = 0)]
        [FindsBy(How = How.ClassName, Using = INV_CLASS_ALT, Priority = 1)]
        public IList<IWebElement> InventoryCards { get; set; }
        public bool HasInventoryCards
        {
            get { return InventoryCards.Count > 0; }
        }

        public IWebElement SelectRandomInventoryCard()
        {
            randomInventoryCard_ = HasInventoryCards ? InventoryCards[rng_.Next(InventoryCards.Count)] : null;
            if (randomInventoryCard_ == null) throw new NoSuchElementException("SelectRandomInventoryCard() invalid class name: " + INV_CLASS_ALT);
            return randomInventoryCard_;
        }

        public IWebElement SelectFirstInventoryCard()
        {
            if (HasInventoryCards)
                randomInventoryCard_ = InventoryCards[0];
            else
                throw new NoSuchElementException("SelectRandomInventoryCard() invalid class name: " + INV_CLASS_ALT);

            return randomInventoryCard_;
        }

        //used for debugging:
        public string InventoryCardTitle()
        {
            if (randomInventoryCard_ == null) throw new NoSuchElementException("InventoryCardTitle() invalid class name: " + INV_CLASS_ALT);
            string inventoryCardName = "null";

            try
            {
                inventoryCardName = randomInventoryCard_.FindElement(By.ClassName(CAR_NAME)).Text;
            }
            catch(NoSuchElementException)
            {
                try
                {
                    inventoryCardName = randomInventoryCard_.FindElement(By.ClassName(CAR_NAME_ALT)).Text;
                }
                catch(NoSuchElementException)
                {
                    throw new NoSuchElementException("InventoryCardTitle() invalid class name: " + CAR_NAME_ALT);
                }
            }

             return inventoryCardName;
        }


    }
}
