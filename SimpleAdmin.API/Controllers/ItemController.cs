using Microsoft.EntityFrameworkCore;

namespace SimpleAdmin.API.Controllers;
[ApiController]
[Route("[controller]/[action]")]
[AutoLog,Authorize]
public class ItemController : ControllerBase
{
    public DataAccess.Repository.IRepo<Product, int> _repo { get; }

    public ItemController(IRepo<Product, int> repo)
    {
        _repo = repo;
    }

    [HttpGet("{skip}/{take}")]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(List<Product>))]

    public IActionResult Get(int skip, int take, string filters = "", string filtertxt = "")
    {
        if (skip < 0 || take < 0)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, "skip/take parameter value should be a positive integer");
        }
        if (take > 500)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, "take parameter value should be a less than 500");
        }
        IQueryable<Product> query = GetQuery(filters, filtertxt);
        return Ok(query.Skip(skip).Take(take).Select(c => new ProductModel(c)).ToList());
    }
  
    [HttpGet]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(int))]
    public IActionResult GetCount(string filters , string filtertxt)
    {
        IQueryable<Product> query = GetQuery(filters, filtertxt);
        return Ok(query.Count());
    }
    [HttpGet("{id}")]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProductModel))]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(new ProductModel(await _repo.Get(c=>c.Id==id,new List<string> { "ProductCategory.Category" }).FirstOrDefaultAsync()));
    }
    [HttpPost]
    public async Task<IActionResult> Save([FromBody] ProductModel Item)
    {

        ModelState.RemoveFor<ProductModel>(c => c.ProductCategories);
        if (!ModelState.IsValid)
        {
            throw new ArgumentException(ModelState.GetErrorMessages());
        }
        await _repo.Save(Item.DBProduct, AuthorizeAttribute.AuthUser.User.id);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.Delete(id);
        return Ok();
    }
    
    private IQueryable<Product> GetQuery(string filters, string filtertxt)
    {
        List<string> filtelst = new List<string>();
        if (!string.IsNullOrEmpty(filters))
        {
            filtelst = filters.ToLower().Split(';').ToList();
        }
        IQueryable<Product> query = _repo.Get(c => 1 == 1,new List<string> { "ProductCategory.Category" });
        if (filtelst != null && filtelst.Count != 0 && !string.IsNullOrEmpty(filtertxt))
        {
            foreach (string item in filtelst)
            {
                switch (item)
                {
                    case "title":
                        query = query.Where(c => c.Title.ToLower().Contains(filtertxt.ToLower()));
                        break;
                    case "categoryname":
                        query = query.Where(c => c.ProductCategory.Any(pc => pc.Category.Name.ToLower().Contains(filtertxt.ToLower())));
                        break;
                    case "seotitle":
                        query = query.Where(c => c.Seotitle.ToLower().Contains(filtertxt.ToLower()));
                        break;
                    case "url":
                        query = query.Where(c => c.Url.ToLower().Contains(filtertxt.ToLower()));
                        break;
                    default:
                        if (item.Trim() != "")
                        {
                            throw new ArgumentException("The filters parameter should have either title, categoryname, seotitle, url or it is combination separated by ;");
                        }
                        break;
                }
            }
        }

        return query;
    }
}
