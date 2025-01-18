namespace XLITTE_AuthorizationService.Models
{
    public class AuthorizationQueryParameters
    {
        public string? ClientId { get; set; }
        public string? ResponseType { get; set; }
        public string? State { get; set; }
        public string? RedirectUri { get; set; }

        public AuthorizationQueryParameters(string? clientId, string? responseType, string? state, string? redirectUri)
        {
            ClientId = clientId;
            ResponseType = responseType;
            State = state;
            RedirectUri = redirectUri;
        }

        public AuthorizationQueryParameters() { }
    }

}
