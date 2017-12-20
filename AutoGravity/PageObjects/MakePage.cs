using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.ObjectModel;

namespace AutoGravity.PageObjects
{
    //this differs from ModelPage because MakePage has a new and used tab that can be selected
    //whereas ModelPage does not
    class MakePage : BasePage
    {
        private const string CLASS_NAME = "makeFrame___1XMiK";
        private const string MAKE_IMAGE = "image___1p6Dn";
        private const string NEW_USED_BUTTONS_SELECTOR = ".newUsed___1D9oj > .btn-group > .btn";
        private Random rng_;//reference obtained from UnitTest
        private IWebElement randomMakeType_;

        public MakePage(Random rng,IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.CssSelector, Using = NEW_USED_BUTTONS_SELECTOR)]
        public IList<IWebElement> NewUsedButtons { get; set; }
        public bool HasNewUsedButtons { get { return NewUsedButtons.Count > 0; } }

        public IWebElement SelectRandomNewUsedButton()
        {
            if (!HasNewUsedButtons) throw new NoSuchElementException("SelectRandomNewUsedButton() invalid class: " + NEW_USED_BUTTONS_SELECTOR);
            return NewUsedButtons[rng_.Next(NewUsedButtons.Count)];
        }

        [FindsBy(How = How.ClassName, Using = CLASS_NAME)]
        public  IList<IWebElement> MakesCollection{ get; set; }

        public bool HasMakeCollection
        {
            get { return MakesCollection.Count > 0; }
        }

        //picks the randomMakeType_ of the car everytime this property is accessed
        public IWebElement SelectRandomMakeType()
        {
            randomMakeType_ = HasMakeCollection ? MakesCollection[rng_.Next(MakesCollection.Count)] : null;
            if (randomMakeType_ == null) throw new NoSuchElementException("SelectRandomMakeType() invalid class name: " + CLASS_NAME);
            return randomMakeType_;
        }

        public IWebElement SelectFirstMakeType()
        {
            randomMakeType_ = HasMakeCollection ? MakesCollection[0] : null;
            if (randomMakeType_ == null) throw new NoSuchElementException("SelectFirstMakeType() invalid class name: " + CLASS_NAME);
            return randomMakeType_;
        }

        //used for debugging
        public string RandomMakeTypeTitle
        {
            get { return randomMakeType_ == null ? "null" : randomMakeType_.FindElement(By.ClassName(MAKE_IMAGE)).GetAttribute("title"); }
        }
    }
}
