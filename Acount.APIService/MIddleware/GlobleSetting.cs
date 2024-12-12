using Acount.APIService.Common;

namespace Acount.APIService.MIddleware
{
	public class GlobalSettings
	{
		public static IConfiguration Configuration;
		public static bool RequireHttpsMetadata { get; set; }
		public static bool ValidateAudience { get; set; }
		public static bool ValidateIssuer { get; set; }
		public static string ValidIssuers { get; set; }

		public GlobalSettings()
		{
			JWTGlobal.ServerUrl = Configuration["AppSettings:ServerURL"];
			JWTGlobal.Issuer = Configuration["AppSettings:Issuer"];
			JWTGlobal.Audience = Configuration["AppSettings:Audience"] ?? string.Empty;
			JWTGlobal.SessionTimeout = Convert.ToInt32(Configuration["AppSettings:SessionTimeout"]);
			JWTGlobal.AuthenticationProviderKey = Configuration["AppSettings:AuthenticationProviderKey"];
			JWTGlobal.ApiResourceName = Configuration["AppSettings:ApiResourceName"];

			ValidateAudience = Convert.ToBoolean(Configuration["AppSettings:ValidateAudience"]);
			ValidateIssuer = Convert.ToBoolean(Configuration["AppSettings:ValidateIssuer"]);
			RequireHttpsMetadata = Convert.ToBoolean(Configuration["AppSettings:RequireHttpsMetadata"]);
		}
	}
}
