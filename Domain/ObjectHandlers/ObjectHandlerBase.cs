using Birchman.RemoteProvisioning.Domain.Model;
using Microsoft.SharePoint.Client;

namespace Birchman.RemoteProvisioning.Domain.ObjectHandlers
{
    public abstract class ObjectHandlerBase
    {
        public abstract void ProvisionObjects(Web web, ProvisioningTemplate template);

        public abstract ProvisioningTemplate CreateEntities(Web web, ProvisioningTemplate template, ProvisioningTemplateCreationInformation creationInfo);
    }
}
