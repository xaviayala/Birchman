using System;
using Microsoft.SharePoint.Client;
using Birchman.RemoteProvisioning.Domain.Extensibility;
using Birchman.RemoteProvisioning.Domain.Model;
using Birchman.RemoteProvisioning.Utilities;


namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers
{
    /// <summary>
    /// Extensibility Provider CallOut
    /// </summary>
    class ObjectExtensibilityProviders : ObjectHandlerBase
    {
        ExtensibilityManager _extManager = new ExtensibilityManager();

        public override void ProvisionObjects(Web web, ProvisioningTemplate template)
        {
            var _ctx = web.Context as ClientContext;
            foreach(var _provider in template.Providers)
            {
                try
                {
                    _extManager.ExecuteExtensibilityCallOut(_ctx, _provider, template);
                }
                catch(Exception ex)
                {
                    Log.Error(Constants.LOGGING_SOURCE, ex.Message);
                }
            }
        }

        public override ProvisioningTemplate CreateEntities(Web web, ProvisioningTemplate template, ProvisioningTemplateCreationInformation creationInfo)
        {
            // If a base template is specified then use that one to "cleanup" the generated template model
            if (creationInfo.BaseTemplate != null)
            {
                template = CleanupEntities(template, creationInfo.BaseTemplate);
            }

            return template;
        }

        private ProvisioningTemplate CleanupEntities(ProvisioningTemplate template, ProvisioningTemplate baseTemplate)
        {

            return template;
        }
    }
}
