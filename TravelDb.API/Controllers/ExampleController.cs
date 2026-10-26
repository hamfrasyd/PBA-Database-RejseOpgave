using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelDb.Core.Handlers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelDb.API.Controllers
{
    [Route("api/example")]
    [ApiController]
    [AllowAnonymous]
    public class ExampleController : ControllerBase
    {
        ExampleHandler _exampleHandler;
        public ExampleController(ExampleHandler exampleHandler)
        {
            _exampleHandler = exampleHandler;
        }
        // GET: api/<ExampleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
             _exampleHandler.SomeMethod();

            return new string[] { "value1", "value2" };
        }

        // GET api/<ExampleController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ExampleController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ExampleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ExampleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
