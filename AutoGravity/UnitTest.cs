using System;
using System.Threading;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Device.Location;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.PageObjects;
using AutoGravity.PageObjects;

//note to self firefox is weird with the testing ~ unsure how I would override the use location setting
namespace AutoGravity
{
    [TestFixture]
    public class UnitTest
    {
        private IWebDriver browser_;
        private GeoCoordinate coordinate_;
        public const string HOME_PAGE = "https://www.autogravity.com";
        public const string ZIP_CODE = "92780";

        //source: https://msdn.microsoft.com/en-us/library/ctssatww(v=vs.110).aspx
        private Random rng_;
        //time for thread to sleep to make sure rng doesn't generate identical sequences of pseudo random numbers
        private const int TIME_TO_SLEEP = 20; //measured in milliseconds
        private const double TIMEOUT = 10.0;//measured in seconds

        [OneTimeSetUp]
        public void Init()
        {
            //https://stackoverflow.com/questions/8815895/why-is-thread-sleep-so-harmful
            //sleep in the beginning so no time related issues should occur
            Thread.Sleep(TIME_TO_SLEEP);
            rng_ = new Random();

            //debugging errors and exceptions that get caught (e.g. NoSuchElementException)
            //Errors.log should be found in bin/Debug folder
            Trace.Listeners.Add(new TextWriterTraceListener("Errors.log", "myListener"));

            FirefoxOptions options = new FirefoxOptions();
            //automatically allow consent to share location
            options.SetPreference("geo.prompt.testing", true);
            options.SetPreference("geo.prompt.testing.allow", true);
            //create a mockup location
            options.SetPreference("geo.enabled", true);
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.Start();
            //default to Irvine location if current location not found
            coordinate_ = (watcher.Position.Location.IsUnknown) ? new GeoCoordinate(33.684567, -117.826505) : watcher.Position.Location;
            string format = "data:application/json,{\"location\": {\"lat\": " + coordinate_.Latitude + ", \"lng\": " + coordinate_.Longitude + "}, \"accuracy\": 100.0}";
            Trace.WriteLine("watcher: " + watcher.Position.Location);
            Trace.WriteLine(format);
            options.SetPreference("geo.wifi.uri", format);
            watcher.Dispose();

            browser_ = new FirefoxDriver(options);
            browser_.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(TIMEOUT);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            browser_.Quit();
            Trace.Close();
        }

        [Test]
        public void Create_Credit_App_Firefox()
        {
            browser_.Navigate().GoToUrl(HOME_PAGE);

            //1st step is to select get started button
            try
            {
                HomePage homePage = new HomePage(browser_);
                homePage.GetStartedButton.Click();
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            //2nd step is to randomly "Select Make"...
            try
            {
                MakePage makePage = new MakePage(rng_, browser_);
                Trace.WriteLine("MakesCollection Count: " + makePage.MakesCollection.Count);
                IWebElement randomMakeType = makePage.SelectRandomMakeType();
                Trace.WriteLine("Make Type Selected: " + makePage.RandomMakeTypeTitle);
                randomMakeType.Click();
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            try
            {
                //3rd step is to randomly select a car
                ModelPage modelPage = new ModelPage(rng_, browser_);
                Trace.WriteLine("ModelsCollection Count: " + modelPage.ModelsCollection.Count);
                IWebElement randomModelType = modelPage.SelectRandomModelType();
                Trace.WriteLine("Car Model Selected: " + modelPage.RandomModelTypeTitle);
                randomModelType.Click();

                //4th step is to Enter Location
                //check if location modal pops up in webpage ~ stalls if modelPage cannot be found in current webpage
                if (modelPage.LocationModalContent.Displayed && modelPage.LocationModalContent.Enabled)
                {
                    Trace.WriteLine("Location Modal is Displayed");
                    modelPage.UseMyLocationButton.Click();
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            //5th step determine if the new page has a inventory list or a trim list
            try
            {
                InventoryPage inventoryPage = new InventoryPage(rng_, browser_);
                IWebElement inventoryCard = inventoryPage.SelectRandomInventoryCard();
                Trace.WriteLine("Inventory selected: " + inventoryPage.InventoryCardTitle());
                inventoryCard.Click();
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            //6th step is to select if its Trade-in? ... select No for now and hit Next
            //IWebElement nextButton = browser_.FindElement(By.CssSelector("button.newButton___3mgiP"));
            try
            {
                IWebElement nextButton = browser_.FindElement(By.CssSelector("button.buttonNext___2w_Xa"));
                nextButton.Click();
                IWebElement nextButton2 = browser_.FindElement(By.CssSelector("button.newButton___3mgiP"));
                nextButton2.Submit();
            }
            catch(NoSuchElementException ex2)
            {
                Trace.TraceWarning(ex2.Message);
                IWebElement nextButton2 = browser_.FindElement(By.CssSelector("button.newButton___3mgiP"));
                nextButton2.Submit();
            }

            //7th step is to select the first dealer available and click on 'Select This Dealer' button to continue
            IWebElement selectThisDealerButton = browser_.FindElement(By.ClassName("dealerSelectBtn___8r46A"));
            selectThisDealerButton.Click();

            Trace.WriteLine("timeout time in seconds: " + browser_.Manage().Timeouts().ImplicitWait.Duration().TotalSeconds);

            //8th step is to stop at "Search For Financing" Personal Information Page
            IWebElement personalInfoText = browser_.FindElement(By.CssSelector("h4.title"));
            Assert.AreEqual("Personal Information", personalInfoText.Text);
            Trace.WriteLine("Test Succeeded!");

        }
    }
}
