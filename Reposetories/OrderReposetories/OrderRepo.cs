using E_Commerce_Proj.Data;
using E_Commerce_Proj.DTOs.OrderDTOs;
using E_Commerce_Proj.DTOs.ShipmentDTOs;
using E_Commerce_Proj.Reposetories.CartReposetories;
using Microsoft.EntityFrameworkCore;
using storeProject.Models;

namespace E_Commerce_Proj.Reposetories.OrderReposetories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;
        private readonly ICartRepo _cartRepo;
        private readonly HttpClient _httpClient;

        public OrderRepo(AppDbContext context, ICartRepo cartRepo, HttpClient httpClient)
        {
            _context = context;
            _cartRepo = cartRepo;
            _httpClient = httpClient;
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

        public async Task<ShippingResponseDTO> CreateOrderAsync(CreateOrderDTO orderDTO)
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

            var price = 0m;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            
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
            await _cartRepo.ClearCartAsync(user.Id);

            var request = new MakeShipmentDTO
            {
                senderName = "Aura Store",
                senderPhone = "1145466800",
                senderAddress = "sohaj,tahta,15",
                receiverName = user.FName + " " + user.LName,
                receiverAddress = orderDTO.Address,
                receiverPhone = orderDTO.PhoneNumber.Substring(1),
                egpAmount = price,
                clientID = 1

            };
            var api = "http://auracompanyfordeleveryservices.runasp.net/api/Shipment/MakeShipment";
            var response = await _httpClient.PostAsJsonAsync(api, request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ShippingResponseDTO>();
            order.OrderStatus = result.nowStatus;
            result.id = order.Id;

            var shipping = new Shipping
            {
                EstimatedDate = DateTime.Now,
                ExpectedDate = result.DeliveredAt,
                OrderId = order.Id,
                ShippingCarrier = result.DriverName ??  "Not Assigned Yet" ,
                ShippingStatus = result.nowStatus,
                TrackingNumber = "1234"
            };
            await _context.Shippings.AddAsync(shipping);
            await _context.SaveChangesAsync();
            return result;

        }

        public async Task<List<DisplayOrderDetails>> DisplayAllOrdersAsync()
        {
            var orders = await _context.Orders.Select(x => new DisplayOrderDetails
            {
                Id = x.Id,
                Items = x.orderItems.Select(oi => new DisplayOrderItem
                {
                    Id = oi.Id,
                    Image = oi.product.ImageURL,
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

        public async Task<DisplayOrderDetails> DisplayOrderDetailsAsync(int orderId)
        {
            var order = await _context.Orders.Where(x => x.Id == orderId).Select(x => new DisplayOrderDetails
            {
                Id = x.Id,
                Items = x.orderItems.Select(oi => new DisplayOrderItem
                {
                    Id = oi.Id,
                    Image = oi.product.ImageURL,
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
            }).FirstOrDefaultAsync();
            if(order == null)
                return null;
            var api = $"http://auracompanyfordeleveryservices.runasp.net/api/Shipment/GetShipmentHistory/{orderId}";
            var response = await _httpClient.GetAsync(api);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<ShippingStatusDTO>>();

           order.ShippingStatus = result.OrderByDescending(x => x.changeAt).FirstOrDefault()?.statusValue ?? order.ShippingStatus;
           await _context.SaveChangesAsync();



            return order;
        }

        public async Task<List<DisplayOrderDetails>> DisplayOrdersPerUserAsync(int userId)
        {
            var userOrders =  await _context.Orders.Where(x => x.CustomerId == userId).Select(x => new DisplayOrderDetails
            {
                Id = x.Id,
                Items = x.orderItems.Select(oi => new DisplayOrderItem
                {
                    Id = oi.Id,
                    Image = oi.product.ImageURL,
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
