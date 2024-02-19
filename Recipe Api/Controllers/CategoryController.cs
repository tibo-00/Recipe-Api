using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe_Api.Data;
using Recipe_Api.Dto;

namespace Recipe_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly AppDBContext _context;

        public CategoryController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IEnumerable<Categories> GetCategories()
        {
            return  _context.Categories.ToList();
        }

        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            Categories category = _context.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateCategory(CreateCategoryDto dto)
        {
            Categories category = new Categories()
            {
                Name= dto.Name
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
    }
}
