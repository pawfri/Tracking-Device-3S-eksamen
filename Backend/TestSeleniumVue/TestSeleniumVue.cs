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
    }
}
