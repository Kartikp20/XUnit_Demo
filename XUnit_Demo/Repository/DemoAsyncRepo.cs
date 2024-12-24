using Dapper;
using OfficeOpenXml;
using System.Data.Common;
using XUnit_Demo.Model;
using XUnit_Demo.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace XUnit_Demo.Repository
{
	public class DemoAsyncRepo : BaseAsyncRepository, IDemoAsyncRepo
	{
		public DemoAsyncRepo(IConfiguration configuration) : base(configuration)
		{
		}

		public async Task<ResponseModel> AddData(AddDemoModel model)
		{
			ResponseModel response = new ResponseModel();
			using (DbConnection dbConnection = sqlwriterConnection)
			{
				var result = await dbConnection.QuerySingleOrDefaultAsync<int>(@"INSERT INTO tblDemo (Name, MobileNo, EmailId, UserName, Password, 
																																	  CreatedBy, CreatedDate, IsDeleted) 
																																	  VALUES (@Name, @MobileNo, @EmailId, @UserName, @Password, @CreatedBy, getdate(), 0);
																																	  SELECT SCOPE_IDENTITY();", model);

				if(result > 0)
				{
					response.Id = result;
					response.IsSuccess = true;
					response.Msg = "Inserted";
				}
				else
				{
					response.Id = 0;
					response.IsSuccess = false;
					response.Msg = "Error";
				}

				return response;
			}
		}

		public async Task<ResponseModel> DeleteData(int Id)
		{
			ResponseModel response = new ResponseModel();
			using(DbConnection dbConnection = sqlwriterConnection)
			{
				var result = await dbConnection.ExecuteAsync(@"UPDATE tblDemo SET IsDeleted = 0 WHERE Id = @Id", new { Id = Id });
				if(result == 1)
				{
					response.Id = result;
					response.IsSuccess = true;
					response.Msg = "Data deleted successfully";
				}
				else
				{
					response.Id = 0;
					response.IsSuccess = false;
					response.Msg = "Error";
				}

				return response;
			}
		}

		public async Task<ResponseModel> EditData(AddDemoModel model)
		{
			ResponseModel response = new ResponseModel();
			using (DbConnection dbConnection = sqlwriterConnection)
			{
				var result = await dbConnection.ExecuteAsync(@"UPDATE tblDemo
																											SET 
																												Name = @Name,
																												MobileNo = @MobileNo,
																												EmailId = @EmailId,
																												UserName = @UserName,
																												Password = @Password,
																												ModifiedBy = @CreatedBy,
																												ModifiedDate = GETDATE()
																											WHERE 
																												Id = @Id;", model);

				if (result > 0)
				{
					response.Id = result;
					response.IsSuccess = true;
					response.Msg = "Updated";
				}
				else
				{
					response.Id = 0;
					response.IsSuccess = false;
					response.Msg = "Error";
				}

				return response;
			}
		}

		public async Task<List<DemoModel>> GetAllData()
		{
			List<DemoModel> demoModels = new List<DemoModel>();
			using(DbConnection dbConnection = sqlwriterConnection)
			{
				var result = await dbConnection.QueryAsync<DemoModel>(@"SELECT * FROM tblDemo WHERE IsDeleted = 0");
				demoModels = result.ToList();
			}
			return demoModels;
		}

		public async Task<DemoModel> GetData(int Id)
		{
			DemoModel demoModels = new DemoModel();
			using (DbConnection dbConnection = sqlwriterConnection)
			{
				var result = await dbConnection.QueryAsync<DemoModel>(@"SELECT * FROM tblDemo WHERE IsDeleted = 0 AND Id = @Id", new { Id = Id });
				demoModels = result.FirstOrDefault();
			}
			return demoModels;
		}

		public async Task<List<UserRecord>> ReadExcelData(string filePath)
		{
			List<UserRecord> userRecords = new List<UserRecord>();
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Required for EPPlus

			using (var package = new ExcelPackage(new FileInfo(filePath)))
			{
				var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet
				var rowCount = worksheet.Dimension.Rows;

				for (int row = 2; row <= rowCount; row++) // Skip the header row
				{
					var user = new UserRecord
					{
						Id = int.TryParse(worksheet.Cells[row, 1].Text, out var id) ? id : 0,
						Name = worksheet.Cells[row, 2].Text,
						MobileNo = worksheet.Cells[row, 3].Text,
						EmailId = worksheet.Cells[row, 4].Text,
						UserName = worksheet.Cells[row, 5].Text,
						Password = worksheet.Cells[row, 6].Text,
						CreatedBy = worksheet.Cells[row, 7].Text,
						CreatedDate = DateTime.TryParse(worksheet.Cells[row, 8].Text, out var createdDate) ? createdDate : DateTime.MinValue,
						IsDeleted = bool.TryParse(worksheet.Cells[row, 9].Text, out var isDeleted) && isDeleted
					};

					userRecords.Add(user);
				}
			}

			return userRecords;
		}
	}
}
