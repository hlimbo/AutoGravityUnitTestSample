using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class PersonalInformationPage : BasePage
    {
        private const string TITLE_SELECTOR = "h4.title";

        public PersonalInformationPage(IWebDriver driver) : base(driver) { }

        [FindsBy(How = How.CssSelector, Using = TITLE_SELECTOR)]
        public IWebElement PersonalInformation { get; set; }
    }
}
