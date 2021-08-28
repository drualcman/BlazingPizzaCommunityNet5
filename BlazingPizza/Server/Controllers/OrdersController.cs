using BlazingPizza.Server.Models;
using BlazingPizza.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebPush;
using System.Text.Json;

namespace BlazingPizza.Server.Controllers
{
    [Route("orders")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly PizzaStoreContext Context;
        public OrdersController(PizzaStoreContext context) =>
            this.Context = context;

        private string GetUserId()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost]
        public async Task<ActionResult<int>> PlaceOrder(Order order)
        {
            order.CreatedTime = DateTime.Now;
            // Establecer una ubicación de envío ficticia
            order.DeliveryLocation =
                new LatLong(15.1992362, 120.5854669);

            // Establecer el valor de Pizza.SpecialId y Topping.ToppingId
            // para que no se creen nuevos registros Special y Topping.
            foreach (var Pizza in order.Pizzas)
            {
                Pizza.SpecialId = Pizza.Special.Id;
                Pizza.Special = null;

                foreach (var topping in Pizza.Toppings)
                {
                    topping.ToppingId = topping.Topping.Id;
                    topping.Topping = null;
                }
            }
            order.UserId = GetUserId();
            Context.Orders.Attach(order);
            await Context.SaveChangesAsync();

            //en segundo plano enviar las notificacions push de ser posible
            NotificationSubscription subscription = await Context.NotificationSubscriptions.Where(u => u.UserId == GetUserId()).SingleOrDefaultAsync();
            if (subscription is not null)
            {
                _ = TrackAndSendNotificationAsync(order, subscription);
            }

            return order.OrderId;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderWithStatus>>> GetOrders()
        {
            var Orders = await Context.Orders
                .Where(u => u.UserId == GetUserId())
                .Include(o => o.DeliveryLocation)
                .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                .Include(o => o.Pizzas).ThenInclude(p => p.Toppings)
                .ThenInclude(t => t.Topping)
                .OrderByDescending(o=>o.CreatedTime)
                .ToListAsync();
            return Orders.Select(o=> OrderWithStatus.FromOrder(o)).ToList();
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            IActionResult result;

            var Order = await Context.Orders
                .Where(o => o.OrderId == orderId)
                .Where(u => u.UserId == GetUserId())
                .Include(o => o.DeliveryLocation)
                .Include(o => o.Pizzas).ThenInclude(p => p.Special)
                .Include(o => o.Pizzas).ThenInclude(p => p.Toppings)
                .ThenInclude(t => t.Topping)
                .SingleOrDefaultAsync();

            if (Order == null)
            {
                result = NotFound();
            }
            else
            {
                result = Ok(OrderWithStatus.FromOrder(Order));
            }

            return result;
        }

        private async Task SendNotificationAsync(Order order, NotificationSubscription subscription, string message)
        {
            //en una aplication real puedes generar tus propias llaves
            string publicKey ="BLC8GOevpcpjQiLkO7JmVClQjycvTCYWm6Cq_a7wJZlstGTVZvwGFFHMYfXt6Njyvgx_GlXJeo5cSiZ1y4JOx1o";
            string privateKey = "OrubzSz3yWACscZXjFQrrtDwCKg-TGFuWhluQ2wLXDo";

            PushSubscription pushSubscription = new PushSubscription(subscription.Url, subscription.P256dh, subscription.Auth);
            //aqui puedes colocar tu propio correo de informacion
            VapidDetails vapidDetails = new VapidDetails("mailto:someone@example.com", publicKey, privateKey);
            WebPushClient webPushClient = new WebPushClient();
            try
            {
                string payLoad = JsonSerializer.Serialize(new
                {
                    message,
                    url = $"myorders/{order.OrderId}"
                });
                await webPushClient.SendNotificationAsync(pushSubscription, payLoad, vapidDetails);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al enviar la notificacion {e.Message}");
            }

        }

        private async Task TrackAndSendNotificationAsync(Order order, NotificationSubscription notificationSubscription)
        {
            //en un proyecto real otro proceso backend llevaria el seguimiento de la order
            await Task.Delay(OrderWithStatus.PreparationDuration);
            await SendNotificationAsync(order, notificationSubscription, "La orden ya esta en camino");
            await Task.Delay(OrderWithStatus.DeliveryDuration);
            await SendNotificationAsync(order, notificationSubscription, "La orden ya se ha entregado. Disfrutala!!!");
        }
    }
}
