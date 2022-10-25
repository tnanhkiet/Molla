using Microsoft.AspNetCore.Mvc;

public class NewsController : Controller
{
    private readonly MollaContext context;
    public NewsController(MollaContext context)
    {
        this.context = context;
    }
    public ActionResult Index(Guid? categoryId, int page = 1, string ws = "")
    {
        int itemPerPage = 6;

        var categories = context.categories.Where(c => c.ParentId.ToString() == "f6cb50c0-86b9-48bf-bb56-5dd8b2c46d47").Select(c => new List<string>{ c.Id.ToString(), c.Name, context.newsCategories.Where(n => n.CategoryId == c.Id).Count().ToString() }).ToList();
        ViewBag.Categories = categories;
        var rightNews = context.news.OrderByDescending(n => n.CreatedDate).Take(4).ToList();
        ViewBag.RightNews = rightNews;

        var items = context.news.ToList();
        if (categoryId != null)
        {
            items = items.Where(n => context.newsCategories.Where(c => c.CategoryId == categoryId).Select(c => c.NewsId).Contains(n.Id)).ToList();
        }
        if (ws != "")
        {
            items = items.Where(n => n.Title.ToLower().Trim().Contains(ws)).ToList();
        }
        int totalPage = (int)Math.Ceiling((double)items.Count() / itemPerPage);
        ViewBag.TotalPage = totalPage;
        int currentPage = page;
        ViewBag.CurrentPage = currentPage;
        items = items.OrderByDescending(n => n.CreatedDate).Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
        var news = items.Select(i => new List<string>
        {
            i.Picture,
            context.members.Find(i.CreatedBy).Name,
            i.CreatedDate.Value.ToString("dd-MM-yyyy"),
            context.comments.Where(c => c.NewsId == i.Id).Count().ToString(),
            i.Title,
            String.Join(", ", context.categories.Where(b => context.newsCategories.Where(d => d.NewsId == i.Id).Select(d => d.CategoryId).Contains(b.Id)).Select(b => b.Name).ToArray()),
            i.Description,
            i.Id.ToString()
        }).ToList();
        ViewBag.News = news;
        return View();
    }
    public ActionResult Details(Guid Id)
    {
        var categories = context.categories.Where(c => c.ParentId.ToString() == "f6cb50c0-86b9-48bf-bb56-5dd8b2c46d47").Select(c => new List<string>{ c.Id.ToString(), c.Name, context.newsCategories.Where(n => n.CategoryId == c.Id).Count().ToString() }).ToList();
        ViewBag.Categories = categories;
        var rightNews = context.news.OrderByDescending(n => n.CreatedDate).Take(4).ToList();
        ViewBag.RightNews = rightNews;

        var news = context.news.Where(n => n.Id == Id).ToList().Select(i => new List<string>
        {
            i.Picture,
            context.members.Find(i.CreatedBy).Name,
            i.CreatedDate.Value.ToString("dd-MM-yyyy"),
            context.comments.Where(c => c.NewsId == i.Id).Count().ToString(),
            i.Title,
            String.Join(", ", context.categories.Where(b => context.newsCategories.Where(d => d.NewsId == i.Id).Select(d => d.CategoryId).Contains(b.Id)).Select(b => b.Name).ToArray()),
            i.Description,
            i.Id.ToString(),
            i.Content
        }).FirstOrDefault();
        var idCats = context.newsCategories.Where(n => n.NewsId == Id).Select(n => n.CategoryId).ToList();
        var relatedNews = context.news.Where(c => idCats.Contains(context.newsCategories.Where(d => d.NewsId == c.Id).Select(d => d.CategoryId).FirstOrDefault()) && c.Id != Id).OrderByDescending(c => c.CreatedDate).ToList();
        var preNextNews = relatedNews.OrderByDescending(c => c.CreatedDate).ToList();
        int step = 0;
        for (; step < preNextNews.Count; step++)
        {
            if(preNextNews[step].Id == Id)
            {
                break;
            }
        }

        News pre = null, next = null;
        if(relatedNews.Count > 0)
        {
            pre = relatedNews[0];
            next = preNextNews[preNextNews.Count - 1];
        }
        if(step - 1 > 0)
        {
            pre = preNextNews[step - 1];
        }
        if(step + 1 < preNextNews.Count - 1)
        {
            next = preNextNews[step + 1];
        }

        relatedNews = relatedNews.Take(4).ToList();
        ViewBag.RelatedNews = relatedNews;

        ViewBag.Pre = pre;
        ViewBag.Next = next;

        var commentList = context.comments.Where(d => d.NewsId == Id).ToList();
        var comments = commentList.ToList().Select(d => new List<string>
        {
            d.Id.ToString(),
            d.Content,
            d.CreatedDate.Value.ToString("dd-MM-yyyy lúc hh:mm"),
            context.members.Find(d.CreatedBy).Name,
            context.members.Find(d.CreatedBy).Picture,
        }).ToList();
        ViewBag.Comments = comments;

        return View(news);
    }
}