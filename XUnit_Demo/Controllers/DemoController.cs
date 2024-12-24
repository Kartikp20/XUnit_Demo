using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Reflection;
using XUnit_Demo.Model;
using XUnit_Demo.Repository.Interface;

namespace XUnit_Demo.Controllers
{
	[Route("Demo")]
	[ApiController]
	public class DemoController : ControllerBase
	{
		private readonly IDemoAsyncRepo demo;

		public DemoController(IDemoAsyncRepo demo)
		{
			this.demo = demo;
		}

		[HttpPost("AddData")]
		public async Task<ActionResult> AddData(AddDemoModel model)
		{
			BaseResponseStatus responseStatus = new BaseResponseStatus();
			try
			{
				var result = await demo.AddData(model);
				if(result != null)
				{
					if(result.IsSuccess == true)
					{
						responseStatus.StatusCode = StatusCodes.Status200OK.ToString();
						responseStatus.StatusMessage = result.Msg;
						responseStatus.ResponseData = result;
					}
					else
					{
						responseStatus.StatusCode = StatusCodes.Status400BadRequest.ToString();
						responseStatus.StatusMessage = result.Msg;
						responseStatus.ResponseData = result;
					}
				}
				else
				{
					responseStatus.StatusCode = StatusCodes.Status404NotFound.ToString();
					responseStatus.StatusMessage = result.Msg;
					responseStatus.ResponseData = result;
				}

				return Ok(responseStatus);
			}
			catch (Exception ex)
			{
				var returnMsg = string.Format(ex.Message);
				responseStatus.StatusCode = StatusCodes.Status409Conflict.ToString();
				responseStatus.StatusMessage = returnMsg;
				return Ok(responseStatus);
			}
		}

		[HttpPost("EditData")]
		public async Task<ActionResult> EditData(AddDemoModel model)
		{
			BaseResponseStatus responseStatus = new BaseResponseStatus();
			try
			{
				var result = await demo.EditData(model);
				if (result != null)
				{
					if (result.IsSuccess == true)
					{
						responseStatus.StatusCode = StatusCodes.Status200OK.ToString();
						responseStatus.StatusMessage = result.Msg;
						responseStatus.ResponseData = result;
					}
					else
					{
						responseStatus.StatusCode = StatusCodes.Status400BadRequest.ToString();
						responseStatus.StatusMessage = result.Msg;
						responseStatus.ResponseData = result;
					}
				}
				else
				{
					responseStatus.StatusCode = StatusCodes.Status404NotFound.ToString();
					responseStatus.StatusMessage = result.Msg;
					responseStatus.ResponseData = result;
				}

				return Ok(responseStatus);
			}
			catch (Exception ex)
			{
				var returnMsg = string.Format(ex.Message);
				responseStatus.StatusCode = StatusCodes.Status409Conflict.ToString();
				responseStatus.StatusMessage = returnMsg;
				return Ok(responseStatus);
			}
		}

		[HttpGet("GetAllData")]
		public async Task<ActionResult> GetAllData()
		{
			BaseResponseStatus responseStatus = new BaseResponseStatus();
			try
			{
				var result = await demo.GetAllData();
				if (result != null)
				{
					responseStatus.StatusCode = StatusCodes.Status200OK.ToString();
					responseStatus.StatusMessage = "Data Get Successfully";
					responseStatus.ResponseData = result;
				}
				else
				{
					responseStatus.StatusCode = StatusCodes.Status404NotFound.ToString();
					responseStatus.StatusMessage = "Data Not Found";
					responseStatus.ResponseData = result;
				}

				return Ok(responseStatus);
			}
			catch (Exception ex)
			{
				var returnMsg = string.Format(ex.Message);
				responseStatus.StatusCode = StatusCodes.Status409Conflict.ToString();
				responseStatus.StatusMessage = returnMsg;
				return Ok(responseStatus);
			}
		}

		[HttpGet("GetData")]
		public async Task<ActionResult> GetData(int Id)
		{
			BaseResponseStatus responseStatus = new BaseResponseStatus();
			try
			{
				var result = await demo.GetData(Id);
				if (result != null)
				{
					responseStatus.StatusCode = StatusCodes.Status200OK.ToString();
					responseStatus.StatusMessage = "Data Get Successfully";
					responseStatus.ResponseData = result;
				}
				else
				{
					responseStatus.StatusCode = StatusCodes.Status404NotFound.ToString();
					responseStatus.StatusMessage = "Data Not Found";
					responseStatus.ResponseData = result;
				}

				return Ok(responseStatus);
			}
			catch (Exception ex)
			{
				var returnMsg = string.Format(ex.Message);
				responseStatus.StatusCode = StatusCodes.Status409Conflict.ToString();
				responseStatus.StatusMessage = returnMsg;
				return Ok(responseStatus);
			}
		}

		[HttpPost("DeleteData")]
		public async Task<ActionResult> DeleteData(int Id)
		{
			BaseResponseStatus responseStatus = new BaseResponseStatus();
			try
			{
				var result = await demo.DeleteData(Id);
				if (result != null)
				{
					if (result.IsSuccess == true)
					{
						responseStatus.StatusCode = StatusCodes.Status200OK.ToString();
						responseStatus.StatusMessage = result.Msg;
						responseStatus.ResponseData = result;
					}
					else
					{
						responseStatus.StatusCode = StatusCodes.Status400BadRequest.ToString();
						responseStatus.StatusMessage = result.Msg;
						responseStatus.ResponseData = result;
					}
				}
				else
				{
					responseStatus.StatusCode = StatusCodes.Status404NotFound.ToString();
					responseStatus.StatusMessage = result.Msg;
					responseStatus.ResponseData = result;
				}

				return Ok(responseStatus);
			}
			catch (Exception ex)
			{
				var returnMsg = string.Format(ex.Message);
				responseStatus.StatusCode = StatusCodes.Status409Conflict.ToString();
				responseStatus.StatusMessage = returnMsg;
				return Ok(responseStatus);
			}
		}

		[HttpGet("ReadExcelData")]
		public async Task<ActionResult> ReadExcelData(string filePath)
		{
			var res = await demo.ReadExcelData(filePath);
			return Ok(res);
		}
	}
}
