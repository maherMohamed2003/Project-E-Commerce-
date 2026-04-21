using E_Commerce_Proj.DTOs.OrderDTOs;

namespace E_Commerce_Proj.Reposetories.OrderReposetories
{
    public interface IOrderRepo
    {
        public Task<DisplayOrderDetails> CreateOrderAsync(CreateOrderDTO orderDTO);
        public Task<List<DisplayOrderDetails>>  DisplayOrdersPerUserAsync(int userId);
        public Task<List<DisplayOrderDetails>> DisplayAllOrdersAsync();
        public Task<string> CancelOrderAsync(int orderId);

        public Task<OrdersOverviewDTO>  GetOrdersOverviewAsync();
    }
}
