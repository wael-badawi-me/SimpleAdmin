namespace SimpleAdmin.Model;
public class ProductModel
{
    private Product product;
    public ProductModel()
    {
        product = new Product();
    }
    public ProductModel(Product initItem)
    {
        product = initItem;
    }
    [JsonIgnore]
    public Product  DBProduct
    {
        get { return product; }
    }
    public int id
    {
        get { return product.Id; }
        set { product.Id = value; }
    }
    [Required]
    public string title
    {
        get { return product.Title; }
        set { product.Title = value; }
    }
  
   
    [Required]
    public string description
    {
        get { return product.Description; }
        set { product.Description = value; }
    }
    [Required]
    public string summary
    {
        get { return product.Summary; }
        set { product.Summary = value; }
    }
    public bool isEnabled
    {
        get { return product.IsEnabled; }
        set { product.IsEnabled = value; }
    }
    public string Photo
    {
        get { return product.Photo; }
        set { product.Photo = value; }
    }
    public string Seodescription
    {
        get { return product.Seodescription; }
        set { product.Seodescription = value; }
    }
    
    public string seokeywords
    {
        get { return product.Seokeywords; }
        set { product.Seokeywords = value; }
    }
    
    public string seotitle
    {
        get { return product.Seotitle; }
        set { product.Seotitle = value; }
    }
    public string Url
    {
        get { return product.Url; }
        set { product.Url = value; }
    }
    public List<CategoryModel> ProductCategories
    {
        get
        {
            return product.ProductCategory.Select(c => new CategoryModel(c.Category)).ToList();

        }
        set
        {
            product.ProductCategory = new List<ProductCategory>();
            foreach (CategoryModel categoryModelitem in value)
            {
                product.ProductCategory.Add(new ProductCategory { Category = categoryModelitem.DBCategory });
            }
        }
    }
}
