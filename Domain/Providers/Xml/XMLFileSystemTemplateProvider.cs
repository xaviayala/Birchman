using Birchman.RemoteProvisioning.Domain.Connectors;

namespace Birchman.RemoteProvisioning.Domain.Providers.Xml
{
    public class XMLFileSystemTemplateProvider : XMLTemplateProvider
    {
        public XMLFileSystemTemplateProvider(string connectionString, string container) :
            base(new FileSystemConnector(connectionString, container))
        {

        }
    }
}
