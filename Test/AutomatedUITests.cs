using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;
using Xunit;

namespace Test
{
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver driver;

        public AutomatedUITests()
        {
            driver = new ChromeDriver(".");
        }

        [Fact]
        public void Create_WhenExecuted_ReturnsCreateView()
        {
            driver.Navigate().GoToUrl(@"https://localhost:44369/Employees/Create");
            Assert.Equal("Create - EmployeesApp", driver.Title);
        }

        [Fact]
        public void Create_UnderageEmployee_ReturnsErrorMessage()
        {
            driver.Navigate().GoToUrl("https://localhost:44369/Employees/Create");

            driver.FindElement(By.Id("Name")).SendKeys("Test Employee");
            driver.FindElement(By.Id("Age")).SendKeys("17");
            driver.FindElement(By.Id("AccountNumber")).SendKeys("123-9384613085-58");

            Thread.Sleep(TimeSpan.FromSeconds(3));

            driver.FindElement(By.Id("Create")).Click();

            var errorMessage = driver.FindElement(By.Id("Message")).Text;

            Assert.Equal("You must be of legal age", errorMessage);
        }

        [Fact]
        public void Create_WrongModelData_ReturnsErrorMessage()
        {
            driver.Navigate().GoToUrl("https://localhost:44369/Employees/Create");

            driver.FindElement(By.Id("Name")).SendKeys("Test Employee");
            driver.FindElement(By.Id("Age")).SendKeys("34");

            Thread.Sleep(TimeSpan.FromSeconds(3));

            driver.FindElement(By.Id("Create")).Click();

            var errorMessage = driver.FindElement(By.ClassName("AccountNumberError")).Text;

            Assert.Equal("Account Number is required", errorMessage);
        }

        [Fact]
        public void Create_WhenSuccessfullyExecuted_ReturnsIndexViewWithNewEmployee()
        {
            driver.Navigate().GoToUrl("https://localhost:44369/Employees/Create");

            driver.FindElement(By.Id("Name")).SendKeys("Another Test Employee");
            driver.FindElement(By.Id("Age")).SendKeys("34");
            driver.FindElement(By.Id("AccountNumber")).SendKeys("123-9384613085-58");

            Thread.Sleep(TimeSpan.FromSeconds(3));

            driver.FindElement(By.Id("Create")).Click();

            Assert.Equal("Index - EmployeesApp", driver.Title);
            Assert.Contains("Another Test Employee", driver.PageSource);
            Assert.Contains("34", driver.PageSource);
            Assert.Contains("123-9384613085-58", driver.PageSource);
        }

        [Fact]
        public void Create_Reserva_ReturnsSuccesfulMessage()
        {
            driver.Navigate().GoToUrl("https://reservas.utap.edu.co/login.php");

            driver.FindElement(By.Id("usuarioLog")).SendKeys("1787");
            driver.FindElement(By.Id("contrasenaLog")).SendKeys("Dasanzor");

            Thread.Sleep(TimeSpan.FromSeconds(3));

            driver.FindElement(By.XPath("button")).Click();

            Assert.Contains("BIENVENIDO A LAS RESERVAS UTAP", driver.PageSource);
        }

        public void Dispose()
        {
            // Stop all Selenium drivers
            //driver.Quit();
            //driver.Dispose();
        }
    }
}
