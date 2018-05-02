using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreflightHQ.ActionExecutor
{
    public class ActionExecuter
    {
        IWebDriver driver;
        WebDriverWait driverWait;

        public ActionExecuter(IWebDriver driver)
        {
            this.driver = driver;
            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }
        private void WaitUntilReady()
        {
            driverWait.Until(d =>
                ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }
        public void Execute(ClientAction action)
        {
            WaitUntilReady();
            driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            var by = action.Selector == null ? null : By.CssSelector(action.Selector);
            if (by != null)
                driverWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
            switch (action.ActionType)
            {
                case ActionType.Click:
                    driver.FindElement(by).Click();
                    break;
                case ActionType.Type:
                    SendKeysToElement(by, action.Value);
                    break;
                case ActionType.Navigate:
                    driver.Url = action.Value;
                    break;
                case ActionType.SetCheckpoint:
                    var elementText = driver.FindElement(by).Text;
                    var expectedText = System.Net.WebUtility.HtmlDecode(action.Value);

                    if (expectedText != elementText)
                    {
                        throw new ApplicationException($"'{action.Value}' is not equal to '{expectedText}' at {by.ToString()}");
                    }
                    break;
                case ActionType.Enter:
                    driver.FindElement(by).SendKeys(Keys.Enter);
                    break;
                case ActionType.Select:
                    new SelectElement(driver.FindElement(by)).SelectByText(action.Value);
                    break;
            }
        }
        public void Execute(List<ClientAction> actions)
        {
            foreach (var action in actions)
            {
                Execute(action);
            }
        }
      
        public void SendKeysToElement(By element, string valueToEnter)
        {
            driver.FindElement(element).Clear();
            driver.FindElement(element).SendKeys(valueToEnter);
        }

    }
}
