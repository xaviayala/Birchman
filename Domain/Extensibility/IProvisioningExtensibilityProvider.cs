using Microsoft.SharePoint.Client;
using Birchman.RemoteProvisioning.Domain.Model;

namespace Birchman.RemoteProvisioning.Domain.Extensibility
{
    /// <summary>
    /// Defines a interface that accepts requests from the provisioning processing component
    /// </summary>
    public interface IProvisioningExtensibilityProvider
    {
        /// <summary>
        /// Defines a interface that accepts requests from the provisioning processing component
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="template"></param>
        /// <param name="configurationData"></param>
        void ProcessRequest(ClientContext ctx, ProvisioningTemplate template, string configurationData);
    }
}
