using BlazingPizza.Server.Models;
using BlazingPizza.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazingPizza.Server.Controllers
{
    [Route("notifications")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly PizzaStoreContext Context;

        public NotificationsController(PizzaStoreContext context) => 
            Context = context;

        [HttpPut("subscribe")]
        public async Task<NotificationSubscription> Subscribe(NotificationSubscription subscription)
        {
            //Almacenar como maximo una suscripcion por usuario.
            //por lo tanto eliminamos las antiguas
            //opcionalmente podriamos permitir que usuario guarden distintas subscripciones 
            //por dispositivos o navegadores
            string userId = GetUserId();
            var OldSubscription = Context.NotificationSubscriptions.Where(e => e.UserId == userId);
            Context.NotificationSubscriptions.RemoveRange(OldSubscription);
            subscription.UserId = userId;
            Context.NotificationSubscriptions.Attach(subscription);
            await Context.SaveChangesAsync();
            return subscription;
        }

        private string GetUserId() => 
            HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
