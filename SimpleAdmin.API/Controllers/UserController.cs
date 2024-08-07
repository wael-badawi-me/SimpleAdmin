namespace SimpleAdmin.API.Controllers;
[ApiController]
[Route("[controller]/[action]")]
[AutoLog, Authorize]
public class UserController : ControllerBase
{
    public DataAccess.Repository.IRepo<User, int> _repo { get; }

    public UserController(IRepo<User,int> repo)
    {
        _repo = repo;
    }

    [HttpGet("{skip}/{take}")]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(List<User>))]
    
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
        IQueryable<User> query = GetQuery(filters, filtertxt);
        return Ok(query.Skip(skip).Take(take).Select(c => new UserModel(c)).ToList());
    }
  
    [HttpGet]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(int))]
    public IActionResult GetCount(string filters , string filtertxt)
    {
        IQueryable<User> query = GetQuery(filters, filtertxt);
        return Ok(query.Count());
    }
    [HttpGet("{id}")]
    [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(UserModel))]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(new UserModel(await _repo.Get(id)));
    }
    [HttpPost]
    public async Task<IActionResult> Save([FromBody] UserModel user)
    {
        if (!ModelState.IsValid)
        {
            throw new ArgumentException(ModelState.GetErrorMessages());
        }
        
        User exUser = await _repo.Get(user.id);
        if (exUser == null|| user.password != exUser.Password)
        {
            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
        }
        await _repo.Save(user.DBUser, AuthorizeAttribute.AuthUser.User.id );
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.Delete(id);
        return Ok();
    }
    
    private IQueryable<User> GetQuery(string filters, string filtertxt)
    {
        List<string> filtelst = new List<string>();
        if (!string.IsNullOrEmpty(filters))
        {
            filtelst = filters.ToLower().Split(';').ToList();
        }
        IQueryable<User> query = _repo.Get(c => 1 == 1);
        if (filtelst != null && filtelst.Count != 0 && !string.IsNullOrEmpty(filtertxt))
        {
            foreach (string item in filtelst)
            {
                switch (item)
                {
                    case "fullname":
                        query = query.Where(c => c.FullName.ToLower().Contains(filtertxt.ToLower()));
                        break;
                    case "email":
                        query = query.Where(c => c.Email.ToLower().Contains(filtertxt.ToLower()));
                        break;
                    default:
                        if (item.Trim() != "")
                        {
                            throw new ArgumentException("The filters parameter should have either fullname, email or it is combination separated by ;");
                        }
                        break;
                }
            }
        }

        return query;
    }
}
