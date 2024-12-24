using XUnit_Demo.Model;

namespace XUnit_Demo.Repository.Interface
{
	public interface IDemoAsyncRepo
	{
		Task<ResponseModel> AddData(AddDemoModel model);

		Task<ResponseModel> EditData(AddDemoModel model);

		Task<List<DemoModel>> GetAllData();

		Task<DemoModel> GetData(int Id);

		Task<ResponseModel> DeleteData(int Id);

		Task<List<UserRecord>> ReadExcelData(string filePath);
	}
}
