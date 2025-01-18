using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ClientApplication : IDbEntity
    {
        public string Client_Id { get; set; } = null!;
        public string Client_Secret { get; set; } = null!;
        public string ApplicationName { get; set; } = null!;
        public string ApplicationDescription { get; set; } = null!;
        public string ApplicationPrivacyPolicy { get; set; } = null!;
        public string HomeUrl { get; set; } = null!;
        public List<RedirectUrl> RedirectUrls { get; set; } = null!;
        public List<ApplicationScope> ApplicationScopes { get; set; } = null!;
    }

    public class RedirectUrl : IDbEntity
    {
        public int Id {  get; set; }
        public string Url { get; set; } = null!;
        public string ClientId { get; set; } = null!;

        public ClientApplication ClientApplication { get; set; } = null!;
    }

    public class Scope : IDbEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        //public List<ApplicationScope> ApplicationScopes { get; set; } = null!;

    }
    public class ApplicationScope : IDbEntity
    {
        public int Id { get; set; }

        public int ScopeId { get; set; }
        public Scope Scope { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public ClientApplication ClientApplication { get; set; } = null!;
    }

}
