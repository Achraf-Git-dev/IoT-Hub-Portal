// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace TesteE2E
{
    using NUnit.Framework;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    public class AddDevice
    {
        private IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArguments("--headless");
            new DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver(options);
        }

        [Test]
        public void AddDeviceTest()
        {
            driver.Navigate().GoToUrl("https://cgigeiotdemoportal.azurewebsites.net/");

            System.Threading.Thread.Sleep(10000);

            //login
            driver.FindElement(By.Id("username")).SendKeys("");
            driver.FindElement(By.Id("password")).SendKeys("");
            driver.FindElement(By.Id("kc-login")).Click();

            System.Threading.Thread.Sleep(8000);

            // list
            driver.FindElement(By.CssSelector(".mud-paper:nth-child(1) .mud-nav-link-text")).Click();
            System.Threading.Thread.Sleep(8000);
            driver.FindElement(By.Id("addDeviceButton")).Click();

            System.Threading.Thread.Sleep(8000);

            //giving DeviceId and DeviceName
            driver.FindElement(By.CssSelector(".mud-input-adornment path:nth-child(2)")).Click();
            driver.FindElement(By.CssSelector(".mud-typography-body2")).Click();
            driver.FindElement(By.Id("DeviceID")).Click();
            driver.FindElement(By.Id("DeviceID")).SendKeys("6bc7dc880f60ba3b");
            driver.FindElement(By.Id("DeviceName")).Click();
            driver.FindElement(By.Id("DeviceName")).SendKeys("6bc7dc880f60ba3b");
            driver.FindElement(By.CssSelector("#SaveButton > .mud-button-label")).Click();

            System.Threading.Thread.Sleep(8000);

            //logout
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
