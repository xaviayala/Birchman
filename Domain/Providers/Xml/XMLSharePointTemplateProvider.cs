using Microsoft.SharePoint.Client;
using Birchman.RemoteProvisioning.Domain.Connectors;

namespace Birchman.RemoteProvisioning.Domain.Providers.Xml
{
    public class XMLSharePointTemplateProvider : XMLTemplateProvider
    {

        public XMLSharePointTemplateProvider(ClientRuntimeContext cc, string connectionString, string container) :
            base(new SharePointConnector(cc, connectionString, container))
        {

        }
    }
}
