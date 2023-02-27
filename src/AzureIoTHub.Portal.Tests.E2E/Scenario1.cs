//// Copyright (c) CGI France. All rights reserved.
//// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//namespace AzureIoTHub.Portal.Tests.E2E
//{
//    using AzureIoTHub.Portal.Tests.E2E.Pages;
//    using NUnit.Framework;
//    using OpenQA.Selenium.Chrome;
//    using OpenQA.Selenium;
//    using OpenQA.Selenium.Support.UI;

//    public class Scenario1
//    {
//        private IWebDriver driver;
//        private WebDriverWait wait;

//        [SetUp]
//        public void SetUp()
//        {
//            driver = new ChromeDriver();
//            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
//        }

//        [Test]
//        public void TestScenario1()
//        {
//            var loginpage = new LoginPage(driver, wait);

//            loginpage.Login("achraf.boujida@etu.uca.fr", "dDpDDhSFL7VvrQA");

//            var device = new DevicePage(driver, wait);

//            device.AddDevice("6bc7dc880f60ba3b", "6bc7dc880f60ba3b", "NKE Watteco T Sensor");

//            device.RemoveDevice("6bc7dc880f60ba3b", "NKE Watteco T Sensor");
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            driver.Quit();
//        }
//    }
//}
