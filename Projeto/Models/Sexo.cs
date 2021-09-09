using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace TesteITBIT.Models
{
    [Table("Sexo")]
    public partial class Sexo
    {
        public Sexo()
        {
            Usuarios = new HashSet<Usuario>();
        }

        [Key]
        public int SexoId { get; set; }
        [Required]
        [StringLength(15)]
        public string Descricao { get; set; }

        [InverseProperty(nameof(Usuario.Sexo))]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
