using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Market.Data;
using Mini_Market.Models;

namespace Mini_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        /// <summary>
        /// Сервис данных
        /// </summary>
        private readonly IDataStore _dataStore;

        public CategoriesController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        /// <summary>
        /// Id категории
        /// </summary>
        private static int _nextId = 1;


        /// <summary>
        /// Возвращает список всех категорий
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll() => Ok(_dataStore.Categories);

        /// <summary>
        /// Возвращает продукт по Id
        /// </summary>
        /// <param name="id">Id продукта</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _dataStore.Categories.FirstOrDefault(x => x.Id == id);

            return category == null ? NotFound() : Ok(category);
        }

        /// <summary>
        /// Создает новую категорию
        /// </summary>
        /// <param name="category">Новая категория</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] Category category)
        {
            category.Id = _nextId++;
            _dataStore.Categories.Add(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        /// <summary>
        /// Обновляем категорию
        /// </summary>
        /// <param name="id">Id категории</param>
        /// <param name="category">Обновленная категория</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Category category)
        {
            var item = _dataStore.Categories.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            item.Name = category.Name;

            return NoContent();
        }

        /// <summary>
        /// Удаление категории
        /// </summary>
        /// <param name="id">Id категории</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _dataStore.Categories.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            _dataStore.Categories.Remove(item);

            return NoContent();
        }
    }
}
