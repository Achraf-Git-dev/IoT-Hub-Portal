//// Copyright (c) CGI France. All rights reserved.
//// Licensed under the MIT license. See LICENSE file in the project root for full license information.

//namespace AzureIoTHub.Portal.Tests.E2E
//{
//    using AzureIoTHub.Portal.Tests.E2E.Pages;
//    using NUnit.Framework;
//    using OpenQA.Selenium.Chrome;
//    using OpenQA.Selenium;
//    using OpenQA.Selenium.Support.UI;

//    public class DeleteDevice
//    {
//        private IWebDriver driver;

//        [SetUp]
//        public void SetUp()
//        {
//            driver = new ChromeDriver();
//        }

//        [Test]
//        public void DeleteDeviceTest()
//        {
//            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
//            var loginpage = new LoginPage(driver, wait);

//            loginpage.Login("achraf.boujida@etu.uca.fr", "dDpDDhSFL7VvrQA");

//            _ = wait.Until(d => d.FindElement(By.CssSelector(".mud-paper:nth-child(1) .mud-nav-link-text")).Displayed);

//            // list
//            driver.FindElement(By.CssSelector(".mud-paper:nth-child(1) .mud-nav-link-text")).Click();

//            _ = wait.Until(d => d.FindElement(By.CssSelector("#delete-device-6bc7dc880f60ba3b path:nth-child(2)")).Displayed);

//            // deleting
//            driver.FindElement(By.CssSelector("#delete-device-6bc7dc880f60ba3b path:nth-child(2)")).Click();
//            driver.FindElement(By.CssSelector("#delete-device > .mud-button-label")).Click();

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
