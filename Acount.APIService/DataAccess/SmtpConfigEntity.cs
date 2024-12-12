using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Acount.APIService.DataAccess
{
    [Table("smtp_configuration", Schema = "public")]
    public class SmtpConfiguration
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [Column("typeid")]
        public int TypeId { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("host")]
        public string Host { get; set; }

        [Required]
        [Column("smtp_port")]
        public int SmtpPort { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("sender")]
        public string Sender { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [MaxLength(500)]
        [Column("password")]
        public string Password { get; set; }

        [MaxLength(255)]
        [Column("sendername")]
        public string? SenderName { get; set; }

        [Column("orgid")]
        public int OrgId { get; set; } = 0;

        [Column("createdon")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Column("createdby")]
        public long CreatedBy { get; set; } = 0;

        [Column("updatedon")]
        public DateTime? UpdatedOn { get; set; }

        [Column("updatedby")]
        public long? UpdatedBy { get; set; }

        [Required]
        [Column("isdeleted")]
        public short IsDeleted { get; set; } = 0;

        [Column("deletedon")]
        public DateTime? DeletedOn { get; set; }

        [Column("deletedby")]
        public long? DeletedBy { get; set; }
    }
}
