using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_orderapi.Domain.Models
{
    [Table("Role")]
    public partial class Role : BaseEntity
    {
        public Role()
        {
            InverseParent = new HashSet<Role>();
            RoleModules = new HashSet<RoleModule>();
            People = new HashSet<Person>();
        }

        [Key]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("ParentId")]
        [InverseProperty("InverseParent")]
        public virtual Role Parent { get; set; }

        [InverseProperty("Parent")]
        public virtual ICollection<Role> InverseParent { get; set; }

        [InverseProperty("Roles")]
        public virtual ICollection<RoleModule> RoleModules { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<Person> People { get; set; }
    }
}