using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Acount.APIService.DataAccess
{
	[Table("user_master", Schema = "public")]
	public class UserMasterEntity
	{
		[Key]
		[Column("id")]
		public long Id { get; set; }

		[Required]
		[Column("username", TypeName = "character varying(500)")]
		public string UserName { get; set; }

		[Required]
		[Column("displayname", TypeName = "character varying(500)")]
		public string DisplayName { get; set; }

		[Required]
		[Column("emailid", TypeName = "character varying(500)")]
		public string EmailId { get; set; }

		[Required]
		[Column("mobileno", TypeName = "character varying(100)")]
		public string MobileNo { get; set; }

		[Required]
		[Column("password", TypeName = "character varying(200)")]
		public string Password { get; set; }

		[Column("previous_pwd", TypeName = "character varying(100)")]
		public string? PreviousPassword { get; set; }

		[Column("createdon")]
		public DateTime? CreatedOn { get; set; } = DateTime.Now;

		[Column("createdby")]
		public int CreatedBy { get; set; } = 0;

		[Column("updatedon")]
		public DateTime? UpdatedOn { get; set; }

		[Column("updatedby")]
		public int? UpdatedBy { get; set; }

		[Required]
		[Column("isdeleted")]
		public short IsDeleted { get; set; } = 0;

		[Column("deletedon")]
		public DateTime? DeletedOn { get; set; }

		[Column("deletedby")]
		public int? DeletedBy { get; set; }

		[Column("org_id")]
		public int? OrgId { get; set; }

		[Column("usertype")]
		public short? UserType { get; set; } = 0;

		[Column("istemppassword")]
		public int IsTempPassword { get; set; } = 1;
	}

}
