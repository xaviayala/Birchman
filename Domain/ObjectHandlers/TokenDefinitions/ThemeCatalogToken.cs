using Birchman.RemoteProvisioning.Domain.Extensions;
using Microsoft.SharePoint.Client;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers.TokenDefinitions
{
    public class ThemeCatalogToken : TokenDefinition
    {
        public ThemeCatalogToken(Web web)
            : base(web, "~themecatalog","{themecatalog}")
        {
        }

        public override string GetReplaceValue()
        {
            if (CacheValue == null)
            {
                using (ClientContext cc = Web.Context.GetSiteCollectionContext())
                {
                    var catalog = cc.Web.GetCatalog((int)ListTemplateType.ThemeCatalog);
                    cc.Load(catalog, c => c.RootFolder.ServerRelativeUrl);
                    cc.ExecuteQuery();
                    CacheValue = catalog.RootFolder.ServerRelativeUrl;
                }
            }
            return CacheValue;
        }
    }
}