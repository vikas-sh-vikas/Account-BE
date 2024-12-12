namespace Acount.APIService.Common
{
	public class EnvVariables
	{
		public static string GeneralEncKey { get { return "0123456789ZYXWVUTSRQPONMLKJIHGFEDCBA"; } }
		public static int SessionDuration { get; set; }
		public static string BaseAPIUrl { get; set; }
		public static string NotificationUrl { get; set; }
		public static int OTPInterval { get; set; }
		public static string Notificationuser { get; set; }
		public static string Notificationpwd { get; set; }
		public static string FrontEndUrl { get; set; }
		public static string FolderName { get; set; }
		public static string UtcOffset { get; set; }
		public static string DocumentPath { get; set; }
		public static string Timezone { get; set; }
		public static int DocumentSize { get; set; }
		public static string WebVersion { get; set; }
		public static string MobileVersion { get; set; }
		public static List<string> ExcludedAPIListToValidate { get; set; }
	}
	public class JWTGlobal
	{
		public static string Key { get { return "L!ne@ud!t2024@Henk!e"; } }
		public static string Issuer { get; set; }
		public static string Audience { get; set; }
		public static int SessionTimeout { get; set; }
		public static int TokenLifeTimeGateway { get; set; }
		public static string ServerUrl { get; set; }
		public static string AuthenticationProviderKey { get; set; }
		public static string ApiResourceName { get; set; }
	}
}
