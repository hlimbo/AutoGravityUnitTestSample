using System;
using System.Threading;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Device.Location;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

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
            browser_.Close();
            Trace.Close();
        }

        [Test]
        public void Create_Credit_App_Firefox()
        {
            browser_.Navigate().GoToUrl(HOME_PAGE);
            try
            {
                //1st step is to have automation to click on Get Started Button
                IWebElement getStartedButton = browser_.FindElement(By.CssSelector("button.homeButton___3IaYm"));
                getStartedButton.Click();

                //2nd step is to "Select Make"... pick Audi for now!
                //IWebElement audi = browser_.FindElement(By.ClassName("makeFrame___1XMiK"));
                //audi.Click();

                //randomly select a make
                ReadOnlyCollection<IWebElement> makes = browser_.FindElements(By.ClassName("makeFrame___1XMiK"));
                IWebElement randomMake = makes[rng_.Next(makes.Count)];
                string makeTitle = randomMake.FindElement(By.ClassName("image___1p6Dn")).GetAttribute("title");
                Trace.WriteLine("make selected: " + makeTitle);
                randomMake.Click();

                //3rd step is to "Randomly select a car"... pick A3 Cabriolet for now
                //IWebElement a3Cabriolet = browser_.FindElement(By.ClassName("model___3RrUN"));
                //a3Cabriolet.Click();

                //randomly select a car
                ReadOnlyCollection<IWebElement> carModels = browser_.FindElements(By.ClassName("model___3RrUN"));
                IWebElement randomCarModel = carModels[rng_.Next(carModels.Count)];
                string carTitle = randomCarModel.FindElement(By.ClassName("label___2tptv")).Text;
                Trace.WriteLine("car selected: " + carTitle);
                randomCarModel.Click();

                //4th step is to Enter in City or Zip Code? ... Enter in zip code 92780 for now
                //IWebElement textbox = browser_.FindElement(By.CssSelector("input.input___26qtV "));
                //textbox.SendKeys(ZIP_CODE);

                //IWebElement findButton = browser_.FindElement(By.CssSelector("button.blueButton___3S9-a"));
                //findButton.Click();

                //use my location button press instead
                IWebElement useMyLocationButton = browser_.FindElement(By.ClassName("geoButton___1PU6W"));
                useMyLocationButton.Click();

                //determine if the new page has a inventory list or a trim list
                try
                {
                    IWebElement inventoryList = browser_.FindElement(By.CssSelector("div.InventoryList___2RhEw.container"));
                    //select first listed inventory item
                    IWebElement inventoryCard = browser_.FindElement(By.ClassName("InventoryCard___iUPv5"));
                    inventoryCard.Click();
                }
                catch(NoSuchElementException ex2)
                {
                    Trace.TraceWarning(ex2.Message);
                    try
                    {
                        IWebElement vehicleTrimsContainer = browser_.FindElement(By.CssSelector("section.vehicleTrims___3EZv9"));
                        //select first listed trim row
                        IWebElement vehicleTrimRow = vehicleTrimsContainer.FindElement(By.ClassName("trimListRow___3xZy7"));
                        vehicleTrimRow.Click();
                    }
                    catch(NoSuchElementException ex3)
                    {
                        Trace.TraceError(ex3.Message);
                    }
                }

                //5th step is to select either Lease or Loan ... pick Loan for now and select first option
                //ReadOnlyCollection<IWebElement> buttonOptions = browser_.FindElements(By.CssSelector("button.buttonTab___3591E"));
                //foreach (IWebElement buttonOption in buttonOptions)
                //{
                //    if (buttonOption.Text.Equals("Loan"))
                //    {
                //        buttonOption.Click();
                //        break;
                //    }
                //}

                //select 1st model 2.0 TFSI Premium FWD at $534/mo*
                //IWebElement tfsi = browser_.FindElement(By.ClassName("trimListRow___3xZy7"));
                //tfsi.Click();

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
            }
            catch(NoSuchElementException ex)
            {
                Trace.WriteLine(ex.Message);
                //IWebElement outer = browser_.FindElement(By.CssSelector("html"));
                //Trace.WriteLine(outer.GetAttribute("innerHTML"));

                Trace.WriteLine("timeout time in seconds: " + browser_.Manage().Timeouts().ImplicitWait.Duration().TotalSeconds);
                Trace.WriteLine("coordinate: " + coordinate_);
            }
            finally
            {
                //8th step is to stop at "Search For Financing" Personal Information Page
                IWebElement personalInfoText = browser_.FindElement(By.CssSelector("h4.title"));
                Assert.AreEqual("Personal Information", personalInfoText.Text);
                Trace.WriteLine("Test Succeeded!");
            }

            //9th step is to close the browser and end the test


            //here I will have to do some QA work and find if there are some things that can be checked for on this website
        }
    }
}
