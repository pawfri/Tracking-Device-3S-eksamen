using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
namespace TestSeleniumVue
{
	[TestClass]
	public sealed class TestSeleniumVue
	{
		[TestMethod]
		public void DoesTheFindButtonExist()
		{

			//Arrange
			ChromeOptions options = new ChromeOptions();
			options.AddArgument("--headless=new");
			IWebDriver driver = new ChromeDriver(options);
			driver.Navigate().GoToUrl(@"http://127:0.0.1:5500");

			//Act
			IWebElement button = driver.FindElement(By.Id("findButton"));
			var buttonText = button.Text;

			//Assert
			Assert.AreEqual("Find", buttonText);
		}

		[TestMethod] // Formel for en automatiseret test, den virker ikke for vores projekt, men den kan bruges som inspiration
		public void TestMethod2()
		{
			IWebDriver driver = new ChromeDriver();
			driver.Navigate().GoToUrl(@"http://127:0.0.1:5500");

			var searchbox = driver.FindElement(By.Name("q"));
			searchbox.SendKeys("Selenium");
			searchbox.Submit();

			Assert.IsTrue(driver.Title.Contains("Selenium"));
			driver.Quit();

		}
	}
}
