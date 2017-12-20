using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class DealershipPage : BasePage
    {
        private const string DEALER_LIST_ITEMS_CLASS = "dealerListItem___3_3p6";
        private const string DEALER_SELECT_BUTTON_SELECTOR = "button.dealerSelectBtn___8r46A";
        private const string DEALER_NAME_CLASS = "dealerName___3xLSG";
        private const string DEALER_DROPDOWN = "chevronIcon___37mCh";
        private Random rng_;
        private IWebElement randomDealer_;
        private int index_ = -1;//used to locate the correct dealer button to click on.

        public DealershipPage(Random rng,IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.ClassName,Using = DEALER_LIST_ITEMS_CLASS)]
        public IList<IWebElement> DealerList { get; set; }
        public bool HasDealerList
        {
            get { return DealerList.Count > 0; }
        }

        [FindsBy(How = How.CssSelector, Using = DEALER_SELECT_BUTTON_SELECTOR)]
        public IList<IWebElement> DealerButtons { get; set; }
        public bool HasDealerButtons
        {
            get { return DealerButtons.Count > 0; }
        }

        public IWebElement SelectRandomDealer()
        {
            if (!HasDealerList) throw new NoSuchElementException("SelectRandomDealer() has invalid class name: " + DEALER_LIST_ITEMS_CLASS);
            index_ = rng_.Next(DealerList.Count);
            randomDealer_ = DealerList[index_];
            return randomDealer_;
        }

        public IWebElement SelectFirstDealer()
        {
            if (!HasDealerList) throw new NoSuchElementException("SelectFirstDealer() has invalid class name: " + DEALER_LIST_ITEMS_CLASS);
            index_ = 0;
            randomDealer_ = DealerList[0];
            return randomDealer_;
        }

        public IWebElement ToggleDropdown
        {
            get
            {
                if (!HasDealerList) throw new NoSuchElementException("ToggleDropdown() has invalid class name: " + DEALER_LIST_ITEMS_CLASS);
                if(randomDealer_ == null) throw new NoSuchElementException("ToggleDropdown must call SelectRandom Dealer before calling this property");
                return randomDealer_.FindElement(By.ClassName(DEALER_DROPDOWN));
            }
        }

        public IWebElement SelectDealerButton
        {
            get
            {
                if (randomDealer_ == null || index_ == -1 || !HasDealerButtons) throw new NoSuchElementException("SelectDealerButton(): must call SelectRandomDealer() before calling this property or the class is invalid: " + DEALER_SELECT_BUTTON_SELECTOR);
                return DealerButtons[index_];
            }
        }

        public IWebElement DealerName
        {
            get
            {
                if(randomDealer_ == null) throw new NoSuchElementException("DealerName: must call SelectRandomDealer() before calling this property or the class is invalid: " + DEALER_SELECT_BUTTON_SELECTOR);
                return randomDealer_.FindElement(By.ClassName(DEALER_NAME_CLASS));
            }
        }

        
    }
}
