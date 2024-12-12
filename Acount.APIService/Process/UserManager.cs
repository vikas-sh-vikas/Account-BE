using Acount.APIService.Contracts;
using Acount.APIService.DataAccess;
using Acount.APIService.DataValidation;
using Acount.APIService.DataValidationProcess;
using Acount.APIService.Models;
using Acount.APIService.Services;

namespace Acount.APIService.Process
{
	public partial class ProcessManager : IUserManagement
	{

		public Task<UserModelDTO> GetUserList(CommonRequestVO commonRequestVO, PGDBContext context = null)
		{
			return ProcessRequestasync(ListDataValidation.ValidateGetInputRequest, UserService.GetUserList,commonRequestVO, context);
		}
		public Task<UserModelDTO> GetSpecificUserMasterData(UserModelVO argsData, PGDBContext context = null)
		{
			return ProcessRequestasync(ListDataValidation.DefaultValidationResponse, UserService.GetSpecificUserMasterData,argsData, context);
		}
		public Task<UserModelDTO> AddEditUser(UserModelVO argsData, PGDBContext context = null)
		{
			return ProcessRequestasync(ListDataValidation.DefaultValidationResponse, UserService.AddEditUser,argsData, context);
		}

	}

}
