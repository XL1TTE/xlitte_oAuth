using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Persistence.EF_Repository;
using System;
using System.Security.AccessControl;
using System.Text.Json;
using XLITTE_AuthorizationService.Models;

namespace XLITTE_AuthorizationService.Controllers.oauth
{
    [Route("oauth/")]
    [ApiController]
    public class OauthAuthorizationController : ControllerBase
    {
        private readonly ClientApplicationsRepository _clients;
        public OauthAuthorizationController(ClientApplicationsRepository clients)
        {
            _clients = clients;
        }

        [HttpGet("authorize")]
        [ProducesResponseType(typeof(object), 200, "application/json")]
        [ProducesResponseType(typeof(string), 400, "application/json")]         
        public IActionResult AuthorizeUserForApplication(
        [FromQuery] string client_id,
            [FromQuery] string response_type,
            [FromQuery] string state,
            [FromQuery] string redirect_uri
        )
        {

            if(VerifyClientRequestWithClientReturn(client_id, redirect_uri, out var client) && client != null)
            {
                var json = new
                {
                    url = $"http://127.0.0.1:8000/SignIn/v1",

                    application_data = new ApplicationData
                    (
                        client.ApplicationName,
                        client.ApplicationDescription,
                        "sadawfa",
                        client.ApplicationScopes
                    )
                };

                var ser_json = JsonSerializer.Serialize(json);

                return Ok(ser_json);
            }
            return BadRequest("Not found!");
       
        }

        private bool VerifyClientRequest(string client_id, string redirect_uri)
        {
            var _client = _clients.GetAll().FirstOrDefault(client => client.Client_Id == client_id);

            if(_client != null && _client.RedirectUrls.Any(item => item.Url == redirect_uri))
            {
                return true;
            }
            return false;
        }

        private bool VerifyClientRequestWithClientReturn(string client_id, string redirect_uri, out ClientApplication? client)
        {
            var _client = _clients.GetAll().FirstOrDefault(client => client.Client_Id == client_id);

            if (_client != null && _client.RedirectUrls.Any(item => item.Url == redirect_uri))
            {
                client = _client;
                return true;
            }
            client = _client;
            return false;
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
