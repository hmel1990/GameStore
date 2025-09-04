using GameStore.Interfaces;
using GameStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Components
{
    public class TopViewComponent : ViewComponent
    {
        private readonly IOrder _order;
        public TopViewComponent (IOrder order)
        {
            _order = order;
        }


        public IViewComponentResult Invoke()
        {
            var topProducts = _order.GetTopOrders();
            return View(topProducts);
        }

    }
}
