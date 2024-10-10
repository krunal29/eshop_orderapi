using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eshop_orderapi.Domain.Models
{
    [Table("AccessModule")]
    public partial class AccessModule : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int ModuleId { get; set; }
        public bool Overview { get; set; }
        public bool View { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }

        [ForeignKey("ModuleId")]
        [InverseProperty("AccessModules")]
        public virtual Module Module { get; set; }
    }
}