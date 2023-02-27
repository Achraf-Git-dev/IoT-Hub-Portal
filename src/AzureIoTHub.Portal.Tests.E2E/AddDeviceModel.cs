//// Copyright (c) CGI France. All rights reserved.
//// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//namespace AzureIoTHub.Portal.Tests.E2E
//{
//    using AzureIoTHub.Portal.Tests.E2E.Pages;
//    using NUnit.Framework;
//    using OpenQA.Selenium.Chrome;
//    using OpenQA.Selenium;
//    using OpenQA.Selenium.Support.UI;

//    public class AddDeviceModel
//    {
//        private IWebDriver driver;

//        [SetUp]
//        public void SetUp()
//        {
//            driver = new ChromeDriver();
//        }

//        [Test]
//        public void AddDeviceModelTest()
//        {
//            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
//            var loginpage = new LoginPage(driver, wait);

//            loginpage.Login("achraf.boujida@etu.uca.fr", "dDpDDhSFL7VvrQA");

//            _ = wait.Until(d => d.FindElement(By.CssSelector(".mud-paper:nth-child(2) .mud-nav-link-text")).Displayed);
//            driver.FindElement(By.CssSelector(".mud-paper:nth-child(2) .mud-nav-link-text")).Click();

//            Assert.That(driver.Url, Is.EqualTo("https://cgigeiotdemoportal.azurewebsites.net/device-models"));

//            var modelpage = new ModelPage(driver, wait);

//            modelpage.AddDeviceModel("NKE Watteco T Sensor", "The temperature and humidity uses a disposable 3.6V AA-type battery as power supply. It also includes an internal antenna.");

//            //modelpage.RemoveDeviceModel("NKE Watteco T Sensor");

//            //logout
//            loginpage.Logout();

//            System.Threading.Thread.Sleep(4000);
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            driver.Quit();
//        }
//    }
//}
