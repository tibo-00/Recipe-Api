using System.ComponentModel.DataAnnotations.Schema;

namespace Recipe_Api.Data
{
    public class Recipes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Time { get; set; }
        public Difficulty Difficulty { get; set; }
        public Categories Categories { get; set; }
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
    }
}
