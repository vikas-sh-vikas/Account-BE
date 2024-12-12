using Acount.APIService.Common;
using Acount.APIService.DataAccess;
using Acount.APIService.Models;

namespace Acount.APIService.DataValidation
{
	public class ListDataValidation
	{
		internal static ResponseModel ValidateGetInputRequest(CommonRequestVO arg, PGDBContext context = null)
		{
			ResponseModel resp = new()
			{
				ReturnCode = ResultCodes.R_SUCCESS,
				Description = CommonUtility.ConvertEnumToString(ResultCodes.R_SUCCESS.ToString()),
				ResponseDateTime = DateTime.Now
			};

			arg.Data.CurrentPage = arg.Data.CurrentPage == 0 ? 1 : arg.Data.CurrentPage;
			arg.Data.PageSize = arg.Data.PageSize == 0 ? 10 : arg.Data.PageSize;
			arg.Data.OrderBy = arg.Data.OrderBy == null ? "" : arg.Data.OrderBy;
			arg.Data.OrderType = arg.Data.OrderType == null ? "A" : arg.Data.OrderType;
			arg.Data.StartNo = arg.Data.EndNo = 0;

			//For Paging
			Int32 startRecFrom = (Convert.ToInt32(arg.Data.CurrentPage) == 0) ? 1 : Convert.ToInt32(arg.Data.CurrentPage);
			Int32 reqRecordCount = Convert.ToInt32(arg.Data.PageSize);
			//arg.data.actionId = arg.data.actionId == null ? 0 : arg.data.actionId;

			//Initialize paging 
			int temp = startRecFrom == 1 ? startRecFrom : (startRecFrom - 1) * reqRecordCount;
			arg.Data.StartNo = startRecFrom == 1 ? startRecFrom - 1 : temp;
			arg.Data.EndNo = reqRecordCount;

			return resp;
		}

		internal static ResponseModel DefaultValidationResponse(object obj, PGDBContext context = null)
		{
			//Create Default Response
			return new ResponseModel()
			{
				ReturnCode = ResultCodes.R_SUCCESS,
				Description = CommonUtility.ConvertEnumToString(ResultCodes.R_SUCCESS.ToString()),
				ResponseDateTime = DateTime.Now
			};
		}

	}
}
