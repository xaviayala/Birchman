using Microsoft.SharePoint.Client;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers.TokenDefinitions
{
    public class MasterPageCatalogToken : TokenDefinition
    {
        public MasterPageCatalogToken(Web web)
            : base(web, "~masterpagecatalog","{masterpagecatalog}")
        {
        }

        public override string GetReplaceValue()
        {
            if (this.CacheValue == null)
            {
                var catalog = Web.GetCatalog((int) ListTemplateType.MasterPageCatalog);
                Web.Context.Load(catalog, c => c.RootFolder.ServerRelativeUrl);
                Web.Context.ExecuteQuery();
                CacheValue = catalog.RootFolder.ServerRelativeUrl;
            }
            return CacheValue;
        }
    }
}