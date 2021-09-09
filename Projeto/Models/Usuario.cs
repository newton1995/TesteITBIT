using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace TesteITBIT.Models
{
    [Table("Usuario")]
    public partial class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage ="O nome tem que ter no minimo 3 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [Display(Name = "Data De Nascimento")]

        public DateTime DataNascimento { get; set; }
        
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "O e-mail inserido não é valido")]
        [Display(Name = "E-mail")]
        [StringLength(100)]
        public string Email { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
                               ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um caractere especial e ter entre 8 e 15 dígitos.")]
        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [StringLength(30)]
        public string Senha { get; set; }
        public bool Ativo { get; set; }

        [Display(Name = "Sexo")]
        [Required(ErrorMessage = "O sexo é obrigatório")]
        public int SexoId { get; set; }

        [ForeignKey(nameof(SexoId))]
        [InverseProperty("Usuarios")]
        public virtual Sexo Sexo { get; set; }
    }
}
