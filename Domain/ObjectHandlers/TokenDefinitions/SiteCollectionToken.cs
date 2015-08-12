using Microsoft.SharePoint.Client;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers.TokenDefinitions
{
    public class SiteCollectionToken : TokenDefinition
    {
        public SiteCollectionToken(Web web)
            : base(web, "~sitecollection", "{sitecollection}")
        {
        }

        public override string GetReplaceValue()
        {
            if (CacheValue == null)
            {
                var context = this.Web.Context as ClientContext;
                var site = context.Site;
                context.Load(site, s => s.RootWeb.ServerRelativeUrl);
                context.ExecuteQuery();
                CacheValue = site.RootWeb.ServerRelativeUrl;
            }
            return CacheValue;
        }
    }
}