using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokTemplate.DAL;
using PustokTemplate.ViewModels;

namespace PustokTemplate.Services
{
    public class LayoutService
    {
        private readonly PustokDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutService(PustokDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.ToDictionary(x=>x.Key, x => x.Value); 
        }

        public BasketViewModel GetBasket()
        {
            var bv = new BasketViewModel();
            var basketObj = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            if(basketObj != null)
            {
                var serializingBasketObj = JsonConvert.DeserializeObject<List<BasketItemCountViewModel>>(basketObj);

                foreach (var ci in serializingBasketObj)
                {
                    BasketItemViewModel basketItem = new BasketItemViewModel
                    {
                        Count = ci.Count,
                        Book = _context.Books.Include(x => x.Images).FirstOrDefault(x => x.Id == ci.BookId),
                    };
                    bv.BasketItems.Add(basketItem);
                    bv.TotalPrice += (basketItem.Book.DiscountPercent > 0 ? (basketItem.Book.InitialPrice * (100 - basketItem.Book.DiscountPercent) / 100) : basketItem.Book.InitialPrice) * basketItem.Count;
                }
            }
            return bv;
        }
    }
}
