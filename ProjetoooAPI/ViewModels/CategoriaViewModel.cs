using System.ComponentModel.DataAnnotations;

namespace ProjetoooAPI.ViewModels
{
    public class CategoriaViewModel
    {
        public class CategoriaCreateViewModel
        {
            [Required]
            public string Name { get; set; } = string.Empty;
        }
    }
}
