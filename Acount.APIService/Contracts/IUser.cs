using Acount.APIService.DataAccess;
using Acount.APIService.Models;

namespace Acount.APIService.Contracts
{
	public interface IUserManagement
	{
		Task<UserModelDTO> GetUserList(CommonRequestVO requestModel, PGDBContext context = null);
		Task<UserModelDTO> AddEditUser(UserModelVO argsData, PGDBContext context = null);
		//Task<AuthenticationDTO> Login(AuthenticationVO argsData, PGDBContext context = null);
		Task<UserModelDTO> GetSpecificUserMasterData(UserModelVO argsData, PGDBContext context = null);


	}

}
