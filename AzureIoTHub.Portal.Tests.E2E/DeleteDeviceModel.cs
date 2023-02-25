// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace TesteE2E
{
    using NUnit.Framework;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;

    public class DeleteDeviceModel
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void DeleteDeviceModelTest()
        {
            driver.Navigate().GoToUrl("https://cgigeiotdemoportal.azurewebsites.net/");

            System.Threading.Thread.Sleep(10000);

            //login
            driver.FindElement(By.Id("username")).SendKeys("");
            driver.FindElement(By.Id("password")).SendKeys("");
            driver.FindElement(By.Id("kc-login")).Click();

            System.Threading.Thread.Sleep(8000);

            // list
            driver.FindElement(By.CssSelector(".mud-paper:nth-child(2) .mud-nav-link-text")).Click();
            System.Threading.Thread.Sleep(8000);

            // deleting
            driver.FindElement(By.CssSelector("#deleteButton .mud-icon-root")).Click();
            driver.FindElement(By.CssSelector(".mud-button-text-primary > .mud-button-label")).Click();

            System.Threading.Thread.Sleep(8000);

            // logout
            driver.FindElement(By.CssSelector(".mud-menu-activator > .mud-button-root .mud-icon-root")).Click();
            driver.FindElement(By.CssSelector(".mud-list-item-icon")).Click();

            System.Threading.Thread.Sleep(4000);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
