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
	//[TypeFilter(typeof(AuthorizationAttribute))]

	public class AuthController : ControllerBase
	{
		IAuth userManagementService;
		private readonly PGDBContext dbContext;

		public AuthController(IAuth user, PGDBContext context)
		{
			userManagementService = user;
			dbContext = context;
		}
		[HttpPost("Login")]
		//[Authorize]
		public async Task<AuthenticationDTO> Login(AuthenticationVO authenticationVO)
		{
			try
			{
				//Get Arguments along with the HTTP Request
				//UserMasterVO argsData = (UserMasterVO)RequestGenerator.GetHTTPRequest(HttpContext, userMasterVO);
				authenticationVO.requestSource = RequestSource.Web;
				return await userManagementService.Login(authenticationVO, dbContext);
			}
			catch (System.Exception ex)
			{
				SystemLogger.LogData<AuthController>().LogError(ex.ToString());
				AuthenticationDTO errResponse = Utils.GetProcessDefaultResponse<AuthenticationDTO>(ex.Message);
				return errResponse;
			}
		}
		[HttpPost("SignUp")]
		//[Authorize]
		public async Task<UserModelDTO> SignUp(UserModelVO authenticationVO)
		{
			try
			{
				authenticationVO.requestSource = RequestSource.Web;
				return await userManagementService.SignUp(authenticationVO, dbContext);
			}
			catch (System.Exception ex)
			{
				SystemLogger.LogData<AuthController>().LogError(ex.ToString());
                UserModelDTO errResponse = Utils.GetProcessDefaultResponse<UserModelDTO>(ex.Message);
				return errResponse;
			}
		}


	}
}
