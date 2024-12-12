using Acount.APIService.Common;
using Acount.APIService.DataAccess;
using Acount.APIService.Models;

namespace Acount.APIService.DataValidation
{
	internal class AuthValidation
	{
		internal static ResponseModel LoginValidation(AuthenticationVO arg, PGDBContext context = null)
		{
			if (string.IsNullOrEmpty(arg.UserName))
			{
				return Utils.GetProcessDefaultResponse("Username cann't be blank.", ResultCodes.R_FIELD_BLANK);
			}
			if (string.IsNullOrEmpty(arg.Password))
			{
				return Utils.GetProcessDefaultResponse("Password cann't be blank.", ResultCodes.R_FIELD_BLANK);
			}

			return Utils.GetProcessDefaultResponse(CommonUtility.ConvertEnumToString(ResultCodes.R_SUCCESS.ToString()), ResultCodes.R_SUCCESS);
		}
	}	
	
}
