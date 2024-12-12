
namespace Acount.APIService.Models
{

	public class AuthenticationVO : RequestModel
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public int LoginMode { get; set; }
		/*public RequestSource requestSource { get; set; }*/
	}

	public class AuthenticationDTO : BaseResponseModel
	{
		public string AuthToken { get; set; }
		public string ExpiresAt { get; set; }
	}
}