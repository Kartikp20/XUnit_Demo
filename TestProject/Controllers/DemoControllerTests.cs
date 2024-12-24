using Moq;
using System;
using System.Threading.Tasks;
using XUnit_Demo.Controllers;
using XUnit_Demo.Repository.Interface;
using Xunit;
using XUnit_Demo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text;
using XUnit_Demo.Repository;
using Microsoft.Extensions.Configuration;

namespace TestProject.Controllers
{
	public class DemoControllerTests
	{
		private readonly Mock<IDemoAsyncRepo> _mockDemoService;
		private readonly DemoController _controller;
		private readonly IConfiguration _configuration;

		public DemoControllerTests()
		{
			_mockDemoService = new Mock<IDemoAsyncRepo>();
			var inMemorySettings = new Dictionary<string, string>{{ "ExcelFilePath", @"C:\\Users\\prasadp\\Desktop\\Demo Scheduler\\XUnit_Demo\\Book.xlsx" }};
			_configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
			_controller = new DemoController(_mockDemoService.Object);
		}

		[Fact]
		public async Task AddData_ShouldReturnOk_WhenResultIsSuccess_FromExcel()
		{
			// Arrange
			var demoAsync = new DemoAsyncRepo(_configuration);

			// Retrieve Excel file path from configuration
			var excelFilePath = _configuration["ExcelFilePath"];
			var records = await demoAsync.ReadExcelData(excelFilePath);

			foreach (var record in records)
			{
				var addDemoModel = new AddDemoModel
				{
					Name = record.Name,
					MobileNo = record.MobileNo,
					EmailId = record.EmailId,
					UserName = record.UserName,
					Password = record.Password,
					CreatedBy = int.Parse(record.CreatedBy)
				};

				var expectedResponse = new ResponseModel
				{
					IsSuccess = true,
					Msg = "Data added successfully"
				};

				_mockDemoService.Setup(service => service.AddData(addDemoModel)).ReturnsAsync(expectedResponse);

				// Act
				var result = await _controller.AddData(addDemoModel) as OkObjectResult;

				// Assert
				result.Should().NotBeNull();
				result.StatusCode.Should().Be(StatusCodes.Status200OK);

				var responseStatus = result.Value as BaseResponseStatus;
				responseStatus.Should().NotBeNull();
				responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK.ToString());
				responseStatus.StatusMessage.Should().Be(expectedResponse.Msg);
				responseStatus.ResponseData.Should().BeEquivalentTo(expectedResponse);
			}
		}

