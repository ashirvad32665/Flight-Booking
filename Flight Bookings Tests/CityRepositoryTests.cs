using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CityService;
using CityService.Controllers;
using CityService.Process;
using CityService.Repository;
using CommonUse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Flight_Bookings_Tests
{
    [TestClass]
    public sealed class CityRepositoryTests
    {
        private Mock<ICity> _mockRepo;
        private CityProcess _cityProcess;
        private CityController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Mock the ICity repository
            _mockRepo = new Mock<ICity>();

            // Pass the mocked ICity repository to CityProcess
            _cityProcess = new CityProcess(_mockRepo.Object);

            // Initialize the controller with the CityProcess instance
            _controller = new CityController(_cityProcess);
        }

        // GetAllData
        [TestMethod]
        public async Task GetAllData_ShouldReturnOkResult_WhenCitiesExist()
        {
            // Arrange
            var cities = new List<City> { new City { CityCode = "DEL", CityName = "Delhi" }, new City { CityCode = "MUM", CityName = "Mumbai" } };
            _mockRepo.Setup(p => p.GetAllDataAsync()).ReturnsAsync(cities);

            // Act
            var result = await _controller.GetAllData();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); // Status code 200 for Ok
        }
        [TestMethod]
        public async Task GetAllData_ShouldReturnNotFound_WhenExceptionOccurs()
        {
            // Arrange
            _mockRepo.Setup(p => p.GetAllDataAsync()).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.GetAllData();

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode); // Status code 404 for Not Found
        }

        //Add City
       [TestMethod]
        public async Task AddCity_ShouldReturnOkResult_WhenCityIsAdded()
        {
            // Arrange
            var city = new City { CityCode = "KOL", CityName = "Kolkata" };
            _mockRepo.Setup(p => p.AddCityAsync(It.IsAny<City>())).ReturnsAsync(true);

            // Act
            var result = await _controller.AddCity(city);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public async Task AddCity_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var city = new City { CityCode = "KOL", CityName = "Kolkata" };
            _mockRepo.Setup(p => p.AddCityAsync(It.IsAny<City>())).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.AddCity(city);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode); // Status code 400 for BadRequest
        }

        // Update City
        [TestMethod]
        public async Task UpdateCity_ShouldReturnOkResult_WhenCityIsUpdated()
        {
            // Arrange
            var cityCode = "DEL";
            var city = new City { CityCode = "DEL", CityName = "Delhi" };
            _mockRepo.Setup(p => p.UpdateCityAsync(cityCode, city)).ReturnsAsync(city);

            // Act
            var result = await _controller.UpdateCity(cityCode, city);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); // Status code 200 for Ok
        }

        [TestMethod]
        public async Task UpdateCity_ShouldReturnNotFound_WhenCityNotFound()
        {
            // Arrange
            var cityCode = "XYZ";
            var city = new City { CityCode = "XYZ", CityName = "UnknownCity" };
            _mockRepo.Setup(p => p.UpdateCityAsync(cityCode, city)).ThrowsAsync(new ArgumentNullException("City not found"));

            // Act
            var result = await _controller.UpdateCity(cityCode, city);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode); // Status code 404 for NotFound
        }

        // GetByCityCode
        [TestMethod]
        public async Task GetCityByCityCode_ShouldReturnOkResult_WhenCityExists()
        {
            // Arrange
            var cityCode = "DEL";
            var city = new City { CityCode = "DEL", CityName = "Delhi" };
            _mockRepo.Setup(p => p.GetCityByCityCodeAsync(cityCode)).ReturnsAsync(city);

            // Act
            var result = await _controller.GetCityByCode(cityCode);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); // Status code 200 for Ok
        }

        [TestMethod]
        public async Task GetCityByCityCode_ShouldReturnNotFound_WhenCityDoesNotExist()
        {
            // Arrange
            var cityCode = "XYZ";
            _mockRepo.Setup(p => p.GetCityByCityCodeAsync(cityCode)).ThrowsAsync(new KeyNotExistException("City not found"));

            // Act
            var result = await _controller.GetCityByCode(cityCode);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode); // Status code 404 for NotFound
        }

        // Delete City
        [TestMethod]
        public async Task DeleteCity_ShouldReturnOkResult_WhenCityIsDeleted()
        {
            // Arrange
            var cityCode = "DEL";
            _mockRepo.Setup(p => p.DeleteCityAsync(cityCode)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCity(cityCode);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); // Status code 200 for Ok
        }

        [TestMethod]
        public async Task DeleteCity_ShouldReturnBadRequest_WhenDeleteFails()
        {
            // Arrange
            var cityCode = "XYZ";
            _mockRepo.Setup(p => p.DeleteCityAsync(cityCode)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCity(cityCode);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode); // Status code 400 for BadRequest
        }

        // Update Airport Charges
        [TestMethod]
        public async Task UpdateAirportCharge_ShouldReturnOkResult_WhenAirportChargeIsUpdated()
        {
            // Arrange
            var cityCode = "DEL";
            int airportCharge = 500;
            var updatedCity = new City { CityCode = "DEL", CityName = "Delhi", AirportCharge = airportCharge };
            _mockRepo.Setup(p => p.UpdateAirportChargeAsync(cityCode, airportCharge)).ReturnsAsync(updatedCity);

            // Act
            var result = await _controller.UpdateAirportCharge(cityCode, airportCharge);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); // Status code 200 for Ok
        }

        [TestMethod]
        public async Task UpdateAirportCharge_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var cityCode = "XYZ";
            int airportCharge = 500;
            _mockRepo.Setup(p => p.UpdateAirportChargeAsync(cityCode, airportCharge)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _controller.UpdateAirportCharge(cityCode, airportCharge);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode); // Status code 400 for BadRequest
        }

        // Get City By City Name

        [TestMethod]
        public async Task GetCityByCityName_ShouldReturnOkResult_WhenCityExists()
        {
            // Arrange
            var cityName = "Delhi";
            var city = new City { CityCode = "DEL", CityName = "Delhi" };
            _mockRepo.Setup(p => p.GetCityByNameAsync(cityName)).ReturnsAsync(city);

            // Act
            var result = await _controller.GetCityByName(cityName);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); // Status code 200 for Ok
        }

        [TestMethod]
        public async Task GetCityByCityName_ShouldReturnNotFound_WhenCityDoesNotExist()
        {
            // Arrange
            var cityName = "xyzser";
            _mockRepo.Setup(p => p.GetCityByNameAsync(cityName)).ThrowsAsync(new Exception("City not found"));

            // Act
            var result = await _controller.GetCityByName(cityName);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode); // Status code 404 for NotFound
        }


    }
}
