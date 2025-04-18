using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Market.Data;
using Mini_Market.Models;

namespace Mini_Market.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        /// <summary>
        /// Сервис данных
        /// </summary>
        private readonly IDataStore _dataStore;

        public OrdersController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        /// <summary>
        /// Id заказа
        /// </summary>
        private static int _nextId = 1;


        /// <summary>
        /// Возвращает список всех заказов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll() => Ok(_dataStore.Orders);

        /// <summary>
        /// Возвращает заказ по Id
        /// </summary>
        /// <param name="id">Id заказа</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _dataStore.Orders.FirstOrDefault(x => x.Id == id);

            return order == null ? NotFound() : Ok(order);
        }

        /// <summary>
        /// Создает новый заказ
        /// </summary>
        /// <param name="productIds">Id продуктов</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody] List<int> productIds)
        {
            if (productIds == null || !productIds.Any())
                return BadRequest("Product IDs are required");

            var products = _dataStore.Products.Where(p => productIds.Contains(p.Id)).ToList();
            if (products.Count != productIds.Count)
                return BadRequest("One or more product IDs are invalid");

            var order = new Order
            {
                Id = _nextId++,
                ProductIds = productIds,
                OrderDate = DateTime.UtcNow,
                TotalPrice = products.Sum(p => p.Price)
            };

            _dataStore.Orders.Add(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
    }
}
