using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DataProvider.Web.Services;

namespace DataProvider.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly RoleClaimsProvider roleClaimsProvider;

        public ValuesController(RoleClaimsProvider roleClaimsProvider)
        {
            this.roleClaimsProvider = roleClaimsProvider;
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get(CancellationToken cancellationToken)
        {
            var roleClaims = await roleClaimsProvider.GetDataAsync(cancellationToken);
            return new string[] { "value1", "value2", roleClaims.ToString() };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
