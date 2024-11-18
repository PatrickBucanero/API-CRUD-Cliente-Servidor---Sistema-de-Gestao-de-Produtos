namespace ProjetoooAPI.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Value { get; set; }

        public Categoria? Category { get; set; }
    }
}
