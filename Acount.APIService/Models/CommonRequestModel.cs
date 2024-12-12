

namespace Acount.APIService.Models
{
    public class CommonRequestVO : RequestModel
	{
        public CommonRequestModel Data { get; set; }
    }
    public class CommonRequestModel 
	{
        public virtual int Id { get; set; }
        public virtual int CurrentPage { get; set; }
        public virtual int PageSize { get; set; }
        public virtual string OrderBy { get; set; }
        public virtual string OrderType { get; set; }
        public virtual string SearchText { get; set; }
        public virtual long TotalRows { get; set; }
        public virtual long FilterRowsCount { get; set; }
        public virtual int StartNo { get; set; }
        public virtual int EndNo { get; set; }

    }
}
