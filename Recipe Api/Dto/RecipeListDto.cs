using Recipe_Api.Data;

namespace Recipe_Api.Dto
{
    public class RecipeListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Time { get; set; }
        public string Category { get; set; }
        public int Difficulty { get; set; }
    }
}
