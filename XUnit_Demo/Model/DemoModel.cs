using Microsoft.AspNetCore.Mvc;

namespace XUnit_Demo.Model
{
	public class DemoModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string MobileNo { get; set; }
		public string EmailId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class AddDemoModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string MobileNo { get; set; }
		public string EmailId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public int CreatedBy { get; set; }
	}

	public class UserRecord
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string MobileNo { get; set; }
		public string EmailId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsDeleted { get; set; }
	}


	public class ResponseModel
	{
		public long Id { get; set; }
		public bool IsSuccess { get; set; }
		public string Msg { get; set; }
	}

	public class BaseResponseStatus
	{
		public string StatusCode { get; set; }
		public string StatusMessage { get; set; }
		public object ResponseData { get; set; }

		public static implicit operator BaseResponseStatus(int v)
		{
			throw new NotImplementedException();
		}

		public static explicit operator BaseResponseStatus(ActionResult v)
		{
			throw new NotImplementedException();
		}
	}
}
