using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipe_Api.Data;
using Recipe_Api.Dto;
using Recipe_Api.Model;

namespace Recipe_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecipeController : ControllerBase
    {
        private readonly AppDBContext _context;

        public RecipeController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IEnumerable<RecipeListDto> GetRecipes()
        {
            return _context.Recipes.Select(x => new RecipeListDto
            {
                Id = x.Id,
                Title = x.Title,
                Time = x.Time,
                Category = x.Categories.Name,
                Difficulty = (int)x.Difficulty
            }).ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetRecipe(int id)
        {
            Recipes recipe = _context.Recipes.Include(x => x.Categories).FirstOrDefault(x => x.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }
            IEnumerable<Ingredient> ingredients = _context.Ingredients.Where(x => x.RecipeId == recipe.Id);

            RecipeDto recipeDto = new RecipeDto
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Time = recipe.Time,
                Category = recipe.Categories.Name,
                Difficulty = (int)recipe.Difficulty,
                Ingredients = ingredients.Select(x => new IngredientDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Quantity = x.Quantity,
                    Unit = x.Unit
                }).ToList()
            };

            return Ok(recipeDto);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateRecipe(CreateRecipeDto recipeDto)
        {
            Categories category = _context.Categories.Find(recipeDto.CategoryId);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            Recipes recipe = new Recipes
            {
                Title = recipeDto.Title,
                Time = recipeDto.Time,
                Difficulty = (Difficulty)recipeDto.Difficulty,
                CategoryId = recipeDto.CategoryId,
            };
            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            foreach (CreateIngredientDto ingredientDto in recipeDto.Ingredients)
            {
                Ingredient ingredient = new Ingredient()
                {
                    Name = ingredientDto.Name,
                    Quantity = ingredientDto.Quantity,
                    Unit = ingredientDto.Unit,
                    RecipeId = recipe.Id
                };
                _context.Ingredients.Add(ingredient);
            }

            _context.SaveChanges();

            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult DeleteRecipe(int id)
        {
            Recipes recipe = _context.Recipes.Find(id);
            if (recipe == null) { return NotFound(); }

            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<RecipeListDto>> Search([FromQuery] RecipeSearchOptions options)
        {
            IQueryable<Recipes> recipes = _context.Recipes.Include(r => r.Categories).AsQueryable();

            if (!string.IsNullOrEmpty(options.SearchTerm))
            {
                recipes = recipes.Where(x => x.Title.Contains(options.SearchTerm));
            }

            if (options.MaxDifficulty.HasValue)
            {
                recipes = recipes.Where(x => (int)x.Difficulty <= options.MaxDifficulty);
            }

            if (options.MaxTime.HasValue)
            {
                recipes = recipes.Where(x => x.Time <= options.MaxTime);
            }

            if (options.Categories.Any())
            {
                recipes = recipes.Where(x => options.Categories.Contains(x.CategoryId));
            }

            IEnumerable<RecipeListDto> recipeListDto = recipes.Select(x => new RecipeListDto
            {
                Id = x.Id,
                Title = x.Title,
                Time = x.Time,
                Category = x.Categories.Name,
                Difficulty = (int)x.Difficulty
            });

            return Ok(recipeListDto);
        }
    }
}
