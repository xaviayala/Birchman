using Birchman.RemoteProvisioning.Domain.Extensions;
using Microsoft.SharePoint.Client;
using Birchman.RemoteProvisioning.Domain.ObjectHandlers;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers.TokenDefinitions
{
    public class SiteToken : TokenDefinition
    {
        public SiteToken(Web web)
            : base(web, "~site", "{site}")
        {
        }

        public override string GetReplaceValue()
        {
            if (CacheValue == null)
            {
                Web.Context.Load(Web, w => w.ServerRelativeUrl);
                Web.Context.ExecuteQuery();
                CacheValue = Web.ServerRelativeUrl;
            }
            return CacheValue;
        }
    }
}