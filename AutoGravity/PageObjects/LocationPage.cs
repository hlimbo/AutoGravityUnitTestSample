using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace AutoGravity.PageObjects
{
    class LocationPage : BasePage
    {
        private const string INPUT = "input___26qtV";
        private const string FIND_BUTTON = "blueButton___3S9-a";
        private const string GEO_BUTTON = "geoButton___1PU6W";
        private const string LOCATION_HEADER_SELECTOR = ".headerLink___RUeIy.zipCode___1l6tm";
        private const string LOC_TEXT_SELECTOR = "span.zipCode___1l6tm";

        //use this string to see if location is already set
        private const string NA = "N/A";

        public LocationPage(IWebDriver driver) : base(driver) { }

        [FindsBy(How = How.CssSelector, Using = LOC_TEXT_SELECTOR)]
        public IWebElement LocationText { get; set; }
        public bool IsLocationSpecified { get { return !LocationText.Text.Equals(NA); } }

        [FindsBy(How = How.ClassName, Using = INPUT)]
        public IWebElement LocationInputField { get; set; }

        [FindsBy(How = How.ClassName, Using = FIND_BUTTON)]
        public IWebElement FindLocationButton { get; set; }

        [FindsBy(How = How.ClassName, Using = GEO_BUTTON)]
        public IWebElement UseMyLocationButton { get; set; }

        [FindsBy(How = How.CssSelector, Using = LOCATION_HEADER_SELECTOR)]
        public IWebElement LocationHeaderButton { get; set; }
    }
}
