using Acount.APIService.Common;
using Acount.APIService.DataAccess;
using Acount.APIService.Models;
using Acount.APIService.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Acount.APIService.Services
{
	internal class AuthService
	{

		public static string GenerateToken(UserModel args, RequestSource requestSource)
		{
			TimeSpan tokenLifetime;
			if (requestSource == RequestSource.Gateway)
			{
				tokenLifetime = TimeSpan.FromDays(JWTGlobal.TokenLifeTimeGateway);
			}
			else
			{
				tokenLifetime = TimeSpan.FromHours(JWTGlobal.SessionTimeout);
			}

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTGlobal.Key));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new List<Claim> {
			new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new (JwtRegisteredClaimNames.Sub, args.Username.ToString()),
			new (JwtRegisteredClaimNames.Email, args.EmailId),
			new (JwtRegisteredClaimNames.AuthTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer),
			new("UserName",args.DisplayName),
			//new("OrgId",args.OrgId.ToString()),
			//new("UserType",args.UserType.ToString()),
			new("SourceType",requestSource.ToString()),
			new("UserIdentity", Cryptography.GetEncryptedString(EnvVariables.GeneralEncKey, args.Id.ToString(), 1)),
			//new("AppId",Constants.ProjectId),
			//new("LanguageUniqueId",args.DefLanguageUniqueId),
			new("scope",requestSource.ToString()),            
            //new("Contact",args.MobileNo),
            };

			var tokenDesc = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.Add(tokenLifetime),
				Issuer = JWTGlobal.Issuer,
				Audience = JWTGlobal.Audience,
				SigningCredentials = credentials,
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDesc);

			var sToken = tokenHandler.WriteToken(token);
			return sToken;
		}
		
		internal static async Task<AuthenticationDTO> Login(AuthenticationVO data, PGDBContext context = null)
		{
			// Declare and initialize return model
			AuthenticationDTO userModelDTO = new()
			{
				DataResponse = Utils.GetProcessDefaultResponse(CommonUtility.ConvertEnumToString(ResultCodes.R_NOT_FOUND.ToString()), ResultCodes.R_NO_DATA_FOUND)
			};

			try
			{
				UserMasterEntity userMasterEntity = await context.userMasterEntity
					.Where(x => x.IsDeleted == 0 && x.EmailId.ToLower() == data.UserName.ToLower()).FirstOrDefaultAsync();

                data.Password = Cryptography.GetEncryptedString(EnvVariables.GeneralEncKey, data.Password, 1);

                if (userMasterEntity == null)
				{
					userModelDTO.DataResponse = Utils.GetProcessDefaultResponse("User not found", ResultCodes.R_INVALID_LOGIN_ID);
					return userModelDTO;
				}

				if (!userMasterEntity.EmailId.Equals(data.UserName, StringComparison.OrdinalIgnoreCase))
				{
					userModelDTO.DataResponse.ReturnCode = ResultCodes.R_INVALID_REQUEST;
					userModelDTO.DataResponse.Description = "Enter valid email ID";
					return userModelDTO;
				}
				else if (!userMasterEntity.Password.Equals(data.Password, StringComparison.OrdinalIgnoreCase))
				{
					userModelDTO.DataResponse.ReturnCode = ResultCodes.R_INVALID_REQUEST;
					userModelDTO.DataResponse.Description = "Enter valid password";
					return userModelDTO;
				}

				else
				{
					userModelDTO.DataResponse.ReturnCode = ResultCodes.R_SUCCESS;
					userModelDTO.DataResponse.Description = "Log In Success";
					//userMasterEntity.DefLanguageUniqueId = "en";
					UserModel user = new()
					{
						Id = userMasterEntity.Id,
						DisplayName = userMasterEntity.DisplayName,
						EmailId = userMasterEntity.EmailId,
						MobileNo = userMasterEntity.MobileNo,
						Username = userMasterEntity.UserName,
						DefLanguageUniqueId = "en"

					};
					string token = GenerateToken(user, data.requestSource);
					userModelDTO.AuthToken = token;
				}
			}
			catch (Exception ex)
			{
				// Log errors
				SystemLogger.LogData<UserService>().LogError(ex, ex.Message);
				userModelDTO.DataResponse.ReturnCode = ResultCodes.R_SERVICE_ERROR;
				userModelDTO.DataResponse.Description = ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString();
			}
			finally
			{
				context?.Dispose();
			}

			return userModelDTO;
		}
		
		internal static async Task<UserModelDTO> SignUp(UserModelVO data, PGDBContext context = null)
		{
            // Declare and initialize return model
            UserModelDTO userModelDTO = new()
			{
				DataResponse = Utils.GetProcessDefaultResponse(CommonUtility.ConvertEnumToString(ResultCodes.R_NOT_FOUND.ToString()), ResultCodes.R_NO_DATA_FOUND)
			};

			try
			{
                using (var transaction = await context.Database.BeginTransactionAsync())
				{

					userModelDTO = await UserService.AddUserMaster(data, context);

					if(userModelDTO.DataResponse.ReturnCode != ResultCodes.R_SUCCESS)
					{
						await transaction.RollbackAsync();
					}
					else if(userModelDTO.DataResponse.ReturnCode == ResultCodes.R_SUCCESS)
					{
                        await transaction.CommitAsync();
						return userModelDTO;
                    }

                }

            }
			catch (Exception ex)
			{
				// Log errors
				SystemLogger.LogData<UserService>().LogError(ex, ex.Message);
				userModelDTO.DataResponse.ReturnCode = ResultCodes.R_SERVICE_ERROR;
				userModelDTO.DataResponse.Description = ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString();
			}
			finally
			{
				context?.Dispose();
			}

			return userModelDTO;
		}


	}

}