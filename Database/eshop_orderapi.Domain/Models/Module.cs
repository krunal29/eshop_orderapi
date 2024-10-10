using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_orderapi.Domain.Models
{
    [Table("Module")]
    public partial class Module : BaseEntity
    {
        public Module()
        {
            AccessModules = new HashSet<AccessModule>();
            InverseParent = new HashSet<Module>();
            RoleModules = new HashSet<RoleModule>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public short Type { get; set; }
        public int? SortOrder { get; set; }

        [ForeignKey("ParentId")]
        [InverseProperty("InverseParent")]
        public virtual Module Parent { get; set; }

        [InverseProperty("Module")]
        public virtual ICollection<AccessModule> AccessModules { get; set; }

        [InverseProperty("Parent")]
        public virtual ICollection<Module> InverseParent { get; set; }

        [InverseProperty("Module")]
        public virtual ICollection<RoleModule> RoleModules { get; set; }
    }
}