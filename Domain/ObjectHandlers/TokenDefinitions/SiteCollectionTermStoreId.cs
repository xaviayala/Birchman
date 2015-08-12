using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers.TokenDefinitions
{
    public class SiteCollectionTermStoreIdToken : TokenDefinition
    {
        public SiteCollectionTermStoreIdToken(Web web)
            : base(web, "~sitecollectiontermstoreid", "{sitecollectiontermstoreid}")
        {
        }

        public override string GetReplaceValue()
        {
            if (CacheValue == null)
            {
                TaxonomySession session = TaxonomySession.GetTaxonomySession(Web.Context);
                var termStore = session.GetDefaultSiteCollectionTermStore();
                Web.Context.Load(termStore, t => t.Id);
                Web.Context.ExecuteQuery();
                if (termStore != null)
                {
                    CacheValue = termStore.Id.ToString();
                }
            }
            return CacheValue;
        }
    }
}