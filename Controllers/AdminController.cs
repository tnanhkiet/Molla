using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.SessionService;

public class AdminController : MiddlewareController
{
    // private MemberService memberService;
    private readonly MollaContext context;
    public AdminController(MollaContext context)
    {
        // this.memberService = memberService;
        this.context = context;
    }
    public ActionResult Index()
    {
        return View();
    }
    public ActionResult Login()
    {
        return View();
    }  
    [HttpPost]
    public ActionResult CheckMember(IFormCollection form)
    {
        string LoginName = form["loginname"], Password = GetMD5Hash(form["password"]);
        var item = context.members.Where(m => m.LoginName == LoginName && m.Password == Password).FirstOrDefault();
        if(item != null)
        {
            HttpContext.Session.Set<Member>("Member", item);
            return RedirectToAction("Index");
        }
        else
        {
            return RedirectToAction("Login");
        }
    }
    public string GetMD5Hash(string input)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
    public ActionResult Logout()
    {
        HttpContext.Session.Remove("Member");
        return RedirectToAction("Login");
    }

    // [HttpPost]
    // public ActionResult Login(string loginname, string password)
    // {
    //     var member = memberService.Login(loginname, password);
    //     if (member != null)
    //     {
    //         HttpContext.Session.SetString("loginname", loginname);
    //         return RedirectToAction("Welcome");
    //     }
    //     else
    //     {
    //         return View("Index");
    //     }
    // }

