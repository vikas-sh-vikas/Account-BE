namespace Acount.APIService.Common
{
	public class AuthorizationModel
	{
		public Int64 UserId { get; set; }
		public string UserName { get; set; }
		public int OrgId { get; set; }
		public int MasterUser { get; set; }
		public DateTime SessionTime { get; set; }
		public string AuthToken { get; set; }
		public int RoleId { get; set; }
		public int LanguageId { get; set; }
		public int LangAbbreviation { get; set; }
		public Int64 TenantId { get; set; }
		public Int64 ExpirationTime { get; set; }
		public Int64 NotBeforeTime { get; set; }
		public Int64 AuthTime { get; set; }
		public string UserType { get; set; }
		public string SourceType { get; set; }
		public string AppId { get; set; }
		public string LanguageUniqueId { get; set; }
	}

}
