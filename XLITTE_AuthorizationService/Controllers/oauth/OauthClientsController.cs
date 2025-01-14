using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using XLITTE_AuthorizationService.Models;
using XLITTE_AuthorizationService.Services;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace XLITTE_AuthorizationService.Controllers.oauth
{
    [Route("oauth/Clients/")]
    [ApiController]
    public class OauthClientsController : ControllerBase
    {
        [HttpPost("Register")]
        public IActionResult RegisterClientApplication([FromBody] ClientApplicationRegisterData ApplicationData)
        {
            string client_secret = ClientSecretsGenerator.Generate();
            string client_id = $"{ApplicationData.ApplicationName}:{ClientIDGenerator.Generate(ApplicationData.ApplicationName)}";

            var json_response = new {
                client_id = client_id,
                client_secret = client_secret,
            };

            // Also we need to store client credentials in data base
            // In case we want to use one-time-toshow flow for client_secret we will need to hash the secret and store hashed version.
            // In data base we'll store all the information provided by client throght the ClienApplicationRegisterData scheme!
            
            // Then we should make authentification form for user that will be acces by the followed link
            // https://localhost:8080/oauth/authorize?client_id=example&response_type=code&scope=profile&state=csrf_token&redirect_uri=https://example.com

            //Endpoint to authorization_code_token exchange with client application authentification with hashing.

            return Ok(json_response);
        }
    }
}
