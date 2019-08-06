using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MySiteApi.Filters;
using MySiteApi.Others;
using MySiteApi.Others.Logger;

namespace MySiteApi.Controllers
{
    [Route("api/[controller]")]
    [ServiceFilter(typeof(LogInputsFilter))]
    [ServiceFilter(typeof(IpLockFilter))]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMyLogger logger;
        private readonly IMapper mapper;

        public ValuesController(IMyLogger logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var strings = new string[] { "common value 1", "cool value 2" };
            foreach (var item in strings)
            {
                logger.Log(item);
            }
            return strings;
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
