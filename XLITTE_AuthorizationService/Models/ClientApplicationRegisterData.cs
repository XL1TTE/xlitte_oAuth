namespace XLITTE_AuthorizationService.Models
{
    public record ClientApplicationRegisterData(
        string ApplicationName,
        string ApplicationDescription,
        string ApplicationPrivacyPolicy,
        string HomeURL,
        List<string> RedirectURLs
        );
}
