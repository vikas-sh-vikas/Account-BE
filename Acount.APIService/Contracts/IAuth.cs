using Acount.APIService.DataAccess;
using Acount.APIService.Models;

namespace Acount.APIService.Contracts
{
	public interface IAuth
	{
		Task<AuthenticationDTO> Login(AuthenticationVO argsData, PGDBContext context = null);
		Task<UserModelDTO> SignUp(UserModelVO argsData, PGDBContext context = null);
	}

}
