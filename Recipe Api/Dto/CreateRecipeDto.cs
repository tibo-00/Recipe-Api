namespace Recipe_Api.Dto
{
    public class CreateRecipeDto
    {
        public string Title { get; set; }
        public int Difficulty { get; set; }
        public int Time { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<CreateIngredientDto> Ingredients { get; set; }
    }
}