    // public ActionResult Logout()
    // {
    //     HttpContext.Session.Remove("loginname");
    //     return RedirectToAction("Index");
    // }
    public ActionResult News()
    {
        var categories = context.categories.Where(c => c.ParentId.ToString() == "f6cb50c0-86b9-48bf-bb56-5dd8b2c46d47").ToList();
        ViewBag.Categories = categories;
        return View();
    }
    public JsonResult getNewsCategoriesChecked(Guid? newsId)
    {
        var items = context.newsCategories.Where(p => p.NewsId == newsId).Select(p => p.CategoryId).ToList();
        return Json(items);
    }
    public JsonResult saveNewsCategory(Guid newsId, Guid categoryId)
    {
        var item = context.newsCategories.Where(p => p.NewsId == newsId && p.CategoryId == categoryId).FirstOrDefault();
        if (item == null)
        {
            item = new NewsCategory();
            item.Id = Guid.NewGuid();
            item.NewsId = newsId;
            item.CategoryId = categoryId;
            context.newsCategories.Add(item);
        }
        else
        {
            context.newsCategories.Remove(item);
        }
        context.SaveChanges();
        return Json("");
    }
    public JsonResult getNews()
    {
        var items = context.news.Select(n => new List<string> {
            n.Id.ToString(),
            n.Title,
            n.CreatedDate.Value.ToString("dd-MM-yyyy"),
            context.members.Where(m => m.Id == n.CreatedBy).FirstOrDefault().Name
        });
        return Json(items);
    }
    public ActionResult EditNews(Guid? id)
    {
        News item = new News();
        if (id != null)
        {
            item = context.news.Find(id);
        }
        return View(item);
    }
    [HttpPost]
    public ActionResult SaveNews(IFormCollection form)
    {
        try
        {
            Guid id = Guid.Parse(form["id"]);
            News item = new News();
            if (id == Guid.Empty)
            {
                item = new News();
                item.Id = Guid.NewGuid();
                item.CreatedDate = DateTime.Now;
                item.CreatedBy = member.Id;
                context.news.Add(item);
            }
            else
            {
                item = context.news.Find(id);
            }
            item.Title = form["title"];
            item.Description = form["description"];
            IFormFile file = Request.Form.Files[0];
            if (file != null && file.Length > 0)
            {
                item.Picture = file.FileName.Substring(file.FileName.LastIndexOf(@"/") + 1);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/news", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            item.Content = form["content"];
            context.SaveChanges();
        }
        catch { }
        return RedirectToAction("News");
    }
    public ActionResult RemoveNews(Guid id)
    {
        try
        {
            var item = context.news.Find(id);
            context.news.Remove(item);
            context.SaveChanges();
        }
        catch { }
        return RedirectToAction("News");
    }
    public ActionResult Product()
    {
        var categories = context.categories.Where(c => c.ParentId.ToString() == "b52746f0-0500-4a36-9310-329961094830").ToList();
        ViewBag.Categories = categories;
        return View();
    }
    public JsonResult getProductCategoriesChecked(Guid? productId)
    {
        var items = context.productCategory.Where(p => p.ProductId == productId).Select(p => p.CategoryId).ToList();
        return Json(items);
    }
    public JsonResult saveProductCategory(Guid productId, Guid categoryId)
    {
        var item = context.productCategory.Where(p => p.ProductId == productId && p.CategoryId == categoryId).FirstOrDefault();
        if (item == null)
        {
            item = new ProductCategory();
            item.Id = Guid.NewGuid();
            item.ProductId = productId;
            item.CategoryId = categoryId;
            context.productCategory.Add(item);
        }
        else
        {
            context.productCategory.Remove(item);
        }
        context.SaveChanges();
        return Json("");
    }
    public JsonResult getProducts()
    {
        var items = context.product.ToList().Select(p => new List<string> {
            p.Id.ToString(),
            p.Picture.Split(',')[0],
            p.Name,
            context.brand.Find(p.BrandId).Name,
            p.CreatedDate.ToString(),
            context.members.Find(p.CreatedBy).Name,
        });
        return Json(items);
    }
    public ActionResult EditProduct(Guid? id)
    {
        Product item = new Product();
        if (id != null)
        {
            item = context.product.Find(id);
        }
        var brands = new SelectList(context.brand.ToList(), "Id", "Name");
        ViewBag.Brands = brands;
        return View(item);
    }
    [HttpPost]
    public ActionResult SaveProduct(IFormCollection form, List<IFormFile> files)
    {
        Guid Id = Guid.Parse(form["id"]);
        Product product = new Product();
        if (Id == Guid.Empty)
        {
            product.Id = Guid.NewGuid();
            product.CreatedDate = DateTime.Now;
            product.CreatedBy = member.Id;
            context.product.Add(product);
        }
        product.Name = form["name"];
        product.Description = form["description"];
        product.Information = form["information"];
        product.Additional = form["additional"];
        product.Shipping = form["shipping"];
        product.BrandId = Guid.Parse(form["brandId"]);
        List<string> picture = new List<string>();
        foreach (IFormFile file in files)
        {
            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/products", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                    product.Picture += fileName + ", ";
                }
            }
        }
        context.SaveChanges();
        return RedirectToAction("Product");
    }
    public ActionResult RemoveProduct(Guid id)
    {
        context.productVoting.RemoveRange(context.productVoting.Where(p => p.ProductId == id).ToList());
        List<string> picture_product_price = context.productPrice.Where(p => p.ProductId == id).Select(p => p.Picture).ToList();
        context.productPrice.RemoveRange(context.productPrice.Where(p => p.ProductId == id).ToList());
        foreach (var item in picture_product_price)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/products", item);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        context.productCategory.RemoveRange(context.productCategory.Where(p => p.ProductId == id).ToList());
        var product = context.product.Find(id);
        string[] picture_product = product.Picture.Split(", ");
        for (int i = 0; i < picture_product.Length; i++)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/products", picture_product[i]);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
        context.product.Remove(product);
        context.SaveChanges();
        return RedirectToAction("Product");
    }
    public ActionResult ProductPrice(Guid id)
    {
        return View(id);
    }
    public JsonResult getPrices(Guid productId)
    {
        var items = context.productPrice.Where(p => p.ProductId == productId).ToList().Select(p => new List<string>{
            p.Id.ToString(),
            p.Picture,
            context.color.Find(p.ColorId).Name,
            context.size.Find(p.SizeId).Name,
            p.Price.ToString(),
            p.Amount.ToString()
        });
        return Json(items);
    }
    public ActionResult EditPrices(Guid? Id, Guid productId)
    {
        ProductPrice item = new ProductPrice();
        item.ProductId = productId;
        if (Id != null)
        {
            item = context.productPrice.Find(Id);
        }
        var colors = new SelectList(context.color.ToList(), "Id", "Name");
        ViewBag.Color = colors;
        var sizes = new SelectList(context.size.Where(s => s.ParentId.ToString() == "c5bb1a44-07b0-4147-be04-b407a558e3ba").ToList(), "Id", "Name");
        ViewBag.Size = sizes;
        return View(item);
    }
    [HttpPost]
    public ActionResult SavePrices(IFormCollection form)
    {
        Guid Id = Guid.Parse(form["id"]);
        ProductPrice item = new ProductPrice();
        if (Id == Guid.Empty)
        {
            item.Id = Guid.NewGuid();
            context.productPrice.Add(item);
        }
        else
        {
            item = context.productPrice.Find(Id);
        }
        item.ProductId = Guid.Parse(form["productId"]);
        item.SizeId = Guid.Parse(form["sizeId"]);
        item.ColorId = Guid.Parse(form["colorId"]);
        item.Price = decimal.Parse(form["price"]);
        item.Amount = int.Parse(form["amount"]);
        IFormFile file = Request.Form.Files[0];
        if (file != null && file.Length > 0)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/products", file.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
                item.Picture = file.FileName;
            }
        }
        context.SaveChanges();
        return RedirectToAction("ProductPrice", new {Id = item.ProductId});
    }
    public ActionResult RemovePrices(Guid id)
    {
        ProductPrice products = new ProductPrice();
        // HttpContext.Session.Set<ProductPrice>("products", products);
        products = context.productPrice.Find(id);
        try
        {
            var item = context.productPrice.Find(id);
            context.productPrice.Remove(item);
            context.SaveChanges();
        }
        catch { }
        return RedirectToAction("ProductPrice", new {Id = products.ProductId});
    }
}