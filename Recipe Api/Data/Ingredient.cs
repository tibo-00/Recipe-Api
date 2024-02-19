using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Api.Data
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public Recipes Recipes { get; set; }
        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; }
    }
}
