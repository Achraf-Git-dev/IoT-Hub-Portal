//// Copyright (c) CGI France. All rights reserved.
//// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//namespace AzureIoTHub.Portal.Tests.E2E
//{
//    using AzureIoTHub.Portal.Tests.E2E.Pages;
//    using NUnit.Framework;
//    using OpenQA.Selenium.Chrome;
//    using OpenQA.Selenium;
//    using OpenQA.Selenium.Support.UI;

//    public class DeleteDeviceModel
//    {
//        private IWebDriver driver;

//        [SetUp]
//        public void SetUp()
//        {
//            driver = new ChromeDriver();
//        }

//        [Test]
//        public void DeleteDeviceModelTest()
//        {
//            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
//            var loginpage = new LoginPage(driver, wait);

//            loginpage.Login("achraf.boujida@etu.uca.fr", "dDpDDhSFL7VvrQA");

//            //_ = wait.Until(d => d.FindElement(By.CssSelector(".mud-paper:nth-child(2) .mud-nav-link-text")).Displayed);

//            //// list
//            //driver.FindElement(By.CssSelector(".mud-paper:nth-child(2) .mud-nav-link-text")).Click();
//            //_ = wait.Until(d => d.FindElement(By.CssSelector("#deleteButton .mud-icon-root")).Displayed);

//            //// deleting
//            //driver.FindElement(By.CssSelector("#deleteButton .mud-icon-root")).Click();
//            //driver.FindElement(By.CssSelector(".mud-button-text-primary > .mud-button-label")).Click();

//            ////confirming
//            //Assert.That((driver.FindElement(By.ClassName("mud-snackbar-content-message"))).Text, Is.EqualTo("Device model NKE Watteco T Sensor has been successfully deleted!"));

//            var m = new ModelPage(driver, wait);

//            m.RemoveDeviceModel("Non LoRa Device");

//            //Assert.That((driver.FindElement(By.ClassName("mud-snackbar-content-message"))).Text, Is.EqualTo("Device model NKE Watteco T Sensor has been successfully deleted!"));


//            // logout
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
