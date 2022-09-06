using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WPImporter.BotWP
{
    public class Bot
    {
        private readonly string _URL;
        private readonly List<string> _ratingCategories;

        public Bot(string url)
        {
            _URL = url;
            _ratingCategories = new List<string>
            {
                "rating-kontakt",
                "rating-terminowosc",
                "rating-cena",
                "rating-jakosc-wykonania"
            };
        }

        public void AddComment(long rating, string author, string comment, string companyName)
        {
            var options = new ChromeOptions();

            // options.AddArgument("--headless");            

            using var driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl(_URL);

            Thread.Sleep(3000);

            var authorElement = driver.FindElement(By.Id("author"));
            authorElement.SendKeys(author);

            var commentElement = driver.FindElement(By.Id("comment"));
            commentElement.SendKeys($"{comment} - opinia pobrana z Google wizytówki firmy {companyName}");

            foreach (var category in _ratingCategories)
            {
                var ratingElement = driver.FindElement(By.CssSelector($"label[for='{category}-{rating}']"));
                ratingElement.Click();
            }

            Thread.Sleep(3000);

            driver.FindElement(By.Id("submit")).Click();
        }
    }
}
