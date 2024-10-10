using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_orderapi.Domain.Models
{
    [Table("RoleModule")]
    public partial class RoleModule : BaseEntity
    {
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public bool Overview { get; set; }
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }

        [ForeignKey("ModuleId")]
        [InverseProperty("RoleModules")]
        public virtual Module Module { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("RoleModules")]
        public virtual Role Roles { get; set; }
    }
}