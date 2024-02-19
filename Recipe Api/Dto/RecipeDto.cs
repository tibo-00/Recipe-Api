namespace Recipe_Api.Dto
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Difficulty { get; set; }
        public int Time { get; set; }
        public string Category { get; set; }
        public IEnumerable<IngredientDto> Ingredients { get; set; }
    }
}
