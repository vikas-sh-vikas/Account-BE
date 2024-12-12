using Acount.APIService.Common;
using Acount.APIService.DataAccess;
using Acount.APIService.Models;

namespace Acount.APIService.Process
{
	public partial class ProcessManager
	{
		protected R ProcessRequest<T, R>(Func<T, PGDBContext, ResponseModel> validator, Func<T, PGDBContext, R> serviceCall, T arg, PGDBContext dbContext)
		   where T : RequestModel
		   where R : BaseResponseModel, new()
		{
			ResponseModel errorParam;

			R response = new()
			{
				DataResponse = new ResponseModel()
				{
					ReturnCode = ResultCodes.R_INVALID_REQUEST,
					ResponseDateTime = DateTime.Now,
					Description = CommonUtility.ConvertEnumToString(ResultCodes.R_INVALID_REQUEST.ToString())
				}
			};
			try
			{
				//If no arguments are found
				if (arg == null) return response;
				//Set DateTime
				arg.RequestDateTime = DateTime.Now;

				try
				{
					//Create Default Response
					errorParam = new ResponseModel()
					{
						ReturnCode = ResultCodes.R_UNKNOWN,
						ResponseDateTime = DateTime.Now,
						Description = CommonUtility.ConvertEnumToString(ResultCodes.R_UNKNOWN.ToString())
					};

					//Validate Request
					if (validator != null)
					{
						errorParam = validator(arg, dbContext);
						//response.ReturnCode = errorParam.ReturnCode;
						response.DataResponse.ReturnCode = errorParam.ReturnCode;

						//Debug log
						SystemLogger.LogData<ProcessManager>().LogDebug("Validation process Complete");

						if (errorParam.ReturnCode != ResultCodes.R_SUCCESS)
						{
							response.DataResponse.Description = errorParam.Description;
							return response;
						}
					}

					//Service Call
					response = serviceCall(arg, dbContext);
					return response;
				}
				catch (Exception ex)
				{
					//Log Request
					SystemLogger.LogData<ProcessManager>().LogError(ex, ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString());
					response.DataResponse = new ResponseModel() { ReturnCode = ResultCodes.R_SERVICE_ERROR, Description = ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString() + " " + "R_SERVICE_ERROR" };
				}
			}
			finally
			{
				dbContext?.Dispose();
			}

			return response;
		}

		protected async Task<R> ProcessRequestasync<T, R>(Func<T, PGDBContext, ResponseModel> validator, Func<T, PGDBContext, Task<R>> serviceCall, T arg, PGDBContext dbContext)
		   where T : RequestModel
		   where R : BaseResponseModel, new()
		{
			ResponseModel errorParam;

			R response = new()
			{
				DataResponse = new ResponseModel()
				{
					ReturnCode = ResultCodes.R_INVALID_REQUEST,
					ResponseDateTime = DateTime.Now,
					Description = CommonUtility.ConvertEnumToString(ResultCodes.R_INVALID_REQUEST.ToString())
				}
			};
			try
			{
				//If no arguments are found
				if (arg == null) return response;
				//Set DateTime
				arg.RequestDateTime = DateTime.Now;

				try
				{
					//Create Default Response
					errorParam = new ResponseModel()
					{
						ReturnCode = ResultCodes.R_UNKNOWN,
						ResponseDateTime = DateTime.Now,
						Description = CommonUtility.ConvertEnumToString(ResultCodes.R_UNKNOWN.ToString())
					};

					//Validate Request
					if (validator != null)
					{
						errorParam = validator(arg, dbContext);
						//response.ReturnCode = errorParam.ReturnCode;
						response.DataResponse.ReturnCode = errorParam.ReturnCode;

						//Debug log
						SystemLogger.LogData<ProcessManager>().LogDebug("Validation process Complete");

						if (errorParam.ReturnCode != ResultCodes.R_SUCCESS)
						{
							response.DataResponse.Description = errorParam.Description;
							return response;
						}
					}

					//Service Call
					response = await serviceCall(arg, dbContext);
					return response;
				}
				catch (Exception ex)
				{
					//Log Request
					SystemLogger.LogData<ProcessManager>().LogError(ex, ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString());
					response.DataResponse = new ResponseModel() { ReturnCode = ResultCodes.R_SERVICE_ERROR, Description = ex.InnerException == null ? ex.Message : ex.Message + " InnerException: " + ex.InnerException.Message.ToString() + " " + "R_SERVICE_ERROR" };
				}
			}
			finally
			{
				dbContext?.Dispose();
			}

			return response;
		}

	}

}
