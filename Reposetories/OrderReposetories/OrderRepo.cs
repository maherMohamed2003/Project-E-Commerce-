using E_Commerce_Proj.Data;
using E_Commerce_Proj.DTOs.OrderDTOs;
using E_Commerce_Proj.Reposetories.CartReposetories;
using Microsoft.EntityFrameworkCore;
using storeProject.Models;

namespace E_Commerce_Proj.Reposetories.OrderReposetories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;
        private readonly ICartRepo _cartRepo;

        public OrderRepo(AppDbContext context, ICartRepo cartRepo)
        {
            _context = context;
            _cartRepo = cartRepo;
        }

        public async Task<string> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                return "Order not found.";
            }

            var orderItems = await _context.OrderItems.Where(x => x.OrderId == orderId).ToListAsync();
            if(orderItems == null || orderItems.Count == 0)
            {
                return "No items found for this order.";
            }

            
            foreach (var item in orderItems)
            {
                Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }
            
            await _context.OrderItems.Where(o => o.OrderId == orderId).ForEachAsync(oi => _context.OrderItems.Remove(oi));
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return "Order cancelled successfully.";
        }

        public async Task<DisplayOrderDetails> CreateOrderAsync(CreateOrderDTO orderDTO)
        {
            var user = await _context.Customers.Include(x => x.cart).FirstOrDefaultAsync(x => x.Id == orderDTO.UserId);
            if(user == null)
            {
                return null;
            }

            var cartItems = await _context.CartItems.Include(x => x.product).Where(x => x.CartId == user.cart.Id).ToListAsync();
            if(cartItems == null || cartItems.Count == 0)
            {
                return null;
            }
            var order = new Order
            {
                CustomerId = orderDTO.UserId,
                OrderDate = DateTime.Now,
                OrderStatus = "Pending",
                Address = orderDTO.Address,
                PhoneNumber = orderDTO.PhoneNumber
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var price = 0m;
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    OrderId =order.Id
                };
                await _context.OrderItems.AddAsync(orderItem);
                price += item.product.Price * item.Quantity;

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;
                }

            }
            order.TotalPrice = price;
            await _context.SaveChangesAsync();
            
            var info = new DisplayOrderDetails
            {
                Id = order.Id,
                Items = cartItems.Select(x => new DisplayOrderItem {
                    Id = x.Id,
                    Quantity = x.Quantity,
                ProductName = x.product.Name,
                PricePerUnit = x.product.Price
                }).ToList(),

                TotalPrice = price,
                PhoneNumber = order.PhoneNumber,
                Address = order.Address,
                ShippingStatus = order.shipping != null ? order.shipping.ShippingStatus : "Not Shipped",
                ShippingCarrier = order.shipping != null ? order.shipping.ShippingCarrier : "N/A",
                TrackingNumber = order.shipping != null ? order.shipping.TrackingNumber : "N/A",
            };
            await _cartRepo.ClearCartAsync(user.Id);
            return info;

            //Send this Order To Shipping Service Here !!
        }

        public async Task<List<DisplayOrderDetails>> DisplayAllOrdersAsync()
        {
            var orders = await _context.Orders.Select(x => new DisplayOrderDetails
            {
                Id = x.Id,
                Items = x.orderItems.Select(oi => new DisplayOrderItem
                {
                    ProductName = oi.product.Name,
                    Quantity = oi.Quantity,
                    PricePerUnit = oi.product.Price
                }).ToList(),

                TotalPrice = x.TotalPrice,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                ShippingStatus = x.shipping != null ? x.shipping.ShippingStatus : "Not Shipped",
                ShippingCarrier = x.shipping != null ? x.shipping.ShippingCarrier : "N/A",
                TrackingNumber = x.shipping != null ? x.shipping.TrackingNumber : "N/A",
            }).ToListAsync();
            
            return orders;
        }

        public async Task<List<DisplayOrderDetails>> DisplayOrdersPerUserAsync(int userId)
        {
            var userOrders =  await _context.Orders.Where(x => x.CustomerId == userId).Select(x => new DisplayOrderDetails
            {
                Id = x.Id,
                Items = x.orderItems.Select(oi => new DisplayOrderItem
                {
                    ProductName = oi.product.Name,
                    Quantity = oi.Quantity,
                    PricePerUnit = oi.product.Price
                }).ToList(),
                TotalPrice = x.TotalPrice,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                ShippingStatus = x.shipping != null ? x.shipping.ShippingStatus : "Not Shipped",
                ShippingCarrier = x.shipping != null ? x.shipping.ShippingCarrier : "N/A",
                TrackingNumber = x.shipping != null ? x.shipping.TrackingNumber : "N/A",
            }).ToListAsync();

            return userOrders;
        }

        public async Task<OrdersOverviewDTO> GetOrdersOverviewAsync()
        {
            var numberOfOrders = await _context.Orders.CountAsync();
            var numberOfPendingOrders = await _context.Orders.Where(x => x.OrderStatus == "Pending").CountAsync();
            var numberOfCompletedOrders = await _context.Orders.Where(x => x.OrderStatus == "Completed").CountAsync();
            var res = new OrdersOverviewDTO
            {
                NumberOfAllOrders = numberOfOrders,
                NumberOfPendingOrders = numberOfPendingOrders,
                NumberOfCompletedOrders = numberOfCompletedOrders
            };
            return res;
        }
    }
}
