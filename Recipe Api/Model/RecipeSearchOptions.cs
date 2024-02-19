namespace Recipe_Api.Model
{
    public class RecipeSearchOptions
    {
        public string? SearchTerm { get; set; }
        public int? MaxDifficulty { get; set; }
        public int? MaxTime { get; set; }
        public List<int> Categories { get; set; } = new List<int>();
    }
}
