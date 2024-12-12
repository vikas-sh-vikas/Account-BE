using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Acount.APIService.Utility;

namespace Acount.APIService.Common
{
	[AttributeUsage(AttributeTargets.Class)]
	public class AuthorizationAttribute : Attribute, IAuthorizationFilter
	{
		private readonly IConfiguration configuration;
		public AuthorizationAttribute(IConfiguration _configuration)
		{
			this.configuration = _configuration;
		}
		public void OnAuthorization(AuthorizationFilterContext filterContext)
		{
			try
			{
				var authHeader = filterContext.HttpContext.Request.Headers.Authorization;
				var requestApi = filterContext.HttpContext.Request.Path;
				var token = authHeader[0].Replace("Bearer ", "");
				var handler = new JwtSecurityTokenHandler();
				var jwtSecurityToken = handler.ReadJwtToken(token);
				if (jwtSecurityToken is JwtSecurityToken tokenS)
				{
					AuthorizationModel authModel = new()
					{
						//OrgId = Convert.ToInt32(tokenS.Claims.First(claim => claim.Type == "OrgId").Value),
						RoleId = 0,
						//UserId = Convert.ToInt64(Cryptography.GetDecryptedString(EnvVariables.GeneralEncKey, tokenS.Claims.First(claim => claim.Type == "UserIdentity").Value, 1)),
						UserName = Convert.ToString(tokenS.Claims.First(claim => claim.Type == "UserName").Value),
						//UserType = Convert.ToString(tokenS.Claims.First(claim => claim.Type == "UserType").Value),
						SourceType = Convert.ToString(tokenS.Claims.First(claim => claim.Type == "SourceType").Value),
						ExpirationTime = Convert.ToInt64(tokenS.Claims.First(claim => claim.Type == "exp").Value),
						NotBeforeTime = Convert.ToInt64(tokenS.Claims.First(claim => claim.Type == "nbf").Value),
						AuthTime = Convert.ToInt64(tokenS.Claims.First(claim => claim.Type == "auth_time").Value),
						//AppId = Convert.ToString(tokenS.Claims.First(claim => claim.Type == "AppId").Value),
						//LanguageUniqueId = Convert.ToString(tokenS.Claims.First(claim => claim.Type == "LanguageUniqueId").Value),
						//LanguageUniqueId = "hi_2",
						AuthToken = authHeader
					};

					//Validate token for specific source and App
					//if (authModel.SourceType == RequestSource.Mobile.ToString() || authModel.SourceType == RequestSource.Gateway.ToString() || authModel.AppId != Constants.ProjectId)
					//{
					//	//Check excludedApi list to validate if not in list then thorw error.
					//	bool contains = EnvVariables.ExcludedAPIListToValidate.Contains(requestApi);
					//	if (contains == false)
					//	{
					//		SystemLogger.LogData<AuthorizationAttribute>().LogInformation("Invalid Token.");
					//		throw new Exception("Invalid Token.");
					//	}
					//}

					//Set Usersession
					AuthSession authSession = new(authModel);

					//Set Token value dynamically
					if (filterContext.HttpContext.Request.ContentType == "application/json")
					{
						string originalContent;
						using (StreamReader stream = new StreamReader(filterContext.HttpContext.Request.Body))
						{
							originalContent = stream.ReadToEnd();
						}
						var dataSourceData = JsonConvert.DeserializeObject(originalContent);
						JObject dataSource = dataSourceData as JObject;
						if (dataSource.ContainsKey("Session"))
							dataSource["Session"] = JValue.FromObject(authModel);
						else
							dataSource.Add("Session", JValue.FromObject(authModel));
						string json = JsonConvert.SerializeObject(dataSource);
						var requestData = Encoding.UTF8.GetBytes(json);
						filterContext.HttpContext.Request.Body = new MemoryStream(requestData);
						filterContext.HttpContext.Request.ContentLength = filterContext.HttpContext.Request.Body.Length;
					}
				}
				else
				{
					throw new Exception("Invalid Token.");
				}
			}
			catch (Exception ex)
			{
				string err = ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString();
				SystemLogger.LogData<AuthorizationAttribute>().LogError(ex, err);
				throw;
			}
		}

	}
	public class AuthSession
	{
		public static AuthorizationModel UserSession { get; set; }

		public AuthSession(AuthorizationModel authModel)
		{
			UserSession = authModel;
		}
	}

}
