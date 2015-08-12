using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers.TokenDefinitions
{
    public class KeywordsTermStoreIdToken : TokenDefinition
    {
        public KeywordsTermStoreIdToken(Web web)
            : base(web, "~keywordstermstoreid", "{keywordstermstoreid}")
        {
        }

        public override string GetReplaceValue()
        {
            if (CacheValue == null)
            {
                TaxonomySession session = TaxonomySession.GetTaxonomySession(Web.Context);
                var termStore = session.GetDefaultKeywordsTermStore();
                Web.Context.Load(termStore, t => t.Id);
                Web.Context.ExecuteQuery();
                CacheValue = termStore.Id.ToString();
            }
            return CacheValue;
        }
    }
}