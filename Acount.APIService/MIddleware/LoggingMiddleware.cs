using Acount.APIService.Common.Models;
using Microsoft.AspNetCore.Http.Features;
using System.Text;

namespace Acount.APIService.MIddleware
{
	public class LoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _logger;
		public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
		{
			_next = next;
			_logger = loggerFactory.CreateLogger<LoggingMiddleware>();
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				//EndpointMetadataCollection endpointMetaData = context.Features.Get<IEndpointFeature>()?.Endpoint.Metadata;

				context.Request.EnableBuffering();

				var requestLog = string.Empty;

				var auditLog = await LogRequest(context);

				//Log All Incoming Requests
				requestLog = requestLog + $"REQUEST HttpURL: {auditLog.HttpURL}, " +
					//$"Headers: {Newtonsoft.Json.JsonConvert.SerializeObject(auditLog.Headers)}"+
					$"Body: {auditLog.Form}";
				_logger.LogInformation(requestLog);



				await LogResponse(context, auditLog);
			}
			catch (UnauthorizedAccessException uex)
			{
				_logger.LogError(uex, "UnAuthorized Access Exception.");
				throw;
			}
			catch (Exception ex)
			{
				//Custom exception logging here
				_logger.LogError(ex, "Exception Occurs.");
			}
		}

		public async Task<AuditLog> LogRequest(HttpContext context)
		{
			IHttpRequestFeature features = context.Features.Get<IHttpRequestFeature>();
			string url = $"{features.Scheme}://{context.Request.Host.Value}{features.RawTarget}";

			IFormCollection form = null;
			string formString = string.Empty;

			if (context.Request.HasFormContentType)
			{
				form = context.Request.Form;
			}
			else
			{
				formString = await new StreamReader(context.Request.Body).ReadToEndAsync();
				var injectedRequestStream = new MemoryStream();
				byte[] bytesToWrite = Encoding.UTF8.GetBytes(formString);
				injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
				injectedRequestStream.Seek(0, SeekOrigin.Begin);
				context.Request.Body = injectedRequestStream;
			}

			return new AuditLog()
			{
				RemoteHost = context.Connection.RemoteIpAddress.ToString(),
				HttpURL = url,
				LocalAddress = context.Connection.LocalIpAddress.ToString(),
				Headers = Newtonsoft.Json.JsonConvert.SerializeObject(context.Request.Headers),
				Form = form != null ? Newtonsoft.Json.JsonConvert.SerializeObject(form) : formString
			};
		}

		public async Task LogResponse(HttpContext context, AuditLog auditLog)
		{
			if (auditLog == null)
			{
				await _next(context);
				return;
			}

			Stream originalBody = context.Response.Body;

			try
			{

				using (var memStream = new MemoryStream())
				{
					context.Response.Body = memStream;
					await _next(context);

					memStream.Position = 0;
					string responseBody = new StreamReader(memStream).ReadToEnd();
					auditLog.ResponseStatusCode = context.Response.StatusCode;
					auditLog.ResponseBody = responseBody;
					//_logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(auditLog));
					//_logger.LogInformation("Response Data:" + responseBody);
					if (context.Response.ContentType == "application/json; charset=utf-8")
					{

						_logger.LogInformation("Response Data:" + responseBody);
					}
					memStream.Position = 0;
					await memStream.CopyToAsync(originalBody);
				}



			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while loggine HTTP Response.");
				//throw ex;
			}
			finally
			{
				context.Response.Body = originalBody;
			}
		}

	}

}
