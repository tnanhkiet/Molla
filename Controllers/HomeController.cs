using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.SessionService;

namespace KShop.Controllers;

public class HomeController : Controller
{
    private readonly MollaContext context;
    public HomeController(MollaContext context)
    {
        this.context = context;
    }

    public ActionResult Index()
    {
        var items = context.categories.Where(c => c.ParentId.ToString() == "b52746f0-0500-4a36-9310-329961094830").OrderBy(c => c.Name).ToList();
        var categories = items.Select(i => new List<string>
        {
            i.Id.ToString(),
            i.Name,
            context.productCategory.Where(p => p.CategoryId == i.Id).Count().ToString()
        }).ToList();
        ViewBag.Categories = categories;

        var sizes = context.size.Where(s => s.ParentId.ToString() == "c5bb1a44-07b0-4147-be04-b407a558e3ba").OrderBy(s => s.Name).ToList();
        ViewBag.Sizes = sizes;

        var colors = context.color.ToList();
        ViewBag.Colors = colors;

        var brands = context.brand.ToList();
        ViewBag.Brands = brands;

        return View();
    }
    public ActionResult Details(Guid id)
    {
        var db = context;
        ViewBag.DB = db;
        var product = context.product.Find(id);
        return View(product);
    }
    public JsonResult getPrices(Guid productId, Guid colorId, Guid sizeId)
    {
        var item = context.productPrice.Where(p => p.ProductId == productId && p.ColorId == colorId && p.SizeId == sizeId).FirstOrDefault();
        if (item == null)
        {
            item = new ProductPrice();
        }
        return Json(item);
    }
    public JsonResult Products(List<Guid?> categories, List<Guid?> sizes, Guid? color, List<Guid?> brands, decimal priceFrom = 0, decimal priceTo = 0, string orderBy = "Date", int page = 1)
    {
        int itemsPerPage = 6;
        var items = context.product.ToList();
        if (categories.Count > 0)
        {
            items = items.Where(p => context.productCategory.Where(c => categories.Contains(c.CategoryId)).Select(c => c.ProductId).Contains(p.Id)).ToList();
        }
        if (sizes.Count > 0)
        {
            items = items.Where(p => context.productPrice.Where(c => sizes.Contains(c.SizeId)).Select(c => c.ProductId).Contains(p.Id)).ToList();
        }
        if (color != null)
        {
            items = items.Where(p => context.productPrice.Where(c => c.ColorId == color).Select(c => c.ProductId).Contains(p.Id)).ToList();
        }
        if (brands.Count > 0)
        {
            items = items.Where(p => brands.Contains(p.BrandId)).ToList();
        }
        if (priceFrom != null)
        {
            items = items.Where(p => context.productPrice.Where(c => (c.Price >= priceFrom && c.Price <= priceTo)).Select(c => c.ProductId).Contains(p.Id)).ToList();
        }
        switch (orderBy)
        {
            case "Date":
                items = items.OrderByDescending(p => p.CreatedDate).ToList();
                break;
            case "Rated":
                var productVote = (from v in context.productVoting
                                   group v by new
                                   {
                                       v.ProductId
                                   } into g
                                   select new
                                   {
                                       g.Key,
                                       VoteAvg = g.Average(p => p.Star)
                                   }
                                    ).ToList();
                var data = (from a in productVote
                            join b in items on a.Key.ProductId equals b.Id
                            orderby a.VoteAvg descending
                            select b).ToList();
                items.Clear();
                items.AddRange(data);
                break;
        }
        int totalItems = items.Count();
        items = items.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
        Dictionary<string, object> myJson = new Dictionary<string, object>();
        var products = items.Select(i => new List<string>
        {
            i.Id.ToString(),
            i.Picture,
            string.Join(", ", context.categories.Where(c => context.productCategory.Where(d => d.ProductId == i.Id).Select(d => d.CategoryId).Contains(c.Id)).Select(c => c.Name).ToArray()),
            i.Name,
            i.Description,
            string.Join(", ", context.productPrice.Where(p => p.ProductId == i.Id).Select(p => p.Picture).ToArray()),
            ((Double)context.productPrice.Where(p => p.ProductId == i.Id).Min(p => p.Price)).ToString(),
            context.productVoting.Where(p => p.ProductId == i.Id).Count().ToString(),
            context.productVoting.Where(p => p.ProductId == i.Id).Average(p => p.Star).ToString()
        });
        myJson.Add("Products", products);
        myJson.Add("TotalItems", totalItems);
        int totalPages = totalItems / itemsPerPage;
        if (totalPages * itemsPerPage < totalItems)
            totalPages++;
        myJson.Add("TotalPages", totalPages);
        return Json(myJson);
    }
    public JsonResult getCart()
    {
        List<myCart> myProducts = new List<myCart>();
        if (HttpContext.Session.Get<List<myCart>>("Cart") != null)
        {
            myProducts = HttpContext.Session.Get<List<myCart>>("Cart");
        }
        return Json(myProducts);
    }
    List<myCart> getCartItems () {

        string jsoncart = HttpContext.Session.GetString ("Cart");
        if (jsoncart != null) {
            return JsonConvert.DeserializeObject<List<myCart>> (jsoncart);
        }
        return new List<myCart> ();
    }
    public JsonResult addCart(Guid productId, Guid colorId, Guid sizeId, int amount = 1)
    {
        List<myCart> myProducts = new List<myCart>();
        if (HttpContext.Session.Get<List<myCart>>("Cart") != null)
        {
            myProducts = HttpContext.Session.Get<List<myCart>>("Cart");
        }
        var product = context.product.Find(productId);
        var productPrice = context.productPrice.Where(p => p.ProductId == productId && p.ColorId == colorId && p.SizeId == sizeId).FirstOrDefault();
        if (productPrice != null)
        {
            myProducts.Add(new myCart()
            {
                _product = product,
                _productPrice = productPrice,
                _amount = amount
            });
            HttpContext.Session.Set<List<myCart>>("Cart", myProducts);
        }
        return Json("");
    }
    public JsonResult removeProduct(Guid productId, Guid colorId, Guid sizeId, int amount)
    {
        var cart = getCartItems();
        var cartitem = cart.Find(p => p._product.Id == productId && p._productPrice.ColorId == colorId && p._productPrice.SizeId == sizeId && p._amount == amount);
        if (cartitem != null)
        {
            cart.Remove(cartitem);
        }
        HttpContext.Session.Set<List<myCart>>("Cart", cart);
        return Json("");
    }
    public JsonResult removeCart()
    {
        HttpContext.Session.Remove("Cart");
        return Json("");
    }
    public class myCart
    {
        public ProductPrice _productPrice { get; set; }
        public Product _product { get; set; }
        public int _amount { get; set; }
    }
    public ActionResult Pay()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }
}
