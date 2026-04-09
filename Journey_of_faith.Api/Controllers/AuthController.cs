using Journey_of_faith.Application.usecases.auth.queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Journey_of_faith.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _me;
        public AuthController(IMediator me)
        {
            _me = me;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginQuery query)
        {
            var login = await _me.Send(query);
            return Ok(login);
        }
    }
}
