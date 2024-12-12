namespace Acount.APIService.Common
{
	public class SystemLogger
	{
		//public static ILogger _logger;
		public static ILoggerFactory loggerFactory;

		public static ILogger LogData<T>()
		{
			ILogger _logger = SystemLogger.loggerFactory.CreateLogger<T>();
			return _logger;
		}
	}
}
