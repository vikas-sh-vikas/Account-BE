using Acount.APIService.Models;

namespace Acount.APIService.Common
{
	public class Utils
	{
		public static ResponseModel GetProcessDefaultResponse(string Message, ResultCodes resultCode)
		{
			ResponseModel dto = new()
			{
				ReturnCode = resultCode,
				ResponseDateTime = System.DateTime.Now,
				Description = Message
			};
			return dto;
		}
		public static T GetProcessDefaultResponse<T>(string Message)
		{
			ResponseModel dto = new()
			{
				ReturnCode = ResultCodes.R_INTERNAL_SERVER_ERROR,
				ResponseDateTime = System.DateTime.Now,
				Description = Message
			};
			var objType = CommonUtility.ConvertFromDynamicObject<T>(null);
			objType?.GetType().GetProperty("DataResponse").SetValue(objType, dto);
			return objType;
		}

		public static T GetProcessDefaultResponse<T>(string Message, ResultCodes resultCode)
		{
			ResponseModel dto = new()
			{
				ReturnCode = resultCode,
				ResponseDateTime = System.DateTime.Now,
				Description = Message
			};
			var objType = CommonUtility.ConvertFromDynamicObject<T>(null);
			objType?.GetType().GetProperty("DataResponse").SetValue(objType, dto);
			return objType;
		}
	}

}
