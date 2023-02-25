﻿// Copyright (c) CGI France. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace TesteE2E
{
    using NUnit.Framework;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium;

    public class LoginTest
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
        public void TestLogin()
        {
            driver.Navigate().GoToUrl("https://cgigeiotdemoportal.azurewebsites.net/");

            System.Threading.Thread.Sleep(8000);

            driver.FindElement(By.Id("username")).SendKeys("");
            driver.FindElement(By.Id("password")).SendKeys("");
            driver.FindElement(By.Id("kc-login")).Click();

            System.Threading.Thread.Sleep(5000);

            Assert.That(driver.Url, Is.EqualTo("https://cgigeiotdemoportal.azurewebsites.net/"));

            driver.FindElement(By.CssSelector(".mud-menu-activator > .mud-button-root .mud-icon-root")).Click();
            driver.FindElement(By.CssSelector(".mud-list-item-icon")).Click();

            System.Threading.Thread.Sleep(5000);

            Assert.That(driver.Url, Is.EqualTo("https://cgigeiotdemoportal.azurewebsites.net/authentication/logged-out"));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
