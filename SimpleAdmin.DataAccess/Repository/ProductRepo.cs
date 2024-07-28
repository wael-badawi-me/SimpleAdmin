namespace SimpleAdmin.DataAccess.Repository;
public class ProductRepo : BaseRepo, IRepo<Product, int>
{
    public ProductRepo(DbContextOptions<SimpleadminContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public IQueryable<Product> Get(System.Linq.Expressions.Expression<Func<Product, bool>> predicate, List<string> include = null)
    {
        if (predicate == null)
        {
            predicate = c => 1 == 1;
        }
        IQueryable<Product> query = Context.Product;
        if (include != null)
        {
            foreach (string item in include)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    query= query.Include(item);
                }
            }
        }
        return query.Where(predicate);
    }
    public Task<Product> Get(int Id)
    {
        return Context.Product.FirstOrDefaultAsync(c => c.Id == Id);
    }
    public async Task Save(Product product,int userId)
    {
        product.CreatedBy = userId;
        product.LastModifiedBy = userId;
        List<ProductCategory> currentProductCategories = null;

        if (product.ProductCategory!=null)
        {
            currentProductCategories = product.ProductCategory.ToList();
        
            product.ProductCategory = null; 

        }

        if (product.Id == 0)
        {
            await Insert(product);
        }
        else {
        
           await Update(product);
        }
        foreach (var item in currentProductCategories)
        {
            item.CategoryId = item.Category.Id;
            item.ProductId = product.Id;
            item.Category = null;
        }
        //delete old Product Categories
        await DeleteOldProductCategories(product.Id);
        if (currentProductCategories != null)
        {
            //Add New Product CAtegories
            await InsertProductCategories(currentProductCategories);
        }

    }
    public async Task Update(Product product)
    {
        try
        {
            Product oldProduct = await Get(product.Id);
            if (oldProduct == null)
            {
                throw new ArgumentException("Item with provided Id can't be found");
            }

            product.CreatedBy = oldProduct.CreatedBy;
            product.CreatedDate = oldProduct.CreatedDate;
            product.LastModifiedDate = DateTime.Now;

            Context.Entry(await Context.Set<Product>().FindAsync(product.Id)).State = EntityState.Detached;
            Context.Entry(product).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task Insert(Product product)
    {
        Context.Product.Add(product);
        await Context.SaveChangesAsync();
    }
    public async Task Delete(int Id)
    {
        Product oldProduct = await Get(Id);
        if (oldProduct == null)
        {
            throw new ArgumentException("Item with provided Id can't be found");
        }
        Context.Product.Remove(await Get(Id));
        await Context.SaveChangesAsync();
    }

    private async Task DeleteOldProductCategories(int Id)
    {
       List<ProductCategory> oldProductCategories = Context.ProductCategory.Where(c => c.ProductId == Id).ToList();
        if (oldProductCategories == null)
        {
            return;
        }
        Context.ProductCategory.RemoveRange(oldProductCategories);
        await Context.SaveChangesAsync();
    }

    private async Task InsertProductCategories(List<ProductCategory> productCategories)
    {
        await Context.ProductCategory.AddRangeAsync(productCategories);
        await Context.SaveChangesAsync();
    }

}

