using eshop_orderapi.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_orderapi.Domain
{
    public partial class Person : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public int RoleId { get; set; }

        [StringLength(450)]
        public string AspNetUserId { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("People")]
        public virtual Role Role { get; set; }
    }
}