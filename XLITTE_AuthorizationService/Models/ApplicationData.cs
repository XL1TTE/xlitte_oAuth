using Domain;

namespace XLITTE_AuthorizationService.Models
{
    [Serializable]
    public class ApplicationData
    {
        public string app_name {  get; set; }
        public string app_desc { get; set; }
        public string app_policy {  get; set; }
        private List<string> app_scopes {  get; set; }


        public ApplicationData(string app_name, string app_desc, string app_policy, List<ApplicationScope> Scopes)
        {
            this.app_name = app_name;
            this.app_desc = app_desc;
            this.app_policy = app_policy;
            app_scopes = RetriveScopesNames(Scopes);
        }
        private List<string> RetriveScopesNames(List<ApplicationScope> Scopes)
        {
            var scopes = new List<string>();    
            foreach(var scope in Scopes)
            {
                scopes.Add(scope.Scope.Name);
            }
            return scopes;
        }
    }
}
