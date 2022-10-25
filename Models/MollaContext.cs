using Microsoft.EntityFrameworkCore;

public class MollaContext : DbContext
{
    public MollaContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Category> categories { get; set; }
    public DbSet<News> news { get; set; }
    public DbSet<NewsCategory> newsCategories { get; set; }
    public DbSet<Comment> comments { get; set; }
    public DbSet<Member> members { get; set; }
    public DbSet<Size> size { get; set; }
    public DbSet<Color> color { get; set; }
    public DbSet<Brand> brand { get; set; }
    public DbSet<Product> product { get; set; }
    public DbSet<ProductCategory> productCategory { get; set; }
    public DbSet<ProductPrice> productPrice { get; set; }
    public DbSet<ProductVoting> productVoting { get; set; }
}