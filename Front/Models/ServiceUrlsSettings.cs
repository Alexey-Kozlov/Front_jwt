using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Front.Models
{
    public class ServiceUrlsSettings
    {
        public string AuthorityApiEndpoint { get; set; }
        public string WebApiEndpoint { get; set; }
        public string DefaultRedirectUri { get; set; }
    }
}
