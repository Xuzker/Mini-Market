using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Market.Data;
using Mini_Market.Models;

namespace Mini_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Сервис данных
        /// </summary>
        private readonly IDataStore _dataStore;

        public ProductsController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }


        /// <summary>
        /// Id продукта
        /// </summary>
        private static int _nextId = 1;


        /// <summary>
        /// Возвращает список всех продуктов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll() => Ok(_dataStore.Products);

        /// <summary>
        /// Возвращает продукт по Id
        /// </summary>
        /// <param name="id">Id продукта</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _dataStore.Products.FirstOrDefault(x => x.Id == id);

            return product == null ? NotFound() : Ok(product);
        }

        /// <summary>
        /// Создает новый продукт
        /// </summary>
        /// <param name="product">Новый продукт</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name)) return BadRequest("Name is required");
            if (product.Price < 0) return BadRequest("Price cannot be negative");
            if (product.Stock < 0) return BadRequest("Stock cannot be negative");

            product.Id = _nextId++;
            _dataStore.Products.Add(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Обновляем продукт
        /// </summary>
        /// <param name="id">Id продукта</param>
        /// <param name="product">Обновленный продукт</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            var item = _dataStore.Products.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            if (string.IsNullOrWhiteSpace(product.Name)) return BadRequest("Name is required");
            if (product.Price < 0) return BadRequest("Price cannot be negative");
            if (product.Stock < 0) return BadRequest("Stock cannot be negative");

            item.Name = product.Name;
            item.CategoryId = product.CategoryId;
            item.Price = product.Price;
            item.Stock = product.Stock;

            return NoContent();
        }

        /// <summary>
        /// Удаление продукта
        /// </summary>
        /// <param name="id">Id продукта</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _dataStore.Products.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            _dataStore.Products.Remove(item);

            return NoContent();
        }

        /// <summary>
        /// Фильтрация товара по условию
        /// </summary>
        /// <param name="categoryId">Id категории</param>
        /// <param name="minPrice">Минимальная цена</param>
        /// <param name="maxPrice">Максимальная цена</param>
        /// <returns></returns>
        [HttpGet("filter")]
        public IActionResult GetFiltered([FromQuery] int? categoryId, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var filtered = _dataStore.Products.AsEnumerable();

            if (categoryId.HasValue)
                filtered = filtered.Where(p => p.CategoryId == categoryId.Value);

            if (minPrice.HasValue)
                filtered = filtered.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                filtered = filtered.Where(p => p.Price <= maxPrice.Value);

            return Ok(filtered.ToList());
        }

    }
}
