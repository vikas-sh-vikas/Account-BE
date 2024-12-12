
namespace Acount.APIService.Models
{

	public class UserModelDTO : BaseResponseModel
	{
		public CommonRequestModel FilterModel { get; set; }
	}
	public class UserModelVO : RequestModel
	{
		public UserModel Data { get; set; }
	}
	public class UserModel
	{
		public long Id { get; set; } // Matches bigint
		public string Username { get; set; } = string.Empty; // Matches character varying(500)
		public string DisplayName { get; set; } = string.Empty; // Matches character varying(500)
		public string EmailId { get; set; } = string.Empty; // Matches character varying(500)
		public string MobileNo { get; set; } = string.Empty; // Matches character varying(100)
		public string Password { get; set; } = string.Empty; // Matches character varying(200)
		public string DefLanguageUniqueId { get; set; }



	}
}