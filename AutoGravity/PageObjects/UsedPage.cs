using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
namespace AutoGravity.PageObjects
{
    //use for Used model, year, and trim pages
    class UsedPage :BasePage
    {
        private const string LIST_ITEM_SELECTOR = ".list-group-item.item___2KSDy";
        private Random rng_;
        private IWebElement randomUsedItem_;

        public UsedPage(Random rng,IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.CssSelector,Using = LIST_ITEM_SELECTOR)]
        public IList<IWebElement> UsedCollection { get; set; }
        public bool HasUsedCollection { get { return UsedCollection.Count > 0; } }

        public IWebElement SelectRandomUsedItem()
        {
            if (!HasUsedCollection) throw new NoSuchElementException("SelectRandomUsedItem() invalid class: " + LIST_ITEM_SELECTOR);
            randomUsedItem_ = UsedCollection[rng_.Next(UsedCollection.Count)];
            return randomUsedItem_;
        }
    }
}
