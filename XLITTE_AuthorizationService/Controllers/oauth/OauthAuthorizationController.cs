using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System;
using System.Security.AccessControl;
using XLITTE_AuthorizationService.Models;

namespace XLITTE_AuthorizationService.Controllers.oauth
{
    [Route("oauth/")]
    [ApiController]
    public class OauthAuthorizationController : ControllerBase
    {
        [HttpGet("authorize")]
        [ProducesResponseType(typeof(AuthorizationQueryParameters), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult AuthorizeUserForApplication(
            [FromQuery] string client_id,
            [FromQuery] string response_type,
            [FromQuery] string state,
            [FromQuery] string redirect_uri
        )
        {

            var json = new
            {
                url = "http://127.0.0.1:8000/SignIn/v1"
            };

            return Ok(json);


            //return Redirect($"http://127.0.0.1:8000/oauth/authorize?client_id={client_id}&response_type={response_type}&scope=profile&redirect_uri={redirect_uri}");
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
