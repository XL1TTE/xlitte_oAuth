using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence.EF_Repository;
using Persistence.Interfaces;
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
        private readonly ClientApplicationsRepository _clientsDB;
        public OauthClientsController(ClientApplicationsRepository clientsDB)
        {
            _clientsDB = clientsDB;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterClientApplication([FromBody] ClientApplicationRegisterData ApplicationData)
        {
            string client_secret = ClientSecretsGenerator.Generate();
            string client_id = $"{ApplicationData.ApplicationName}:{ClientIDGenerator.Generate(ApplicationData.ApplicationDescription)}";

            var json_response = new {
                client_id = client_id,
                client_secret = client_secret,
            };

            List<ApplicationScope> scopes = new List<ApplicationScope>();
            foreach(var item in ApplicationData.Scopes)
            {
                Scope? scope = _clientsDB.GetScopeByName(item);
                if(scope != null)
                {
                    scopes.Add(new ApplicationScope
                    {
                        ScopeId = scope.Id,
                    });
                }
            }

            List<RedirectUrl> redirectUrls = new List<RedirectUrl>();
            foreach(var item in ApplicationData.RedirectURLs)
            {
                redirectUrls.Add(new RedirectUrl
                {
                    ClientId = client_id,
                    Url = item
                });
            }

            ClientApplication client = new ClientApplication
            {
                Client_Id = client_id,
                Client_Secret = client_secret,
                ApplicationName = ApplicationData.ApplicationName,
                ApplicationDescription = ApplicationData.ApplicationDescription,
                ApplicationPrivacyPolicy = ApplicationData.ApplicationPrivacyPolicy,
                HomeUrl = ApplicationData.HomeURL,
                ApplicationScopes = scopes,
                RedirectUrls = redirectUrls
            };

            await _clientsDB.AddEntity(client);

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
