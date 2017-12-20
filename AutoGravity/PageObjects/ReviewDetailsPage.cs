﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;


//TODO: figure out how to fill in drop down values for make,model, and trim when trade-in is yes
namespace AutoGravity.PageObjects
{
    class ReviewDetailsPage : BasePage
    {
        private const string BUTTON_CLASS = "newButton___3mgiP";
        private const string FINANCE_TOGGLE_CLASS = "financeTypeToggle___1UDYQ";
        private const string FINANCE_TYPE_CLASS = "typeLabel___2rlt9";
        private const string TRADE_IN_BUTTONS_CLASS = "toggleFieldButton___3zwvQ";
        private Random rng_;
        private int index_ = -1;
        private IWebElement randomFinanceToggle_;

        public ReviewDetailsPage(Random rng, IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.ClassName, Using = BUTTON_CLASS)]
        public IWebElement NextButton { get; set; }

        [FindsBy(How = How.ClassName, Using = FINANCE_TOGGLE_CLASS)]
        public IList<IWebElement> FinanceToggles { get; set; }
        public bool HasFinanceToggles
        {
            get { return FinanceToggles.Count > 0; }
        }

        public IWebElement SelectRandomFinanceToggle()
        {
            if (!HasFinanceToggles) throw new NoSuchElementException("SelectRandomFinanceToggle() invalid class: " + FINANCE_TOGGLE_CLASS);
            index_ = rng_.Next(FinanceToggles.Count);
            randomFinanceToggle_ = FinanceToggles[index_];
            return randomFinanceToggle_;
        }

        //must call SelectRandomFinanceToggle() before calling this property
        public IWebElement FinanceType
        {
            get
            {
                if (index_ == -1 || !HasFinanceToggles || randomFinanceToggle_ == null)
                    throw new NoSuchElementException("SelectFinanceToggleType invalid class: " + FINANCE_TOGGLE_CLASS);
                return FinanceTypes[index_];
            }
        }

        [FindsBy(How = How.ClassName, Using = FINANCE_TYPE_CLASS)]
        public IList<IWebElement> FinanceTypes { get; set; }
        public bool HasFinanceTypes
        {
            get { return FinanceTypes.Count > 0; }
        }

        [FindsBy(How = How.ClassName, Using = TRADE_IN_BUTTONS_CLASS)]
        public IList<IWebElement> TradeInButtons { get; set; }
        public bool HasTradeInButtons
        {
            get { return TradeInButtons.Count > 0; }
        }

        public IWebElement TradeInButton
        {
            get
            {
                if (!HasTradeInButtons) throw new NoSuchElementException("YesButton no such class: " + TRADE_IN_BUTTONS_CLASS);
                return TradeInButtons[1];
            }
        }

        public IWebElement NoTradeInButton
        {
            get
            {
                if (!HasTradeInButtons) throw new NoSuchElementException("NoButton no such class: " + TRADE_IN_BUTTONS_CLASS);
                return TradeInButtons[0];
            }
        }
    }
}
