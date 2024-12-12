using Acount.APIService.Common;
using Acount.APIService.DataAccess;
using Acount.APIService.Models;
using Acount.APIService.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Acount.APIService.Services
{
	internal class UserService
	{

        private static async Task<string> GenerateRandomPassword()
        {
            return await Task.Run(() =>
            {
                const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
                const string specialChars = "@$!%*?&";
                const string numericChars = "0123456789";

                StringBuilder password = new StringBuilder();
                Random random = new Random();

                // Append 3 random uppercase letters
                for (int i = 0; i < 3; i++)
                {
                    password.Append(upperChars[random.Next(upperChars.Length)]);
                }

                // Append 3 random lowercase letters
                for (int i = 0; i < 3; i++)
                {
                    password.Append(lowerChars[random.Next(lowerChars.Length)]);
                }

                // Append a random special character
                password.Append(specialChars[random.Next(specialChars.Length)]);

                // Append 2 random numbers
                for (int i = 0; i < 3; i++)
                {
                    password.Append(numericChars[random.Next(numericChars.Length)]);
                }

                return password.ToString();
            });
        }
        internal static async Task<UserModelDTO> AddUserMaster(UserModelVO userModel, PGDBContext context)
		{
			//Declare and initialize return model
			UserModelDTO userModelDTO = new UserModelDTO();
			userModelDTO.DataResponse = Utils.GetProcessDefaultResponse(CommonUtility.ConvertEnumToString(ResultCodes.R_UNKNOWN.ToString()), ResultCodes.R_UNKNOWN);

			try
			{
				// Check if email or mobile number already exists
				bool isEmailExists = await context.userMasterEntity.AnyAsync(u => u.EmailId == userModel.Data.EmailId && u.IsDeleted == 0);
				bool isMobileExists = await context.userMasterEntity.AnyAsync(u => u.MobileNo == userModel.Data.MobileNo && u.IsDeleted == 0);

				if (isEmailExists && isMobileExists)
				{
					userModelDTO.DataResponse.ReturnCode = ResultCodes.R_DUPLICATE;
					userModelDTO.DataResponse.Description = "Email and mobile number already exists.";
					return userModelDTO;
				}
				else if (isEmailExists)
				{
					userModelDTO.DataResponse.ReturnCode = ResultCodes.R_DUPLICATE;
					userModelDTO.DataResponse.Description = "Email already exists.";
					return userModelDTO;
				}
				else if (isMobileExists)
				{
					userModelDTO.DataResponse.ReturnCode = ResultCodes.R_DUPLICATE;
					userModelDTO.DataResponse.Description = "Mobile already exists.";
					return userModelDTO;
				}
				else
				{
                    userModel.Data.Password = Cryptography.GetEncryptedString(EnvVariables.GeneralEncKey, await GenerateRandomPassword(), 1);

                    //Initialize User entity
                    UserMasterEntity userEntity = new UserMasterEntity();
					userEntity.UserName = userModel.Data.Username ?? "";
					userEntity.DisplayName = userModel.Data.DisplayName ?? "";
					userEntity.EmailId = userModel.Data.EmailId;
                    userEntity.MobileNo = userModel.Data.MobileNo;
					userEntity.Password = userModel.Data.Password;
					//userEntity.DefLanguageUniqueId = userModel.Session.LanguageUniqueId;
					context.Entry(userEntity).State = EntityState.Added;
					int recStatus = await context.SaveChangesAsync();
					//userModel.Data.Id = userEntity.Id;
					context.Entry(userEntity).State = EntityState.Detached;

					//Condition for check recStatus
					if (recStatus == 0)
					{
						return userModelDTO;
					}
                    MailFormatModel mailFormat = DomainConfigService.GetBasicMailFormat(context).Data;
                    List<string> ls = new List<string>();
                    ls.Add(userModel.Data.EmailId);
                    mailFormat.Receivers = ls;
                    mailFormat.Subject = "Welcome Email";
                    mailFormat.MessageText = EmailUtility.WelcomeMail();
                    mailFormat.MessageText = mailFormat.MessageText.Replace("@USERNAME@", userModel.Data.Username);
                    mailFormat.MessageText = mailFormat.MessageText.Replace("@EMAIL@", userModel.Data.EmailId);

                    userModel.Data.Password = Cryptography.GetDecryptedString(EnvVariables.GeneralEncKey, userModel.Data.Password, 1);
                    mailFormat.MessageText = mailFormat.MessageText.Replace("@PASSWORD@", userModel.Data.Password);
                    mailFormat.MessageText = mailFormat.MessageText.Replace("@ORIGIN@", EnvVariables.FrontEndUrl);

                    bool sendMailResult = EmailUtility.SendMail(mailFormat);
                    if (sendMailResult)
                    {
                        userModelDTO.DataResponse.ReturnCode = ResultCodes.R_SUCCESS;
                        userModelDTO.DataResponse.Description = "Mail send Success";
                        //userModelDTO.Data = userModel.Data;
                    }
					else
					{
						userModelDTO.DataResponse.ReturnCode = ResultCodes.R_SERVICE_ERROR;
						userModelDTO.DataResponse.Description = "Failed to send email.";
					}
				}
			}
			catch (Exception ex)
			{
				//Log errors
				SystemLogger.LogData<UserService>().LogError(ex, ex.Message);
				userModelDTO.DataResponse.ReturnCode = ResultCodes.R_SERVICE_ERROR;
				userModelDTO.DataResponse.Description = ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString();
			}
			return userModelDTO;
		}

		internal static async Task<UserModelDTO> EditUserMaster(UserModelVO userModel, PGDBContext context)
		{
			//Declare and initialize return model
			UserModelDTO userModelDTO = new UserModelDTO();
			userModelDTO.DataResponse = Utils.GetProcessDefaultResponse(CommonUtility.ConvertEnumToString(ResultCodes.R_UNKNOWN.ToString()), ResultCodes.R_UNKNOWN);

			try
			{
				// Check if email or mobile number already exists
				bool isEmailExists = await context.userMasterEntity.AnyAsync(u => u.EmailId == userModel.Data.EmailId && u.Id != userModel.Data.Id && u.IsDeleted == 0);
				bool isMobileExists = await context.userMasterEntity.AnyAsync(u => u.MobileNo == userModel.Data.MobileNo && u.Id != userModel.Data.Id && u.IsDeleted == 0);

				if (isEmailExists && isMobileExists)
				{
					userModelDTO.DataResponse.ReturnCode = ResultCodes.R_DUPLICATE;
					userModelDTO.DataResponse.Description = "Email or mobile number already exists.";
					return userModelDTO;
				}
				else if (isEmailExists)
				{
					userModelDTO.DataResponse.ReturnCode = ResultCodes.R_DUPLICATE;
					userModelDTO.DataResponse.Description = "Email already exists.";
					return userModelDTO;
				}
				else if (isMobileExists)
				{
					userModelDTO.DataResponse.ReturnCode = ResultCodes.R_DUPLICATE;
					userModelDTO.DataResponse.Description = "Mobile already exists.";
					return userModelDTO;
				}
				else
				{
					// Initialize User entity
					UserMasterEntity userEntity = await context.userMasterEntity.FirstOrDefaultAsync(x => x.Id == userModel.Data.Id && x.IsDeleted == 0);

					if (userEntity != null)
					{
						userEntity.DisplayName = userModel.Data.DisplayName ?? "";
						userEntity.EmailId = userModel.Data.EmailId;
						userEntity.MobileNo = userModel.Data.MobileNo;
						userEntity.UpdatedOn = DateTime.Now;

						context.Entry(userEntity).State = EntityState.Modified;
						int recStatus = await context.SaveChangesAsync();

						if (recStatus == 0)
							return userModelDTO;



						userModelDTO.DataResponse.ReturnCode = ResultCodes.R_SUCCESS;
						userModelDTO.DataResponse.Description = "Success";
						userModelDTO.Data = userModel.Data;
					}
					else
					{
						userModelDTO.DataResponse.ReturnCode = ResultCodes.R_NOT_FOUND;
						userModelDTO.DataResponse.Description = "Data Not Found";
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

			return userModelDTO;
		}

		internal static async Task<UserModelDTO> AddEditUser(UserModelVO userModel, PGDBContext context = null)
		{
			// Declare and initialize return model
			UserModelDTO userModelDTO = new()
			{
				DataResponse = Utils.GetProcessDefaultResponse(CommonUtility.ConvertEnumToString(ResultCodes.R_UNKNOWN.ToString()), ResultCodes.R_UNKNOWN)
			};

			try
			{
				using (var transaction = await context.Database.BeginTransactionAsync())
				{
					// Condition for adding a new user
					if (userModel.Data.Id == 0)
					{
						// Call AddUserMaster function
						userModelDTO = await AddUserMaster(userModel, context);
						// Check return value
						if (userModelDTO.DataResponse.ReturnCode != ResultCodes.R_SUCCESS)
						{
							await transaction.RollbackAsync();
						}
						else
						{
							await transaction.CommitAsync();
							userModelDTO.DataResponse.Description = "Success";
						}
					}
					else // Editing existing user
					{
						userModelDTO = await EditUserMaster(userModel, context);

						// Check return value
						if (userModelDTO.DataResponse.ReturnCode != ResultCodes.R_SUCCESS)
						{
							await transaction.RollbackAsync();
						}
						else
						{
							await transaction.CommitAsync();
							userModelDTO.DataResponse.Description = "Success";
						}
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

		internal static async Task<UserModelDTO> GetSpecificUserMasterData(UserModelVO userModel, PGDBContext context = null)
		{
			UserModelDTO userModelDTO = new UserModelDTO();
			userModelDTO.DataResponse = Utils.GetProcessDefaultResponse(CommonUtility.ConvertEnumToString(ResultCodes.R_NOT_FOUND.ToString()), ResultCodes.R_NO_DATA_FOUND);
			try
			{
				// Create variable 
				Int64 UserId = 0;
				//if (userModel.Data.ID == 0)
				//	UserId = userModel.Session.UserId;
				//else
				//	UserId = userModel.Data.ID;

				// Execute query for getting user information asynchronously
				UserModel userMasterModel = await (from userList in context.userMasterEntity
														 where userList.Id == UserId && userList.IsDeleted == 0
														 select new UserModel
														 {
															 Id = userList.Id,
															 Username = userList.UserName,
															 DisplayName = (!string.IsNullOrEmpty(userList.DisplayName)) ? userList.DisplayName : userList.EmailId,
															 EmailId = userList.EmailId,
															 MobileNo = userList.MobileNo,
														 }).FirstOrDefaultAsync();

				// Update the userModel with retrieved userMasterModel data
				userModel.Data = userMasterModel;
				//userModel.Data.UniqueUserTypeId = userMasterModel.UniqueUserTypeId;
				//userModel.Data.UserName = userMasterModel.UserName;
				//userModel.Data.DisplayName = userMasterModel.DisplayName;
				//userModel.Data.EmailId = userMasterModel.EmailId;
				//userModel.Data.MobileNo = userMasterModel.MobileNo;
				//userModel.Data.IsMaster = userMasterModel.IsMaster;
				//userModel.Data.UserType = userMasterModel.UserType;
				//userModel.Data.IsTempPassword = userMasterModel.IsTempPassword;
				//userModel.Data.DefLanguageUniqueId = userMasterModel.DefLanguageUniqueId ?? "en";
				// Get details of roles mapped to the user asynchronously
				//userModel.Data.Roles = await (from role in context.roleMasterEntities
				//							  join mappedRole in context.userRoleMappingEntities on role.RoleUniuqeId equals mappedRole.RoleUniqueId
				//							  where mappedRole.UserId == UserId && mappedRole.IsDeleted == 0
				//							  select new RoleMasterModel
				//							  {
				//								  Id = role.Id,
				//								  Name = role.Name,
				//								  RoleUniqueId = role.RoleUniuqeId
				//							  }).ToListAsync();

				//userModel.Data.CustomerList = await (from role in context.roleMasterEntities
				//									 join mappedRole in context.userRoleMappingEntities on role.RoleUniuqeId equals mappedRole.RoleUniqueId
				//									 join auditMaster in context.AuditorMasterEntities on mappedRole.UserId equals auditMaster.UserId
				//									 join auditDetail in context.AuditorDetailEntities on auditMaster.ID equals auditDetail.AuditorId
				//									 join customer in context.customerEntity on auditDetail.CustomerId equals customer.Id
				//									 where mappedRole.UserId == UserId && mappedRole.IsDeleted == 0
				//									 select new CustomerModel
				//									 {
				//										 Id = customer.Id,
				//										 Name = customer.Name
				//									 }).Distinct().ToListAsync();
				// Get consolidated privilege list asynchronously
				//BaseResponseModel baseResponse = await GetConsolidatedPrivilege(UserId, context, userModel);

				//if (baseResponse.DataResponse.ReturnCode == ResultCodes.R_SUCCESS)
				//	userModel.Data.PrivilegeList = (List<PrivilegeModel>)baseResponse.Data;
				//else
				//	userModel.Data.PrivilegeList = new List<PrivilegeModel>();

				userModelDTO.Data = new List<UserModel>();
				userModelDTO.Data.Add(userModel.Data);
				userModelDTO.DataResponse.ReturnCode = ResultCodes.R_SUCCESS;
				userModelDTO.DataResponse.Description = "Success.";
			}
			catch (Exception ex)
			{
				userModelDTO.DataResponse.ReturnCode = ResultCodes.R_DB_ERROR;
				userModelDTO.DataResponse.Description = ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString();
				SystemLogger.LogData<UserService>().LogError(ex, ex.Message);
			}
			finally
			{
				if (context != null)
					context.Dispose();
			}
			return userModelDTO;
		}

		internal static async Task<UserModelDTO> GetUserList(CommonRequestVO requestModel, PGDBContext context = null)
		{
			UserModelDTO userModelDTO = new()
			{
				DataResponse = Utils.GetProcessDefaultResponse(CommonUtility.ConvertEnumToString(ResultCodes.R_NOT_FOUND.ToString()), ResultCodes.R_NO_DATA_FOUND)
			};
			try
			{
				IQueryable<UserMasterEntity> userQuery = (from user in context.userMasterEntity
														  where user.IsDeleted == 0
														  select user);

				if (userQuery.Any())
				{
					requestModel.Data.TotalRows = await userQuery.CountAsync();

					//if (!string.IsNullOrEmpty(requestModel.Data.SearchText))
					//{
					//	userQuery = userQuery.Where(c => c.EmailId.ToUpper().Replace(" ", "").Contains(requestModel.Data.SearchText.ToUpper().Replace(" ", "")));
					//}

					//if (requestModel.Data.FromDate != null && requestModel != null)
					//	userQuery = userQuery.Where(c => c.CreatedOn >= requestModel.Data.FromDate.Value && c.CreatedOn <= requestModel.Data.ToDate.Value);

					//requestModel.Data.FilterRowsCount = await userQuery.CountAsync();

					//switch (requestModel.Data.OrderBy.ToUpper().Trim())
					//{
					//	case "EMAILID":
					//		userQuery = (requestModel.Data.OrderBy == "D") ? userQuery.OrderBy(s => s.EmailId) : userQuery.OrderByDescending(s => s.EmailId);
					//		break;

					//	case "CREATEDON":
					//		userQuery = (requestModel.Data.OrderBy == "D") ? userQuery.OrderBy(s => s.CreatedOn) : userQuery.OrderByDescending(s => s.CreatedOn);
					//		break;

					//	default:
					//		userQuery = userQuery.OrderByDescending(s => s.CreatedOn);
					//		break;
					//}

					userModelDTO.Data = await (from userList in userQuery
											   select new UserModel
											   {
												   Id = userList.Id,
												   Username = userList.UserName,
												   EmailId = userList.EmailId,
												   MobileNo = userList.MobileNo,
											   }).Skip(requestModel.Data.StartNo).Take(requestModel.Data.EndNo).ToListAsync();
				}
				else
					userModelDTO.Data = new List<UserModel>();
				userModelDTO.DataResponse.ReturnCode = ResultCodes.R_SUCCESS;
				userModelDTO.DataResponse.Description = "User List retrived.";
				userModelDTO.FilterModel = requestModel.Data;
			}
			catch (Exception ex)
			{
				userModelDTO.DataResponse.ReturnCode = ResultCodes.R_DB_ERROR;
				userModelDTO.DataResponse.Description = ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString();
				SystemLogger.LogData<UserService>().LogError(ex, ex.Message);

			}
			finally
			{
				context?.Dispose();
			}
			return userModelDTO;
		}


	}

}