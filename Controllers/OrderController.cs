using E_Commerce_Proj.DTOs.OrderDTOs;
using E_Commerce_Proj.Reposetories.OrderReposetories;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_Proj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;

        public OrderController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDTO)
        {
            var result = await _orderRepo.CreateOrderAsync(orderDTO);
            if (result == null)
            {
                return NotFound("Empty Cart!");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("DisplayAllOrders")]
        public async Task<IActionResult> DisplayAllOrders()
        {
            var result = await _orderRepo.DisplayAllOrdersAsync();
            if (result == null)
                return NotFound("No Orders Found!");
            return Ok(result);
        }

        [HttpGet]
        [Route("DisplayOrdersPerUser/{userId}")]
        public async Task<IActionResult> DisplayOrdersPerUser(int userId)
        {
            var result = await _orderRepo.DisplayOrdersPerUserAsync(userId);
            if (result == null)
                return NotFound("No Orders Found For This User!");
            return Ok(result);
        }

        [HttpDelete]
        [Route("CancelOrder/{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var result = await _orderRepo.CancelOrderAsync(orderId);
            if (result == "Order not found.")
                return NotFound(result);
            return Ok(result);
        }


        }
}
