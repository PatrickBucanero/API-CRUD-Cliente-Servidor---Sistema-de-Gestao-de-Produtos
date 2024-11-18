using System.ComponentModel.DataAnnotations;

namespace ProjetoooAPI.ViewModels
{
    public class ProdutoCreateViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public decimal Value { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
