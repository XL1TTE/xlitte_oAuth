using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using XLITTE_AuthorizationService.Models;

namespace XLITTE_AuthorizationService.Controllers.oauth
{
    [Route("oauth/")]
    [ApiController]
    public class OauthAuthorizationController : ControllerBase
    {
        [HttpPost("authorize")]
        [ProducesResponseType(typeof(AuthorizationQueryParameters), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult AuthorizeUserForApplication(
            [FromQuery] string client_id,
            [FromQuery] string response_type,
            [FromQuery] string state,
            [FromQuery] string redirect_uri
            )
        {
            //bool SuccessResolved = TryResolveAuthorizationRequest(out AuthorizationQueryParameters? result);
            //if (!SuccessResolved)
            //{
            //    return BadRequest(
            //        "Your url should look like " +
            //        "https://example.com/oauth/authorize?client_id=example&response_type=code&scope=profile&state=csrf_token&redirect_uri=https://example.com"
            //        );
            //}
            //return Ok(result);

            var result = new AuthorizationQueryParameters(client_id, response_type, state, redirect_uri);
            return Ok(result);
        }

        private bool TryResolveAuthorizationRequest(out AuthorizationQueryParameters? result)
        {
            result = null;
            if (!TryGetQueryParameter("client_id", out var client_id) ||
                !TryGetQueryParameter("response_type", out var response_type) ||
                !TryGetQueryParameter("state", out var state) ||
                !TryGetQueryParameter("redirect_uri", out var redirect_uri))
            {
                return false;
            }

            result = new AuthorizationQueryParameters(client_id, response_type, state, redirect_uri);
            return true;
        }

        private bool TryGetQueryParameter(string parameterName, out string? value)
        {
            StringValues stringValues;

            if (!HttpContext.Request.Query.TryGetValue(parameterName, out stringValues))
            {
                value = null;
                return false;
            }

            value = stringValues.ToString();
            return true;
        }
    } 
}
