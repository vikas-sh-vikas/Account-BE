using Acount.APIService.Common;
using Acount.APIService.Contracts;
using Acount.APIService.DataAccess;
using Acount.APIService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Acount.APIService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[TypeFilter(typeof(AuthorizationAttribute))]

	public class UserController : ControllerBase
	{
		IUserManagement userManagementService;
		private readonly PGDBContext dbContext;

		public UserController(IUserManagement user, PGDBContext context)
		{
			userManagementService = user;
			dbContext = context;
		}
		[HttpPost("AddEditUser")]
		//[Authorize]
		public async Task<UserModelDTO> AddEditUser(UserModelVO userMasterVO)
		{
			try
			{
				//Get Arguments along with the HTTP Request
				//UserMasterVO argsData = (UserMasterVO)RequestGenerator.GetHTTPRequest(HttpContext, userMasterVO);
				return await userManagementService.AddEditUser(userMasterVO, dbContext);
			}
			catch (System.Exception ex)
			{
				SystemLogger.LogData<UserController>().LogError(ex.ToString());
				UserModelDTO errResponse = Utils.GetProcessDefaultResponse<UserModelDTO>(ex.Message);
				return errResponse;
			}
		}

		[HttpPost("GetUserList")]
		//[Authorize]
		public async Task<UserModelDTO> GetUserList(CommonRequestVO commonRequestVO)
		{
			try
			{
				return await userManagementService.GetUserList(commonRequestVO, dbContext);
			}
			catch (System.Exception ex)
			{
				SystemLogger.LogData<UserController>().LogError(ex.ToString());
				UserModelDTO errResponse = Utils.GetProcessDefaultResponse<UserModelDTO>(ex.Message);
				return errResponse;
			}
		}

		[HttpPost("GetSpecificUserMasterData")]
		//[Authorize]
		public async Task<UserModelDTO> GetSpecificUserMasterData(UserModelVO userMasterVO)
		{
			try
			{
				return await userManagementService.GetSpecificUserMasterData(userMasterVO, dbContext);
			}
			catch (System.Exception ex)
			{
				SystemLogger.LogData<UserController>().LogError(ex.ToString());
				UserModelDTO errResponse = Utils.GetProcessDefaultResponse<UserModelDTO>(ex.Message);
				return errResponse;
			}
		}
	}
}
