using System;
using System.Threading;
using System.Diagnostics;
using System.Device.Location;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using AutoGravity.PageObjects;


namespace AutoGravity
{
    [TestFixture]
    public class UnitTest
    {
        private IWebDriver browser_;
        private GeoCoordinate coordinate_;
        private const string HOME_PAGE = "https://www.autogravity.com";
        private const string ZIP_CODE = "92780";
        private const string MIN_USED_VEHICLE_PRICE = "6000";
        private const string USED_ODOMETER_VALUE = "1";

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
            //Info.log should be found in bin/Debug folder
            Trace.Listeners.Add(new TextWriterTraceListener("Info.log", "myListener"));

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
            options.SetPreference("geo.wifi.uri", format);
            watcher.Dispose();

            Trace.WriteLine("---------");
            browser_ = new FirefoxDriver(options);
            browser_.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(TIMEOUT);
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
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Assert.Fail("Fail at Home page");
            }

            //2nd step is to randomly "Select Make" 
            string newUsedText = "";
            try
            {
                MakePage makePage = new MakePage(rng_, browser_);

                IWebElement randomButton = makePage.SelectRandomNewUsedButton();
                randomButton.Click();
                newUsedText = randomButton.Text;
                Trace.WriteLine("New/Used: " + randomButton.Text);
                Trace.WriteLine("MakesCollection Count: " + makePage.MakesCollection.Count);
                IWebElement randomMakeType = makePage.SelectRandomMakeType();
                Trace.WriteLine("Make Type Selected: " + makePage.RandomMakeTypeTitle);
                randomMakeType.Click();
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
                Assert.Fail("Fail at Select Make page");
            }

            try
            {
                //3rd step is to randomly select a car
                PageSelector modelPageSelector = new PageSelector(rng_, browser_);
                BasePage nextPage = modelPageSelector.GetNewOrUsedModelPage(newUsedText);

                if(nextPage is ModelPage)
                {
                    ModelPage modelPage = (ModelPage)nextPage;
                    Trace.WriteLine("ModelsCollection Count: " + modelPage.ModelsCollection.Count);
                    IWebElement randomModelType = modelPage.SelectRandomModelType();
                    Trace.WriteLine("Car Model Selected: " + modelPage.RandomModelTypeTitle);
                    randomModelType.Click();
                }
                else if(nextPage is UsedPage)
                {
                    UsedPage usedModelPage = (UsedPage)nextPage;
                    Trace.WriteLine("Used ModelsCollection Count: " + usedModelPage.UsedCollection.Count);
                    IWebElement randomUsedModelType = usedModelPage.SelectRandomUsedItem();
                    Trace.WriteLine("Used Car Model Selected: " + randomUsedModelType.Text);
                    randomUsedModelType.Click();
                }

                //4th step
                LocationPage location = new LocationPage(browser_);
                if(!location.IsLocationSpecified)
                {
                    Trace.WriteLine("Location Modal is Displayed");
                    location.UseMyLocationButton.Click();
                    //alternate way of inputting location information
                    //location.LocationInputField.SendKeys(ZIP_CODE);
                    //location.FindLocationButton.Click();
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
                Assert.Fail("Fail at Model Selection page");
            }

            //5th step determine if the new page has a inventory list or a trim list
            bool wasTrimPageVisited = false;
            try
            {
                PageSelector pageSelector = new PageSelector(rng_,browser_);
                BasePage newPage = pageSelector.GetNewPage();

                if (newPage is TrimPage)
                {
                    TrimPage trimPage = (TrimPage)newPage;
                    IWebElement trimCard = trimPage.SelectRandomTrimCard();
                    Trace.WriteLine("Trim selected: " + trimPage.TrimCardTitle.Text);
                    trimCard.Click();
                    wasTrimPageVisited = true;
                }
                else if(newPage is InventoryPage)
                {
                    InventoryPage inventoryPage = (InventoryPage)newPage;
                    IWebElement inventoryCard = inventoryPage.SelectRandomInventoryCard();
                    Trace.WriteLine("Inventory selected: " + inventoryPage.InventoryCardTitle.Text);
                    string dealerName = inventoryPage.DealerName.Text;
                    inventoryCard.Click();

                    //Review Vehicle Page
                    ReviewVehiclePage reviewVehiclePage = new ReviewVehiclePage(browser_);
                    Trace.WriteLine("Dealership Name: " + reviewVehiclePage.DealerName.Text);
                    Assert.AreEqual(dealerName, reviewVehiclePage.DealerName.Text);
                    reviewVehiclePage.NextButton.Click();
                }
                else if(newPage is UsedPage)
                {
                    UsedPage usedYearPage = new UsedPage(rng_, browser_);
                    IWebElement yearElement = usedYearPage.SelectRandomUsedItem();
                    Trace.WriteLine("Used Year: " + yearElement.Text);
                    yearElement.Click();

                    UsedPage usedTrimPage = new UsedPage(rng_, browser_);
                    IWebElement trimElement = usedTrimPage.SelectRandomUsedItem();
                    Trace.WriteLine("Used Trim: " + trimElement.Text);
                    trimElement.Click();
                    wasTrimPageVisited = true;
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
                Assert.Fail("Fail at PageSelector");
            }

            //6th step ~ no trade-in
            try
            {
                ReviewDetailsPage reviewDetailsPage = new ReviewDetailsPage(rng_,browser_);

                if(newUsedText.Equals("used",StringComparison.InvariantCultureIgnoreCase))
                {
                    reviewDetailsPage.UsedVehiclePriceInput.SendKeys(MIN_USED_VEHICLE_PRICE);
                    reviewDetailsPage.OdomoterReadingInput.SendKeys(USED_ODOMETER_VALUE);
                    IWebElement financeType = browser_.FindElement(By.ClassName("sectionTitleBar___1cVBK"));
                    Trace.WriteLine("used finance type: " + financeType.Text);
                }
                else //new car
                {
                    Trace.WriteLine("Finance Toggle Count: " + reviewDetailsPage.FinanceToggles.Count);
                    reviewDetailsPage.SelectRandomFinanceToggle().Click();
                    Trace.WriteLine("finance type: " + reviewDetailsPage.FinanceType.Text);
                }

                reviewDetailsPage.NoTradeInButton.Click();
                reviewDetailsPage.NextButton.Click();
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
                Assert.Fail("Fail at Review Details page");
            }

            //7th step ~ pick a random dealership if TrimPage was visited instead of inventory page
            if (wasTrimPageVisited)
            {
                try
                {
                    DealershipPage dealershipPage = new DealershipPage(rng_, browser_);
                    dealershipPage.SelectRandomDealer();
                    Trace.WriteLine("Dealership name selected: " + dealershipPage.DealerName.Text);
                    
                    if (!dealershipPage.SelectDealerButton.Displayed)
                        dealershipPage.ToggleDropdown.Click();

                    foreach(IWebElement button in dealershipPage.DealerButtons)
                        Trace.WriteLine("display: " + button.Displayed);

                    dealershipPage.SelectDealerButton.Click();
                }
                catch(Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    Assert.Fail("Fail at Dealership Page");
                }
            }

            //8th step is to stop at "Search For Financing" Personal Information Page
            try
            {
                PersonalInformationPage personalInfoPage = new PersonalInformationPage(browser_);
                Assert.AreEqual("Personal Information", personalInfoPage.PersonalInformation.Text);
                Trace.WriteLine("Test Succeeded!");

            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine("Test Failed!");
                Assert.Fail("Fail at Personal Information page");
            }
        }
    }
}
