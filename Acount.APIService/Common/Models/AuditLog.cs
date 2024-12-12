namespace Acount.APIService.Common.Models
{
	public class AuditLog
	{
		public string RemoteHost { get; set; }
		public string HttpURL { get; set; }
		public string LocalAddress { get; set; }
		public string Headers { get; set; }
		public string Form { get; set; }
		public int ResponseStatusCode { get; set; }
		public string ResponseBody { get; set; }
	}
}