		[Fact]
		public async Task AddData_ShouldReturnOk_WhenResultIsSuccess()
		{
			// Arrange
			var addDemoModel = new AddDemoModel
			{
				Name = "Test_demo",
				MobileNo = "7896541230",
				EmailId = "test_demo@mail.com",
				UserName = "test_demo@mail.com",
				Password = "12345",
				CreatedBy = 1020
			};

			var expectedResponse = new ResponseModel
			{
				IsSuccess = true,
				Msg = "Data added successfully"
			};

			_mockDemoService.Setup(service => service.AddData(addDemoModel)).ReturnsAsync(expectedResponse);

			// Act
			var result = await _controller.AddData(addDemoModel) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK.ToString());
			responseStatus.StatusMessage.Should().Be(expectedResponse.Msg);
			responseStatus.ResponseData.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task AddData_ShouldReturnConflict_WhenExceptionIsThrown()
		{
			// Arrange
			var addDemoModel = new AddDemoModel
			{
				Name = "Test_demo",
				MobileNo = "7896541230",
				EmailId = "test_demo@mail.com",
				UserName = "test_demo@mail.com",
				Password = "12345",
				CreatedBy = 1020
			};

			_mockDemoService.Setup(service => service.AddData(addDemoModel)).ThrowsAsync(new Exception("Unexpected error occurred"));

			// Act
			var result = await _controller.AddData(addDemoModel) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status409Conflict.ToString());
			responseStatus.StatusMessage.Should().Be("Unexpected error occurred");
		}


		[Fact]
		public async Task EditData_ShouldReturnOk_WhenResultIsSuccess()
		{
			// Arrange
			var editDemoModel = new AddDemoModel
			{
				Name = "Test_demo",
				MobileNo = "7896541230",
				EmailId = "test_demo@mail.com",
				UserName = "test_demo@mail.com",
				Password = "12345",
				CreatedBy = 1020
			};

			var expectedResponse = new ResponseModel
			{
				IsSuccess = true,
				Msg = "Data edited successfully"
			};

			_mockDemoService.Setup(service => service.EditData(editDemoModel)).ReturnsAsync(expectedResponse);

			// Act
			var result = await _controller.EditData(editDemoModel) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK.ToString());
			responseStatus.StatusMessage.Should().Be(expectedResponse.Msg);
			responseStatus.ResponseData.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task EditData_ShouldReturnConflict_WhenExceptionIsThrown()
		{
			// Arrange
			var addDemoModel = new AddDemoModel
			{
				Name = "Test_demo",
				MobileNo = "7896541230",
				EmailId = "test_demo@mail.com",
				UserName = "test_demo@mail.com",
				Password = "12345",
				CreatedBy = 1020
			};

			_mockDemoService.Setup(service => service.EditData(addDemoModel)).ThrowsAsync(new Exception("Unexpected error occurred"));

			// Act
			var result = await _controller.EditData(addDemoModel) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status409Conflict.ToString());
			responseStatus.StatusMessage.Should().Be("Unexpected error occurred");
		}


		[Fact]
		public async Task GetAll_ShouldReturnOk_WhenDataExists()
		{
			// Arrange
			var expectedData = new List<DemoModel>
			{
				new DemoModel { Id = 1, Name = "Test1", MobileNo = "1234567890", EmailId = "test1@mail.com" },
				new DemoModel { Id = 2, Name = "Test2", MobileNo = "0987654321", EmailId = "test2@mail.com" }
			};

			_mockDemoService.Setup(service => service.GetAllData()).ReturnsAsync(expectedData);

			// Act
			var result = await _controller.GetAllData() as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK.ToString());
			responseStatus.StatusMessage.Should().Be("Data Get Successfully");
			responseStatus.ResponseData.Should().BeEquivalentTo(expectedData);
		}

		[Fact]
		public async Task GetAll_ShouldReturnNotFound_WhenNoDataExists()
		{
			// Arrange
			var expectedResponse = new BaseResponseStatus
			{
				StatusCode = StatusCodes.Status404NotFound.ToString(),
				StatusMessage = "Data Not Found",
				ResponseData = null
			};

			_mockDemoService.Setup(service => service.GetAllData()).ReturnsAsync((List<DemoModel>)null);

			// Act
			var result = await _controller.GetAllData() as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status404NotFound.ToString());
			responseStatus.StatusMessage.Should().Be("Data Not Found");
			responseStatus.ResponseData.Should().BeNull();
		}

		[Fact]
		public async Task GetAll_ShouldReturnConflict_WhenExceptionIsThrown()
		{
			// Arrange
			_mockDemoService.Setup(service => service.GetAllData()).ThrowsAsync(new Exception("Unexpected error occurred"));

			// Act
			var result = await _controller.GetAllData() as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status409Conflict.ToString());
			responseStatus.StatusMessage.Should().Be("Unexpected error occurred");
		}


		[Fact]
		public async Task GetById_ShouldReturnOk_WhenDataExists()
		{
			// Arrange
			int Id = 1;
			var expectedData = new DemoModel
			{
				Name = "Test_demo",
				MobileNo = "7896541230",
				EmailId = "test_demo@mail.com",
				UserName = "test_demo@mail.com",
				Password = "12345"
			};
			_mockDemoService.Setup(service => service.GetData(Id)).ReturnsAsync(expectedData);

			// Act
			var result = await _controller.GetData(Id) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK.ToString());
			responseStatus.StatusMessage.Should().Be("Data Get Successfully");
			responseStatus.ResponseData.Should().BeEquivalentTo(expectedData);
		}

		[Fact]
		public async Task GetById_ShouldReturnNotFound_WhenNoDataExists()
		{
			// Arrange
			int id = 2;
			var expectedResponse = new BaseResponseStatus
			{
				StatusCode = StatusCodes.Status404NotFound.ToString(),
				StatusMessage = "Data Not Found",
				ResponseData = null
			};
			_mockDemoService.Setup(service => service.GetData(id)).ReturnsAsync((DemoModel)null);

			// Act
			var result = await _controller.GetData(id) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(expectedResponse.StatusCode);
			responseStatus.StatusMessage.Should().Be(expectedResponse.StatusMessage);
			responseStatus.ResponseData.Should().BeNull();
		}

		[Fact]
		public async Task GetById_ShouldReturnConflict_WhenExceptionIsThrown()
		{
			// Arrange
			int Id = 2;
			_mockDemoService.Setup(service => service.GetData(Id)).ThrowsAsync(new Exception("Unexpected error occurred"));

			// Act
			var result = await _controller.GetData(Id) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status409Conflict.ToString());
			responseStatus.StatusMessage.Should().Be("Unexpected error occurred");
		}

		[Fact]
		public async Task DeleteData_ShouldReturnOk_WhenResultIsSuccess()
		{
			// Arrange
			int Id = 1;

			var expectedResponse = new ResponseModel
			{
				IsSuccess = true,
				Msg = "Data deleted successfully"
			};

			_mockDemoService.Setup(service => service.DeleteData(Id)).ReturnsAsync(expectedResponse);

			// Act
			var result = await _controller.DeleteData(Id) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status200OK.ToString());
			responseStatus.StatusMessage.Should().Be(expectedResponse.Msg);
			responseStatus.ResponseData.Should().BeEquivalentTo(expectedResponse);
		}

		[Fact]
		public async Task DeleteData_ShouldReturnConflict_WhenExceptionIsThrown()
		{
			// Arrange
			int Id = 2;

			_mockDemoService.Setup(service => service.DeleteData(Id)).ThrowsAsync(new Exception("Unexpected error occurred"));

			// Act
			var result = await _controller.DeleteData(Id) as OkObjectResult;

			// Assert
			result.Should().NotBeNull();
			result.StatusCode.Should().Be(StatusCodes.Status200OK);

			var responseStatus = result.Value as BaseResponseStatus;
			responseStatus.Should().NotBeNull();
			responseStatus.StatusCode.Should().Be(StatusCodes.Status409Conflict.ToString());
			responseStatus.StatusMessage.Should().Be("Unexpected error occurred");
		}


	}
}
