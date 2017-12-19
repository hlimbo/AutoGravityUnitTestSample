using System;
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
        private Random rng_;//reference obtained from UnitTest
        private ReadOnlyCollection<IWebElement> makesCollection_;
        private IWebElement randomMakeType_;

        public MakePage(Random rng,IWebDriver driver) : base(driver)
        {
            rng_ = rng;
        }

        [FindsBy(How = How.ClassName, Using = CLASS_NAME)]
        public ReadOnlyCollection <IWebElement> MakesCollection
        {
            get
            {
                makesCollection_ = driver_.FindElements(By.ClassName(CLASS_NAME));
                return makesCollection_;
            }
        }

        //picks the randomMakeType_ of the car everytime this property is accessed
        public IWebElement SelectRandomMakeType
        {
            get
            {
                randomMakeType_ = MakesCollection[rng_.Next(MakesCollection.Count)];
                return randomMakeType_;
            }
        }

        //used for debugging
        //if RandomMake wasn't called.. RandomMakeTitle won't have a 
        //title until a random make type is selected
        public string RandomMakeTypeTitle
        {
            get { return randomMakeType_ == null ? "null" : randomMakeType_.FindElement(By.ClassName("image___1p6Dn")).GetAttribute("title"); }
        }
    }
}
