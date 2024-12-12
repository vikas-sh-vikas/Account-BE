using Acount.APIService.Common;
using System.Text.Json.Serialization;

namespace Acount.APIService.Models
{
    public class RequestModel
    {
        public virtual int OrgId { get; set; }
        public DateTime RequestDateTime { get; set; }

        [JsonIgnore]

		public RequestSource requestSource { get; set; }
		[JsonIgnore]
		public string Request { get; set; } = string.Empty;
        [JsonIgnore]
        public string IpAddress { get; set; } = string.Empty;
		[JsonIgnore]
        public string OriginName { get; set; } = string.Empty;

		[JsonIgnore]
        public virtual int IsDeleted { get; set; }
    }

    public class BaseResponseModel
    {
        public ResponseModel DataResponse { get; set; }
        public dynamic Data { get; set; }

    }
    public class ResponseModel
    {
        public ResultCodes ReturnCode { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public string Description { get; set; }
        //public string AuthToken { get; set; }
        //public string Token { get; set; }
    }
}
