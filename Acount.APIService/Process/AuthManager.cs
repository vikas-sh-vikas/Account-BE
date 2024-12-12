using Acount.APIService.Contracts;
using Acount.APIService.DataAccess;
using Acount.APIService.DataValidation;
using Acount.APIService.DataValidationProcess;
using Acount.APIService.Models;
using Acount.APIService.Services;

namespace Acount.APIService.Process
{
	public partial class ProcessManager : IAuth
	{


		public Task<AuthenticationDTO> Login(AuthenticationVO argsData, PGDBContext context = null)
		{
			return ProcessRequestasync(AuthValidation.LoginValidation, AuthService.Login, argsData, context);
		}
		
		public Task<UserModelDTO> SignUp(UserModelVO argsData, PGDBContext context = null)
		{
			return ProcessRequestasync(ServiceDataValidation.DefaultValidationResponse, AuthService.SignUp, argsData, context);
		}

	}

}
